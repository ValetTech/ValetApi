namespace ValetAPI.Models;

public class AreaSitting
{
    public int AreaId { get; set; }
    public Area? Area { get; set; }
    public int SittingId { get; set; }
    public Sitting? Sitting { get; set; }
}