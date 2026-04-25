import { Component, input } from '@angular/core';
import { SyncStatus } from '../../models/sync-status.model';
import { MatChipsModule } from '@angular/material/chips';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-sync-status',
  standalone: true,
  imports: [MatChipsModule, DatePipe],
  template: `
    @if (syncStatus() && syncStatus()!.lastSyncRun) {
      <div class="sync-status">
        <span>Last sync: {{ syncStatus()!.lastSyncRun!.completedUtc | date:'medium' }}</span>
        <mat-chip [class]="'status-' + syncStatus()!.lastSyncRun!.status.toLowerCase()">
          {{ syncStatus()!.lastSyncRun!.status }}
        </mat-chip>
        <span>{{ syncStatus()!.lastSyncRun!.activitiesProcessed }} activities</span>
        @if (syncStatus()!.lastSyncRun!.errorMessage) {
          <span class="error">{{ syncStatus()!.lastSyncRun!.errorMessage }}</span>
        }
      </div>
    } @else {
      <div class="sync-status">Never synced</div>
    }
  `,
  styles: [`
    .sync-status { display: flex; align-items: center; gap: 12px; font-size: 0.9rem; flex-wrap: wrap; }
    .error { color: red; }
  `]
})
export class SyncStatusComponent {
  syncStatus = input<SyncStatus | null>(null);
}
