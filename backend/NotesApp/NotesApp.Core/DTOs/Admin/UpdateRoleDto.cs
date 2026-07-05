using System.ComponentModel.DataAnnotations;

namespace NotesApp.Core.DTOs.Admin;

/// <summary>
/// Data transfer object for updating a user's role.
/// </summary>
public class UpdateRoleDto
{
    [Required]
    public string Role { get; set; } = string.Empty;
}
