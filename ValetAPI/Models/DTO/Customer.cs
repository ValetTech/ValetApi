using Newtonsoft.Json;

namespace ValetAPI.Models.DTO;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    [JsonIgnore] public List<Reservation> Reservations { get; set; } = new();

    public bool IsVip { get; set; } = false;
    public string? FullName => FirstName + " " + LastName;
}