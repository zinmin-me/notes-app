using Microsoft.AspNetCore.Mvc;

namespace NotesApp.Api.Controllers;

/// <summary>
/// Base API controller to provide common functionality.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
}
