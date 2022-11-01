namespace ValetAPI.Models.QueryParameters;

public class CustomerQueryParameters : QueryParameters
{
    // FILTER
    public bool? isVip { get; set; } = null;
    public bool? hasReservations { get; set; } = null;

    // SEARCH
    public string Name { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}