using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace QcOnLocation.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : Controller
{
    [HttpGet("login")]
    public async Task<IActionResult> Login([FromQuery] string? returnUrl = "/locations")
    {
        var schemeProvider =
            HttpContext.RequestServices.GetService(
                    typeof(Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider)) as
                Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider;
        if (schemeProvider == null)
        {
            return Problem("Authentication scheme provider not available", statusCode: 500);
        }

        var scheme =
            await schemeProvider.GetSchemeAsync(Microsoft.AspNetCore.Authentication.Google.GoogleDefaults
                .AuthenticationScheme);
        if (scheme == null)
        {
            return Problem(
                "Google authentication provider not configured. Set Authentication:Google:ClientId and Authentication:Google:ClientSecret.",
                statusCode: 501);
        }

        var props = new AuthenticationProperties { RedirectUri = returnUrl };
        try
        {
            return Challenge(props, "Google");
        }
        catch (InvalidOperationException)
        {
            return Problem(
                "Google authentication provider not configured. Set Authentication:Google:ClientId and Authentication:Google:ClientSecret.",
                statusCode: 501);
        }
    }

    [HttpGet("logout")]
    [AllowAnonymous]
    public IActionResult Logout([FromQuery] string? returnUrl = "/")
    {
        var props = new AuthenticationProperties { RedirectUri = returnUrl };
        return SignOut(props, CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> LogoutPost()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    [HttpGet("status")]
    [AllowAnonymous]
    public IActionResult Status()
    {
        var isAuthenticated = User?.Identity?.IsAuthenticated ?? false;
        var name = isAuthenticated ? User?.Identity?.Name : null;
        return Ok(new { authenticated = isAuthenticated, name });
    }
}