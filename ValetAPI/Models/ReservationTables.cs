namespace ValetAPI.Models;

public class ReservationTables
{
    public int ReservationId { get; set; }
    public ReservationEntity? Reservation { get; set; }
    public int TableId { get; set; }
    public TableEntity? Table { get; set; }
}