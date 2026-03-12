using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace QcOnLocation.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : Controller
{
    [HttpGet("login")]
    public IActionResult Login([FromQuery] string? returnUrl = "/locations")
    {
        var props = new AuthenticationProperties { RedirectUri = returnUrl };
        return Challenge(props, "Google");
    }
    //pushs
}




