namespace PRISM.Api.Dtos;

public class SyncRunDto
{
    public DateTime StartedUtc { get; set; }
    public DateTime? CompletedUtc { get; set; }
    public string Status { get; set; } = "";
    public int ActivitiesProcessed { get; set; }
    public string? ErrorMessage { get; set; }
}

public class SyncStatusDto
{
    public SyncRunDto? LastSyncRun { get; set; }
}
