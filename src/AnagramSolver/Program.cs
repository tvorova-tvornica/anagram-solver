using AnagramSolver.BackgroundJobs;
using AnagramSolver.BackgroundJobs.WikiDataImport;
using AnagramSolver.Data;
using AnagramSolver.Exceptions;
using AnagramSolver.Extensions;
using AnagramSolver.HttpClients;
using EntityFramework.Exceptions.Common;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseShutdownTimeout(TimeSpan.FromSeconds(55));

builder.AddAnagramSolverSentry();

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

builder.AddAnagramSolverAuthentication();

builder.Services.AddHangfire(config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                             .UseSimpleAssemblyNameTypeSerializer()
                                             .UseRecommendedSerializerSettings()
                                             .UsePostgreSqlStorage(builder.Configuration.GetValue<string>("CONNECTION_STRING")));

builder.Services.AddHangfireServer(config => config.ShutdownTimeout = TimeSpan.FromSeconds(50));

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
            return;
        }

        if (exceptionHandlerPathFeature?.Error is UniqueConstraintException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Entity with the same unique key already exists!");
            return;
        }

        if (exceptionHandlerPathFeature?.Error is not null)
        {
            throw exceptionHandlerPathFeature.Error;
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
