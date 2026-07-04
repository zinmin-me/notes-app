namespace NotesApp.Core.DTOs.Common;

/// <summary>
/// Paginated response wrapper with navigation metadata.
/// </summary>
/// <typeparam name="T">Type of the items in the collection.</typeparam>
public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;

    public PaginatedResponse() { }

    public PaginatedResponse(List<T> items, int totalRecords, int currentPage, int pageSize)
    {
        Items = items;
        TotalRecords = totalRecords;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
    }
}
