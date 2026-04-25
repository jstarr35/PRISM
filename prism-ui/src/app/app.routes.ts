import { Routes } from '@angular/router';
import { ConnectStravaComponent } from './pages/connect-strava/connect-strava.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';

export const routes: Routes = [
  { path: '', redirectTo: 'connect', pathMatch: 'full' },
  { path: 'connect', component: ConnectStravaComponent },
  { path: 'dashboard', component: DashboardComponent },
];
