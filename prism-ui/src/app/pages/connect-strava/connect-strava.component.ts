import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-connect-strava',
  standalone: true,
  imports: [MatButtonModule, MatCardModule],
  template: `
    <div class="connect-container">
      <mat-card class="connect-card">
        <mat-card-header>
          <mat-card-title>Connect to Strava</mat-card-title>
          <mat-card-subtitle>Link your Strava account to view your performance stats</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          <p>PRISM analyzes your Strava activities to surface your all-time personal bests and performance insights.</p>
        </mat-card-content>
        <mat-card-actions>
          <a mat-raised-button color="warn" href="/api/auth/strava/connect">
            Connect with Strava
          </a>
        </mat-card-actions>
      </mat-card>
    </div>
  `,
  styles: [`
    .connect-container { display: flex; justify-content: center; align-items: center; height: 80vh; }
    .connect-card { max-width: 480px; width: 100%; padding: 24px; }
  `]
})
export class ConnectStravaComponent {}
