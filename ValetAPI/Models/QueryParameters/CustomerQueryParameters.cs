namespace ValetAPI.Models.QueryParameters;

public class CustomerQueryParameters : QueryParameters
{
    // FILTER
    public int? Id { get; set; } = null;
    public bool? isVip { get; set; } = null;
    public bool? hasReservations { get; set; } = null;

    // SEARCH
    public string? Name { get; set; } = null;
    public string? FirstName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public string? Email { get; set; } = null;
    public string? Phone { get; set; } = null;
}