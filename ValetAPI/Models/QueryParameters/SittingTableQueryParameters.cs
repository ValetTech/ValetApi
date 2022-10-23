namespace ValetAPI.Models.QueryParameters;

/// <inheritdoc />
public class SittingTableQueryParameters : QueryParameters
{
    /// <summary>
    /// </summary>
    public bool Available { get; set; } = false;

    /// <summary>
    /// </summary>
    public bool Taken { get; set; } = false;
}