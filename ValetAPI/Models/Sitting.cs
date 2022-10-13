using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace ValetAPI.Models;

public class Sitting
{
    public int Id { get; set; }
    public int Capacity { get; set; }
    public string Type { get; set; }
    [DisplayFormat(DataFormatString = "{g}", ApplyFormatInEditMode = true)]
    // [DisplayFormat(DataFormatString = "{hh:mm tt}", ApplyFormatInEditMode = true)]
    [Display(Name = "Start Time")]
    public DateTime StartTime { get; set; }
    [DisplayFormat(DataFormatString = "{g}", ApplyFormatInEditMode = true)]
    [Display(Name = "End Time")]
    public DateTime EndTime { get; set; }
    // public int AreaId { get; set; }
    public int VenueId { get; set; }
    // public List<AreaEntity> Areas { get; set; }
    // [JsonIgnore]
    public List<Area> Areas { get; set; } = new List<Area>();
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();

}