using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers;

[ApiController]
public class AuthController : Controller
{
    [HttpGet("~/login")]
    public async Task<ActionResult> Login() => Challenge(new AuthenticationProperties { RedirectUri = "/" }, "Discord");
}