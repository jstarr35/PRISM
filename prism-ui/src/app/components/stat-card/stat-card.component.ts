import { Component, input } from '@angular/core';
import { StatCard } from '../../models/stat-card.model';
import { MatCardModule } from '@angular/material/card';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-stat-card',
  standalone: true,
  imports: [MatCardModule, DatePipe],
  template: `
    <mat-card class="stat-card">
      <mat-card-header>
        <mat-card-title>{{ card().label }}</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <p class="stat-value">{{ card().displayValue }}</p>
        @if (card().activityName) {
          <p class="activity-name">{{ card().activityName }}</p>
        }
        @if (card().sportType) {
          <p class="sport-type">{{ card().sportType }}</p>
        }
        @if (card().startDateLocal) {
          <p class="start-date">{{ card().startDateLocal | date:'mediumDate' }}</p>
        }
        @if (card().stravaActivityUrl) {
          <a [href]="card().stravaActivityUrl" target="_blank" rel="noopener">View on Strava</a>
        }
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    .stat-card { height: 100%; }
    .stat-value { font-size: 1.5rem; font-weight: bold; margin: 8px 0; color: #FC4C02; }
    .activity-name { font-size: 0.9rem; color: rgba(0,0,0,0.7); margin: 4px 0; }
    .sport-type { font-size: 0.85rem; color: rgba(0,0,0,0.5); margin: 4px 0; }
    .start-date { font-size: 0.85rem; color: rgba(0,0,0,0.5); }
    a { color: #FC4C02; font-size: 0.85rem; }
  `]
})
export class StatCardComponent {
  card = input.required<StatCard>();
}
