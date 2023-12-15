using AnagramSolver.BackgroundJobs;
using AnagramSolver.Data;
using AnagramSolver.Exceptions;
using AnagramSolver.HttpClients;
using EntityFramework.Exceptions.Common;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using static AnagramSolver.BackgroundJobs.WikiDataImport.ImportCelebrityRequestsProcessorJob;
using static AnagramSolver.BackgroundJobs.WikiDataImport.ImportCelebrityRequestsSchedulerJob;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseShutdownTimeout(TimeSpan.FromSeconds(55));
builder.WebHost.UseSentry(o => o.SetBeforeSend((sentryEvent, hint) =>
{
    if (sentryEvent.Exception is BusinessRuleViolationException)
    {
        sentryEvent.Level = Sentry.SentryLevel.Info;
    }

    return sentryEvent;
}));

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AnagramSolverContext>(options =>
            options.UseNpgsql(builder.Configuration.GetValue<string>("CONNECTION_STRING")));

builder.Services.Scan(scan => scan.FromCallingAssembly()
    .AddClasses(classes => classes.AssignableTo(typeof(IJob<>)).Where(_ => !_.IsGenericType))
    .AsImplementedInterfaces()
    .WithTransientLifetime());

builder.Services.Decorate(typeof(IJob<>), typeof(TracingJobDecorator<>));

builder.Services.AddHttpClient<WikiDataHttpClient>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
        options.SlidingExpiration = true;
        options.Events.OnRedirectToAccessDenied = c =>
        {
            c.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.FromResult<object?>(null);
        };
        options.Events.OnRedirectToLogin = c =>
        {
            c.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.FromResult<object?>(null);
        };
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddHangfire(config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                             .UseSimpleAssemblyNameTypeSerializer()
                                             .UseRecommendedSerializerSettings()
                                             .UsePostgreSqlStorage(builder.Configuration.GetValue<string>("CONNECTION_STRING")));

builder.Services.AddHangfireServer(config =>
{
    config.ShutdownTimeout = TimeSpan.FromSeconds(50);
    config.StopTimeout = TimeSpan.FromSeconds(45);
    config.ServerTimeout = TimeSpan.FromSeconds(45);
});

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        // using static System.Net.Mime.MediaTypeNames;
        context.Response.ContentType = Text.Plain;

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is BusinessRuleViolationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Business rule violation: {exceptionHandlerPathFeature.Error.Message}!");
        }
        else if (exceptionHandlerPathFeature?.Error is UniqueConstraintException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Entity with the same unique key already exists!");
        }
        else if (exceptionHandlerPathFeature?.Error is not null)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            if (app.Environment.IsDevelopment())
            {
                await context.Response.WriteAsync($"Internal server error: {exceptionHandlerPathFeature.Error}");
            }
            else 
            {
                await context.Response.WriteAsync($"Something went wrong");
            }
        }
    });
});

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseSentryTracing();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/background-jobs", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthFilter() }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

RecurringJob.AddOrUpdate<IJob<ImportCelebrityRequestsSchedulerJobData>>("ImportWikiDataCelebrityRequestsSchedulerJob", x => x.ExecuteAsync(new ImportCelebrityRequestsSchedulerJobData()), Cron.Minutely);
RecurringJob.AddOrUpdate<IJob<ImportCelebrityRequestsProcessorJobData>>("ProcessImportWikiDataCelebrityRequestsJob", x => x.ExecuteAsync(new ImportCelebrityRequestsProcessorJobData()), Cron.Minutely);

app.Run();
