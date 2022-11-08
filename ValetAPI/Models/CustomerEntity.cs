using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValetAPI.Models;

public class CustomerEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public List<ReservationEntity> Reservations { get; set; } = new();
    public bool IsVip { get; set; } = false;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Display(Name = "Name")]
    public string FullName => FirstName + " " + LastName;
}