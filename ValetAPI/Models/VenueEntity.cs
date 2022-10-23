namespace ValetAPI.Models;

public class VenueEntity
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Address { get; set; }

    // public int CompanyId { get; set; }
    // public string Company { get; set; }
    public List<AreaEntity> Areas { get; set; } = new();
    public List<SittingEntity> Sittings { get; set; } = new();
    public List<ReservationEntity> Reservations { get; set; } = new();
}