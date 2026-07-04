namespace NotesApp.Core.DTOs.Common;

/// <summary>
/// Standardized API error response with validation errors.
/// </summary>
public class ApiErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();

    public static ApiErrorResponse Create(string message, List<string>? errors = null)
    {
        return new ApiErrorResponse
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}
