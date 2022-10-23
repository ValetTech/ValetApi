namespace ValetAPI.Models;

public class Table
{
    public int Id { get; set; }

    public string Type { get; set; } //Change to enum

    // coordinate
    public int Capacity { get; set; }

    public int AreaId { get; set; }
    // public Area Area { get; set; }
}