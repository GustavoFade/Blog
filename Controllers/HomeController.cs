using Microsoft.AspNetCore.Mvc;

namespace Blogs.Controllers;

[ApiController]
[Route("/hc")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}