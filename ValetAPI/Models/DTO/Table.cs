namespace ValetAPI.Models.DTO;

public class Table
{
    public int Id { get; set; }

    public string Type { get; set; } //Change to enum

    // coordinate
    public int xPosition { get; set; }
    public int yPosition { get; set; }
    
    public int Capacity { get; set; }

    public int AreaId { get; set; }
    public Area Area { get; set; }

    // public int[] ReservationIds { get; set; }
    public List<Reservation> Reservations { get; set; } = new();

}