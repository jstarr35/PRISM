namespace PRISM.Api.Dtos;

public class StatCardDto
{
    public string Key { get; set; } = "";
    public string Label { get; set; } = "";
    public double Value { get; set; }
    public string Unit { get; set; } = "";
    public string DisplayValue { get; set; } = "";
    public long? ActivityId { get; set; }
    public string? ActivityName { get; set; }
    public string? SportType { get; set; }
    public DateTime? StartDateLocal { get; set; }
    public string? StravaActivityUrl { get; set; }
}
