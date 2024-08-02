namespace project_frej.Models;

/// <summary>
/// Represents a paged result set, containing a list of items along with metadata about the pagination.
/// </summary>
/// <typeparam name="T">The type of the items in the data list.</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// The total number of records available.
    /// </summary>
    public int TotalRecords { get; set; }

    /// <summary>
    /// The total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// The current page number.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// The number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The list of items for the current page.
    /// </summary>
    public List<T> Data { get; set; } = [];
}
