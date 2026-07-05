using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Core.DTOs.Auth;
using NotesApp.Core.Interfaces.Services;

namespace NotesApp.Api.Controllers;

/// <summary>
/// Controller for administrative operations.
/// </summary>
[Authorize(Roles = "Admin")]
public class AdminController : BaseApiController
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    /// <summary>
    /// Gets a list of all registered users.
    /// </summary>
    /// <returns>A list of user profiles.</returns>
    [HttpGet("users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<UserProfileResponse>>> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await _adminService.GetAllUsersAsync(cancellationToken);
        return Ok(users);
    }
}
