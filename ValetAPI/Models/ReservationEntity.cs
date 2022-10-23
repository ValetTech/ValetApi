using System.ComponentModel.DataAnnotations;

namespace ValetAPI.Models;

public class ReservationEntity
{
    public int Id { get; set; }
    public CustomerEntity Customer { get; set; }
    public SittingEntity Sitting { get; set; }

    [DisplayFormat(DataFormatString = "{g}", ApplyFormatInEditMode = true)]
    [Display(Name = "Reservation Time")]
    public DateTime DateTime { get; set; }

    public int Duration { get; set; } // Minutes

    [Display(Name = "No. of Guests")] public int NoGuests { get; set; }

    public Source Source { get; set; }
    public int VenueId { get; set; }
    public VenueEntity Venue { get; set; }

    //public Venue Venue { get; set; }
    public List<TableEntity> Tables { get; set; } = new();
    public State Status { get; set; } = State.Pending;
    public string? Notes { get; set; }
}