import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

  // Auth pages (tanpa sidebar layout)
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },

  // App pages (dengan sidebar layout, butuh login)
  {
    path: '',
    loadComponent: () => import('./shared/layout/main-layout.component').then(m => m.MainLayoutComponent),
    canActivate: [authGuard],
    children: [
      { path: 'dashboard',    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) },
      { path: 'accounts',     loadComponent: () => import('./features/accounts/accounts.component').then(m => m.AccountsComponent) },
      { path: 'transactions', loadComponent: () => import('./features/finance/transactions/transactions.component').then(m => m.TransactionsComponent) },
      { path: 'budgets',      loadComponent: () => import('./features/finance/budgets/budgets.component').then(m => m.BudgetsComponent) },
      { path: 'saving-goals', loadComponent: () => import('./features/finance/saving-goals/saving-goals.component').then(m => m.SavingGoalsComponent) },
      { path: 'activities',   loadComponent: () => import('./features/workout/activities/activities.component').then(m => m.ActivitiesComponent) },
      { path: 'workout/stats',loadComponent: () => import('./features/workout/stats/workout-stats.component').then(m => m.WorkoutStatsComponent) },
      { path: 'strava',       loadComponent: () => import('./features/strava/strava.component').then(m => m.StravaComponent) },
      { path: 'trainings',     loadComponent: () => import('./features/workout/trainings/trainings.component').then(m => m.TrainingComponent)},
      { path: 'reports',      loadComponent: () => import('./features/reports/reports.component').then(m => m.ReportsComponent) },
      { path: 'settings',     loadComponent: () => import('./features/settings/settings.component').then(m => m.SettingsComponent) },
    ]
  },

  { path: '**', redirectTo: 'dashboard' }
];