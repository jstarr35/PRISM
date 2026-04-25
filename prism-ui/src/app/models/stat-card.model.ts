export interface StatCard {
  key: string;
  label: string;
  value: number | null;
  unit: string;
  displayValue: string;
  activityId: number | null;
  activityName: string | null;
  sportType: string | null;
  startDateLocal: string | null;
  stravaActivityUrl: string | null;
}
