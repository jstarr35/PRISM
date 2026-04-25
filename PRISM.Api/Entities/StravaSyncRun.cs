namespace PRISM.Api.Entities;

public class StravaSyncRun
{
    public int Id { get; set; }
    public DateTime StartedUtc { get; set; }
    public DateTime? CompletedUtc { get; set; }
    public string Status { get; set; } = "Running";
    public int ActivitiesProcessed { get; set; }
    public string? ErrorMessage { get; set; }
}
