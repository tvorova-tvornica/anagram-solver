using AnagramSolver.Data;
using AnagramSolver.Extensions;
using EntityFramework.Exceptions.Common;
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

        if (exceptionHandlerPathFeature?.Error is InvalidFullNameException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Celebrity full name is invalid: {exceptionHandlerPathFeature.Error.Message}!");
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
    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always
    });
}
else 
{
    app.UseCookiePolicy(new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.None,
        
    });
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
