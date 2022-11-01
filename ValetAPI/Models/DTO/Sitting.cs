using Newtonsoft.Json;

namespace ValetAPI.Models.DTO;

public class Sitting
{
    public int Id { get; set; }
    public int Capacity { get; set; }
    public SittingType Type { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int VenueId { get; set; }

    [JsonIgnore] public List<Area> Areas { get; set; } = new();

    [JsonIgnore] public List<Reservation> Reservations { get; set; } = new();
}