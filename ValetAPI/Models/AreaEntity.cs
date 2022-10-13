using System.ComponentModel.DataAnnotations;

namespace ValetAPI.Models;

public class AreaEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    // size
    public int VenueId { get; set; }
    public VenueEntity Venue { get; set; }
    public List<TableEntity> Tables { get; set; } = new();
    public List<AreaSittingEntity> AreaSittings { get; set; } = new();
}