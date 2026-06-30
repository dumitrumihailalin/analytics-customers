import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';
import { orgAdminGuard } from './core/guards/org-admin.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
  },
  {
    path: 'subscription',
    canActivate: [authGuard],
    loadComponent: () => import('./features/subscription/subscription.component').then(m => m.SubscriptionComponent)
  },
  {
    path: 'imports',
    canActivate: [authGuard],
    loadComponent: () => import('./features/imports/imports.component').then(m => m.ImportsComponent)
  },
  {
    path: 'analytics',
    canActivate: [authGuard],
    loadComponent: () => import('./features/analytics/analytics.component').then(m => m.AnalyticsComponent)
  },
  {
    path: 'stores',
    canActivate: [authGuard],
    loadComponent: () => import('./features/stores/stores.component').then(m => m.StoresComponent)
  },
  {
    path: 'organizations',
    canActivate: [authGuard],
    loadComponent: () => import('./features/organizations/organizations.component').then(m => m.OrganizationsComponent)
  },
  {
    path: 'profile',
    canActivate: [authGuard],
    loadComponent: () => import('./features/profile/profile.component').then(m => m.ProfileComponent)
  },
  {
    path: 'admin',
    canActivate: [authGuard, adminGuard],
    loadComponent: () => import('./features/admin/admin.component').then(m => m.AdminComponent)
  },
  { path: '**', redirectTo: 'dashboard' }
];
