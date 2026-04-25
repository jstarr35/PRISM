export interface SyncRun {
  id: number;
  startedUtc: string;
  completedUtc: string | null;
  status: string;
  activitiesProcessed: number;
  errorMessage: string | null;
}

export interface SyncStatus {
  lastSyncRun: SyncRun | null;
}
