using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Core.DTOs.Auth;
using NotesApp.Core.DTOs.Admin;
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

    /// <summary>
    /// Gets overall system statistics for the admin dashboard.
    /// </summary>
    /// <returns>Dashboard statistics.</returns>
    [HttpGet("stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AdminDashboardStatsDto>> GetStats(CancellationToken cancellationToken)
    {
        var stats = await _adminService.GetDashboardStatsAsync(cancellationToken);
        return Ok(stats);
    }

    /// <summary>
    /// Updates a user's role.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="dto">The role update details.</param>
    /// <returns>No content if successful.</returns>
    [HttpPut("users/{id:guid}/role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserRole(Guid id, [FromBody] UpdateRoleDto dto, CancellationToken cancellationToken)
    {
        // Optional: Prevent admins from changing their own role to avoid getting locked out.
        var currentUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (currentUserIdClaim != null && Guid.TryParse(currentUserIdClaim.Value, out var currentUserId))
        {
            if (id == currentUserId)
            {
                return BadRequest(new { Message = "You cannot change your own role." });
            }
        }

        var result = await _adminService.UpdateUserRoleAsync(id, dto.Role, cancellationToken);
        
        if (!result)
        {
            return BadRequest(new { Message = "Failed to update role. Ensure the role is valid and the user exists." });
        }

        return NoContent();
    }
}
