using Newtonsoft.Json;

namespace ValetAPI.Models.DTO;

public class Reservation
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int SittingId { get; set; }
    public Sitting? Sitting { get; set; }

    public DateTime DateTime { get; set; }

    public int Duration { get; set; } // Minutes
    public int NoGuests { get; set; }

    public Source? Source { get; set; }

    [JsonIgnore] public List<Table> Tables { get; set; } = new();

    public State Status { get; set; } = State.Pending;
    public string? Notes { get; set; }
}