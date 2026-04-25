import { Component, OnInit, signal, inject } from '@angular/core';
import { PrismApiService } from '../../services/prism-api.service';
import { StatCard } from '../../models/stat-card.model';
import { SyncStatus } from '../../models/sync-status.model';
import { StatCardComponent } from '../../components/stat-card/stat-card.component';
import { SyncStatusComponent } from '../../components/sync-status/sync-status.component';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    StatCardComponent,
    SyncStatusComponent,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatToolbarModule,
    MatIconModule,
    MatSnackBarModule
  ],
  template: `
    <mat-toolbar color="primary">
      <span>PRISM</span>
      <span class="spacer"></span>
      <app-sync-status [syncStatus]="syncStatus()" />
    </mat-toolbar>

    <div class="dashboard-container">
      <div class="actions-row">
        <button mat-raised-button color="accent" (click)="syncActivities()" [disabled]="syncing()">
          @if (syncing()) {
            <mat-spinner diameter="20"></mat-spinner>
          } @else {
            Sync Strava Activities
          }
        </button>
      </div>

      @if (loading()) {
        <div class="loading-container">
          <mat-spinner></mat-spinner>
        </div>
      } @else if (error()) {
        <div class="error-container">
          <p>{{ error() }}</p>
          <button mat-button (click)="loadStats()">Retry</button>
        </div>
      } @else {
        <div class="stats-grid">
          @for (card of stats(); track card.key) {
            <app-stat-card [card]="card" />
          }
        </div>
      }
    </div>
  `,
  styles: [`
    .spacer { flex: 1; }
    .dashboard-container { padding: 24px; max-width: 1400px; margin: 0 auto; }
    .actions-row { margin-bottom: 24px; display: flex; align-items: center; gap: 16px; }
    .loading-container, .error-container { display: flex; flex-direction: column; align-items: center; padding: 48px; }
    .stats-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 16px; }
    mat-spinner { display: inline-block; }
  `]
})
export class DashboardComponent implements OnInit {
  private api = inject(PrismApiService);
  private snackBar = inject(MatSnackBar);

  stats = signal<StatCard[]>([]);
  syncStatus = signal<SyncStatus | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);
  syncing = signal(false);

  ngOnInit() {
    this.loadStats();
    this.loadSyncStatus();
  }

  loadStats() {
    this.loading.set(true);
    this.error.set(null);
    this.api.getAllTimeStats().subscribe({
      next: (data) => {
        this.stats.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load stats. Make sure your backend is running and you have synced activities.');
        this.loading.set(false);
      }
    });
  }

  loadSyncStatus() {
    this.api.getSyncStatus().subscribe({
      next: (data) => this.syncStatus.set(data),
      error: () => {}
    });
  }

  syncActivities() {
    this.syncing.set(true);
    this.api.triggerSync().subscribe({
      next: (data) => {
        this.syncStatus.set(data);
        this.syncing.set(false);
        this.snackBar.open('Sync completed!', 'Close', { duration: 3000 });
        this.loadStats();
      },
      error: (err) => {
        this.syncing.set(false);
        this.snackBar.open('Sync failed. Check console for details.', 'Close', { duration: 5000 });
      }
    });
  }
}
