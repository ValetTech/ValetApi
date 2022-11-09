namespace ValetAPI.Models;

public class TableEntity
{
    public int Id { get; set; }

    public string Type { get; set; } //Change to enum

    // coordinate
    public int xPosition { get; set; }
    public int yPosition { get; set; }

    public int Capacity { get; set; }
    public int? VenueId { get; set; }
    public int AreaId { get; set; }
    public AreaEntity? Area { get; set; }
    public List<ReservationTable>? ReservationTables { get; set; } = new();
}