using System.ComponentModel.DataAnnotations;

namespace ValetAPI.Models;

public class SittingEntity
{
    public int? Id { get; set; }
    public Guid? GroupId { get; set; } = null;
    public int Capacity { get; set; }
    public string? Title { get; set; }
    public SittingType Type { get; set; }

    [DisplayFormat(DataFormatString = "{g}", ApplyFormatInEditMode = true)]
    [Display(Name = "Start Time")]
    public DateTime StartTime { get; set; }

    [DisplayFormat(DataFormatString = "{g}", ApplyFormatInEditMode = true)]
    [Display(Name = "End Time")]
    public DateTime EndTime { get; set; }

    // public int AreaId { get; set; }
    public int VenueId { get; set; }
    public VenueEntity Venue { get; set; }

    // public List<AreaEntity> Areas { get; set; }
    public List<AreaSittingEntity> AreaSittings { get; set; } = new();
    public List<ReservationEntity> Reservations { get; set; } = new();
}