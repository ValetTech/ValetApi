using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ValetAPI.Models.QueryParameters;

/// <inheritdoc />
public class SittingQueryParameters : QueryParameters
{
    // FILTER
    public string? Date { get; set; } = null;
    public string? MinDateTime { get; set; } = null;
    public string? MaxDateTime { get; set; } = null;

    public int? Capacity { get; set; } = null;
    public string? Title { get; set; } = null;

    [EnumDataType(typeof(SittingType))]
    [JsonConverter(typeof(StringEnumConverter))]
    public SittingType? Type { get; set; }

    public bool? hasAreas { get; set; } = null;
    public bool? hasReservations { get; set; } = null;

    public string[] Areas { get; set; } = Array.Empty<string>();
    public string[] Types { get; set; } = Array.Empty<string>();
    public int? Id { get; set; } = null;
    


}