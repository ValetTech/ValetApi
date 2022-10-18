using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ValetAPI.Models;

public class Area
{
    public int Id { get; set; }
    [Required]
    [Display(Name = "Area")]
    [StringLength(50)]
    public string Name { get; set; }
    public string? Description { get; set; }

    // Tables
    [JsonIgnore]
    public int? NoTables { get; set; } 
    [JsonIgnore]
    public int? TableCapacity { get; set; }


    // size
    public int VenueId { get; set; } = 1;

    // public Venue? Venue { get; set; }
    public List<Table> Tables { get; set; } = new();
    public List<Sitting> Sittings { get; set; } = new();
}