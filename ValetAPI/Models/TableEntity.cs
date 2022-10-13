namespace ValetAPI.Models;

public class TableEntity
{
    public int Id { get; set; }
    public string Type { get; set; } //Change to enum
    // coordinate
    public int Capacity { get; set; }
    public int VenueId { get; set; }
    public int AreaId { get; set; }
    public AreaEntity? Area { get; set; }
    public int? ReservationId { get; set; }
    public ReservationEntity? Reservation { get; set; }
}