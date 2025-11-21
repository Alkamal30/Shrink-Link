using Microsoft.AspNetCore.Mvc;

namespace ShrinkLink.UserService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }
}