using Microsoft.AspNetCore.Authentication.Cookies;

namespace AnagramSolver.Extensions;

public static class WebApplicationBuilderAuthExtensions
{
    public static void AddAnagramSolverAuthentication(this WebApplicationBuilder builder)
    {
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
    }
}
