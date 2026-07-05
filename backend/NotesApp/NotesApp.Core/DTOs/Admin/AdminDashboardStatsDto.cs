namespace NotesApp.Core.DTOs.Admin;

/// <summary>
/// Data transfer object for Admin Dashboard statistics.
/// </summary>
public class AdminDashboardStatsDto
{
    public int TotalUsers { get; set; }
    public int TotalNotes { get; set; }
    public int UsersJoinedToday { get; set; }
    public int NotesCreatedToday { get; set; }
}
