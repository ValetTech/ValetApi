using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ValetAPI.Models.DTO;

public class Area
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    // size
    [JsonIgnore]
    public int VenueId { get; set; } = 1;

    [JsonIgnore]
    public List<Table> Tables { get; set; } = new();
    [JsonIgnore]
    public List<Sitting> Sittings { get; set; } = new();
}