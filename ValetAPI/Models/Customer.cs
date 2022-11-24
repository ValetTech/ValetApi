using System.ComponentModel.DataAnnotations;

namespace ValetAPI.Models;

public class Customer
{
    public int Id { get; set; }

    [Display(Name = "First Name")] public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Last Name")] public string LastName { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public List<Reservation> Reservations { get; set; } = new();
    public bool IsVip { get; set; } = false;
    public string? FullName => FirstName + " " + LastName;
}