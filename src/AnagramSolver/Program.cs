using AnagramSolver.BackgroundJobs;
using AnagramSolver.BackgroundJobs.WikiDataImport;
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
using Sentry;

var sentryDSN = Environment.GetEnvironmentVariable("SENTRY_DSN");

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseShutdownTimeout(TimeSpan.FromSeconds(55));

if (sentryDSN is not null)
{
    // This option is recommended. It enables Sentry's "Release Health" feature.
    var autoSessionTracking = true;
    // Enabling this option is recommended for client applications only. It ensures all threads use the same global scope.
    var isGlobalModeEnabled = false;
    // This option will enable Sentry's tracing features. You still need to start transactions and spans.
    var enableTracing = true;
    // Example sample rate for your transactions: captures 10% of transactions
    var tracesSampleRate = 1.0;

    SentrySdk.Init(options =>
    {
        options.Dsn = sentryDSN;
        options.AutoSessionTracking = autoSessionTracking;
        options.IsGlobalModeEnabled = isGlobalModeEnabled;
        options.EnableTracing = enableTracing;
        options.TracesSampleRate = tracesSampleRate;
    });

    builder.Logging.AddSentry(options =>
    {
        options.Dsn = sentryDSN;
        options.AutoSessionTracking = autoSessionTracking;
        options.IsGlobalModeEnabled = isGlobalModeEnabled;
        options.EnableTracing = enableTracing;
        options.TracesSampleRate = tracesSampleRate;
    });
}

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AnagramSolverContext>(options =>
            options.UseNpgsql(builder.Configuration.GetValue<string>("CONNECTION_STRING")));

builder.Services.AddTransient<ImportCelebritiesPageJob>();
builder.Services.AddTransient<EnqueueScheduledImportCelebrityPagesJob>();
builder.Services.AddTransient<ImportCelebrityRequestsSchedulerJob>();
builder.Services.AddTransient<ProcessImportCelebrityRequestsJob>();
builder.Services.AddTransient<ScheduleCelebrityPagesImportJob>();
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
            app.Logger.LogWarning($"Business rule violation: {exceptionHandlerPathFeature.Error.Message}");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Business rule violation: {exceptionHandlerPathFeature.Error.Message}!");
        }
        else if (exceptionHandlerPathFeature?.Error is UniqueConstraintException)
        {
            app.Logger.LogWarning($"Unique constraint violation: {exceptionHandlerPathFeature.Error}");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Entity with the same unique key already exists!");
        }
        else if (exceptionHandlerPathFeature?.Error is not null)
        {
            app.Logger.LogError($"Error occured while executing controller method: {exceptionHandlerPathFeature.Error}");
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

app.UseAuthentication();
app.UseAuthorization();

GlobalConfiguration.Configuration
       .UseActivator(new HangfireActivator(app.Services));

app.UseHangfireDashboard("/background-jobs", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthFilter() }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

RecurringJob.AddOrUpdate<ImportCelebrityRequestsSchedulerJob>("ImportWikiDataCelebrityRequestsSchedulerJob", x => x.ScheduleSingleAsync(), Cron.Minutely);
RecurringJob.AddOrUpdate<ProcessImportCelebrityRequestsJob>("ProcessImportWikiDataCelebrityRequestsJob", x => x.ProcessAsync(), Cron.Minutely);

app.Run();
