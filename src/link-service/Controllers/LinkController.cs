using System;
using Microsoft.AspNetCore.Mvc;

namespace ShrinkLink.LinkService.Controllers;

[ApiController]
public class LinkController : ControllerBase
{
	[HttpGet]
	[Route("[controller]/")]
	public IActionResult GetAll()
	{		
		return Ok("All Links");
	}
}
