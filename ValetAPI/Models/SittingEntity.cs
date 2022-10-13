using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ValetAPI.Models;

public class SittingEntity
{
    public int Id { get; set; }
    public int Capacity { get; set; }
    public string Type { get; set; }
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
    public List<AreaSittingEntity> AreaSittings { get; set; } = new List<AreaSittingEntity>();
    public List<ReservationEntity> Reservations { get; set; } = new List<ReservationEntity>();

}