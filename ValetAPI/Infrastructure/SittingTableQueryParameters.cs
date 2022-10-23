namespace ValetAPI.Infrastructure;

/// <inheritdoc />
public class SittingTableQueryParameters : QueryParameters
{
    public bool Available { get; set; } = false;
    public bool Taken { get; set; } = false;
}