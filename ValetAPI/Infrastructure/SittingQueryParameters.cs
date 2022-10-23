using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ValetAPI.Models;

namespace ValetAPI.Infrastructure;

/// <inheritdoc />
public class SittingQueryParameters : QueryParameters
{
    public DateTime? Date { get; set; }
    public DateTime? DateTime { get; set; }

    [EnumDataType(typeof(SittingType))]
    [JsonConverter(typeof(StringEnumConverter))]
    public SittingType? Type { get; set; }
}