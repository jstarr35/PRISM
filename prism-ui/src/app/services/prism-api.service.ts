import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StatCard } from '../models/stat-card.model';
import { SyncStatus } from '../models/sync-status.model';

@Injectable({ providedIn: 'root' })
export class PrismApiService {
  private http = inject(HttpClient);

  getAllTimeStats(): Observable<StatCard[]> {
    return this.http.get<StatCard[]>('/api/stats/all-time');
  }

  triggerSync(): Observable<SyncStatus> {
    return this.http.post<SyncStatus>('/api/sync', {});
  }

  getSyncStatus(): Observable<SyncStatus> {
    return this.http.get<SyncStatus>('/api/sync/status');
  }
}
