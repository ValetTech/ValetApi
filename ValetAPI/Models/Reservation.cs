using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ValetAPI.Models;

public class Reservation
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int SittingId { get; set; }
    public Sitting? Sitting { get; set; }

    [DataType(DataType.Time)]
    [DisplayFormat(DataFormatString = "{hh:mm tt}", ApplyFormatInEditMode = true)]
    // [DisplayFormat(DataFormatString = "{g}", ApplyFormatInEditMode = true)]
    [Display(Name = "Reservation Time")]
    public DateTime DateTime { get; set; }
    public int Duration { get; set; } // Minutes
    [Display(Name = "No. of Guests")]
    public int NoGuests { get; set; }
    public Source? Source { get; set; } 
    //public Venue Venue { get; set; }
    public List<Table> Tables { get; set; } = new List<Table>();
    public State Status { get; set; } = State.Pending;
    public string? Notes { get; set; }


}