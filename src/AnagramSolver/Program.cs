using AnagramSolver.Data;
using AnagramSolver.Exceptions;
using AnagramSolver.Extensions;
using EntityFramework.Exceptions.Common;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AnagramSolverContext>(options =>
            options.UseNpgsql(builder.Configuration.GetValue<string>("CONNECTION_STRING")));

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

builder.Services.AddHangfire(config =>
		        config.UsePostgreSqlStorage(builder.Configuration.GetValue<string>("CONNECTION_STRING")));

builder.Services.AddHangfireServer();

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
            await context.Response.WriteAsync($"Celebrity with same name already exists!");
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

app.UseHangfireDashboard();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
