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
        // If Google auth is not configured the Challenge will fail. Check for the scheme
        // and return a helpful response instead of throwing an exception.
        var schemeProvider = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider)) as Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider;
        if (schemeProvider == null)
        {
            return Problem("Authentication scheme provider not available", statusCode: 500);
        }

        var scheme = await schemeProvider.GetSchemeAsync(Microsoft.AspNetCore.Authentication.Google.GoogleDefaults.AuthenticationScheme);
        if (scheme == null)
        {
            return Problem("Google authentication provider not configured. Set Authentication:Google:ClientId and Authentication:Google:ClientSecret.", statusCode: 501);
        }

        var props = new AuthenticationProperties { RedirectUri = returnUrl };
        try
        {
            return Challenge(props, "Google");
        }
        catch (InvalidOperationException)
        {
            // Defensive: if the authentication handler isn't registered, return a helpful response
            return Problem("Google authentication provider not configured. Set Authentication:Google:ClientId and Authentication:Google:ClientSecret.", statusCode: 501);
        }
    }

    // Browser-friendly logout: signs out local cookie and redirects to returnUrl
    [HttpGet("logout")]
    [AllowAnonymous]
    public IActionResult Logout([FromQuery] string? returnUrl = "/")
    {
        // Sign out the cookie server-side and redirect the browser to returnUrl.
        // In development the cookie SecurePolicy may be None so signing out over HTTP
        // will clear the cookie; in production cookies are Secure and sign-out should
        // occur over HTTPS.
        var props = new AuthenticationProperties { RedirectUri = returnUrl };
        return SignOut(props, CookieAuthenticationDefaults.AuthenticationScheme);
    }

    // API-friendly logout: sign out the cookie server-side and return 204 No Content
    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> LogoutPost()
    {
        // Sign out the cookie server-side and return 204 No Content for API clients.
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }
}




