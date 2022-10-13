using System.ComponentModel.DataAnnotations;

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
    public int NoTables { get; set; } = 0;
    public int TableCapacity { get; set; } = 0;


    // size
    public int VenueId { get; set; } = 1;

    // public Venue? Venue { get; set; }
    public List<Table> Tables { get; set; } = new();
    public List<Sitting> Sittings { get; set; } = new();
}