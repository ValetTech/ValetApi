using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ValetAPI.Models.QueryParameters;

/// <inheritdoc />
public class SittingQueryParameters : QueryParameters
{
    // FILTER
    public DateTime? Date { get; set; }
    public DateTime? MinDateTime { get; set; }
    public DateTime? MaxDateTime { get; set; }

    public int? Capacity { get; set; } = null;

    [EnumDataType(typeof(SittingType))]
    [JsonConverter(typeof(StringEnumConverter))]
    public SittingType? Type { get; set; }

    public bool? hasAreas { get; set; } = null;
    public bool? hasReservations { get; set; } = null;
}