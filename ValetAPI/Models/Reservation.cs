using System.ComponentModel.DataAnnotations;

namespace ValetAPI.Models;

public class Reservation
{
    public int Id { get; set; }

    [Required] public int CustomerId { get; set; }

    public Customer? Customer { get; set; }

    [Required] public int SittingId { get; set; }

    public Sitting? Sitting { get; set; }
    public int AreaId { get; set; }
    public Area? Area { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "Reservation Time")]
    [Required]
    public DateTime DateTime { get; set; }

    public int Duration { get; set; } = 90;

    [Display(Name = "No. of Guests")]
    [Required]
    public int NoGuests { get; set; }

    public Source? Source { get; set; }

    public int VenueId { get; set; } = 1;

    //public Venue Venue { get; set; }
    public List<Table> Tables { get; set; } = new();
    public State Status { get; set; } = State.Pending;
    public string? Notes { get; set; }
}