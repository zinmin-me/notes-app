using NotesApp.Core.Enums;

namespace NotesApp.Core.DTOs.Notes;

/// <summary>
/// Query parameters for fetching notes with search, filter, sort, and pagination.
/// </summary>
public class NoteQueryParameters
{
    private int _page = 1;
    private int _pageSize = 10;

    /// <summary>
    /// Current page number (minimum 1).
    /// </summary>
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    /// <summary>
    /// Number of items per page (1-50, default 10).
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 50 ? 50 : value < 1 ? 1 : value;
    }

    /// <summary>
    /// Search term to filter by title or content (case-insensitive).
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Sort field.
    /// </summary>
    public SortBy SortBy { get; set; } = SortBy.CreatedAt;

    /// <summary>
    /// Sort direction.
    /// </summary>
    public SortOrder SortOrder { get; set; } = SortOrder.Descending;

    /// <summary>
    /// Date range filter.
    /// </summary>
    public DateFilter DateFilter { get; set; } = DateFilter.All;
}
