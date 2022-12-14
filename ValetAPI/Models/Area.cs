using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ValetAPI.Models;

public class Area
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Area")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Please provide a valid Area name.")]
    public string Name { get; set; }

    public string Description { get; set; } = string.Empty;

    // Tables
    [JsonIgnore] public int? NoTables { get; set; }

    [JsonIgnore] public int? TableCapacity { get; set; }

    

    // size
    public int Width { get; set; }
    public int Height { get; set; }
    
    public int VenueId { get; set; } = 1;

    // public Venue? Venue { get; set; }
    public List<Table> Tables { get; set; } = new();
    public List<Sitting> Sittings { get; set; } = new();
}