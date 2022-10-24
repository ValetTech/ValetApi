using System.ComponentModel.DataAnnotations;

namespace ValetAPI.Models;

public class Reservation
{
    public int Id { get; set; }
    [Required]
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    [Required]
    public int SittingId { get; set; }
    public Sitting? Sitting { get; set; }

    [DataType(DataType.Time)]
    [DisplayFormat(DataFormatString = "{hh:mm tt}", ApplyFormatInEditMode = true)]
    // [DisplayFormat(DataFormatString = "{g}", ApplyFormatInEditMode = true)]
    [Display(Name = "Reservation Time")]
    [Required]
    public DateTime DateTime { get; set; }

    public int Duration { get; set; } = 90;

    [Display(Name = "No. of Guests")] 
    [Required]
    public int NoGuests { get; set; }

    public Source? Source { get; set; }

    //public Venue Venue { get; set; }
    public List<Table> Tables { get; set; } = new();
    public State Status { get; set; } = State.Pending;
    public string? Notes { get; set; }
}