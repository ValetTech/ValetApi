using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ValetAPI.Models.QueryParameters;

public class ReservationQueryParameters : QueryParameters
{
    // FILTER
    public string? MinDate { get; set; } = null;
    public string? MaxDate { get; set; } = null;
    public string? Date { get; set; } = null;

    public int? Duration { get; set; } = null;
    public int? Guests { get; set; } = null;
    
    public bool? hasTables { get; set; } = null;
    
    public string? Id { get; set; } = null;
    public string? CustomerId { get; set; } = null;
    public string? SittingId { get; set; } = null;
    public string? AreaId { get; set; } = null;

    

    [EnumDataType(typeof(Source))]
    [JsonConverter(typeof(StringEnumConverter))]
    public Source? Source { get; set; } = null;

    [EnumDataType(typeof(State))]
    [JsonConverter(typeof(StringEnumConverter))]
    public State? Status { get; set; } = null;

    public string? Areas { get; set; } = null;

    public string? Sittings { get; set; } = null;

    // SEARCH
    public string? Customer { get; set; } = null;
}