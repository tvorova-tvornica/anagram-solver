using System.Security.Claims;
using AnagramSolver.Controllers.Auth.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.Controllers.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync([FromBody] SignInDto logInDto)
    {
        var adminUsername = _configuration["ADMIN_USERNAME"];
        var adminPassword = _configuration["ADMIN_PASSWORD"];

        var isValidLogIn = adminUsername == logInDto.username && adminPassword == logInDto.password;

        if (!isValidLogIn)
        {
            return Unauthorized();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "Admin"),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            IsPersistent = false,
            IssuedUtc = DateTimeOffset.UtcNow,
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );

        return Ok();
    }

    [HttpPost("sign-out")]
    public async Task<IActionResult> SignOutAsync()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
        
        return Ok();
    }

    [HttpGet("is-authenticated")]
    public bool IsAuthenticated()
    {
        return HttpContext.User?.Identity?.IsAuthenticated ?? false;
    }
}
