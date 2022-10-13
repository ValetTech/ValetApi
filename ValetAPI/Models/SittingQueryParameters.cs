namespace ValetAPI.Models;

public class SittingQueryParameters : QueryParameters
{
    public bool Available { get; set; } = false;
    public bool Taken { get; set; } = false;
}