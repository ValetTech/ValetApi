using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ValetAPI.Models;

public class Sitting
{
    public int? Id { get; set; }
    public int Capacity { get; set; }
    public Guid? GroupId { get; set; } = null;

    public string? Title { get; set; }
    public SittingType Type { get; set; }

    [DisplayFormat(DataFormatString = "{g}", ApplyFormatInEditMode = true)]
    // [DisplayFormat(DataFormatString = "{hh:mm tt}", ApplyFormatInEditMode = true)]
    [Display(Name = "Start Time")]
    [Required]
    public DateTime StartTime { get; set; }

    [DisplayFormat(DataFormatString = "{g}", ApplyFormatInEditMode = true)]
    [Display(Name = "End Time")]
    [Required]
    public DateTime EndTime { get; set; }

    // public int AreaId { get; set; }
    public int VenueId { get; set; }

    // public List<AreaEntity> Areas { get; set; }
    // [JsonIgnore]
    public List<string> AreaIds { get; set; } = new();
    public List<Area> Areas { get; set; } = new();
    public List<Reservation> Reservations { get; set; } = new();
}