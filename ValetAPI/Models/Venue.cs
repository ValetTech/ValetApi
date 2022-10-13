using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ValetAPI.Models;

public class Venue
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    // public int CompanyId { get; set; }
    // public string Company { get; set; }
    public List<Area> Areas { get; set; } = new();
    public List<Sitting> Sittings { get; set; } = new();
    public List<Reservation> Reservations { get; set; } = new();
}