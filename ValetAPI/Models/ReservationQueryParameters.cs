namespace ValetAPI.Models;

public class ReservationQueryParameters : QueryParameters
{
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
    public DateTime? Date { get; set; }
    

}