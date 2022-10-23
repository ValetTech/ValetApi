using Microsoft.AspNetCore.Mvc;

namespace ValetAPI.Controllers.API;

/// <summary>
///     Root Controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class RootController : ControllerBase
{
}