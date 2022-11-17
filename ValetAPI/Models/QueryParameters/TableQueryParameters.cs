namespace ValetAPI.Models.QueryParameters;

public class TableQueryParameters : QueryParameters
{
    //-- Id, Type,Capacity, AreaId, SittingId, SittingType, Date, MinDate, MaxDate, IsPositioned, HasReservations, 
    public string? Id { get; set; } = null;
    public string? Type { get; set; } = null;
    public int? Capacity { get; set; } = null;
    public string? AreaId { get; set; } = null;
    public string? SittingId { get; set; } = null;
    public string? SittingType { get; set; } = null;

    public string? Date { get; set; } = null;
    public string? MinDate { get; set; } = null;
    public string? MaxDate { get; set; } = null;
    public bool? IsPositioned { get; set; } = null;
    public bool? HasReservations { get; set; } = null;
}