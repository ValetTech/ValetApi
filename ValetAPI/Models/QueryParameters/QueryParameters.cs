namespace ValetAPI.Models.QueryParameters;

/// <summary>
/// </summary>
public class QueryParameters
{
    private const int _maxSize = 100;
    private int _size = 50;

    private string _sortOrder = "asc";

    public int Page { get; set; } = 1;

    public int Size
    {
        get => _size;
        set => _size = Math.Min(_maxSize, value);
    }

    public string SortBy { get; set; } = "Id";

    public string SortOrder
    {
        get => _sortOrder;

        set
        {
            if (value?.ToLower() is "asc" or "desc") _sortOrder = value;
        }
    }

    public string SearchTerm { get; set; } = string.Empty;
}