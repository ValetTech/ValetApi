using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ValetAPI.Models.QueryParameters;

public class ReservationQueryParameters : QueryParameters
{
    // FILTER
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
    public DateTime? Date { get; set; }

    public int? Duration { get; set; }
    public int? Guests { get; set; }
    [EnumDataType(typeof(SittingType))]
    [JsonConverter(typeof(StringEnumConverter))]
    public Source? Source { get; set; }
    [EnumDataType(typeof(SittingType))]
    [JsonConverter(typeof(StringEnumConverter))]
    public State? Status { get; set; }
    
    // SEARCH
    public string Customer { get; set; } = string.Empty;

}