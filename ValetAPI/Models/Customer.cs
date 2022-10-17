using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace ValetAPI.Models;

public class Customer
{
    public int Id { get; set; }
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    [Display(Name = "Full Name")]
    public string? FullName => LastName + ", " + FirstName;
}