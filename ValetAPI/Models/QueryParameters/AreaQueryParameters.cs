namespace ValetAPI.Models.QueryParameters;

public class AreaQueryParameters : QueryParameters
{
    // FILTER
    // public DateTime? MinDate { get; set; }
    // public DateTime? MaxDate { get; set; }
    public DateTime? Date { get; set; }

    // TODO: Add searching
    // 

    // SEARCH
    public string Name { get; set; } = string.Empty;

    // SORT
}