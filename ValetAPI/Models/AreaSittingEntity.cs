namespace ValetAPI.Models;

public class AreaSittingEntity
{
    public int AreaId { get; set; }
    public AreaEntity? Area { get; set; }
    public int SittingId { get; set; }
    public SittingEntity? Sitting { get; set; }
}