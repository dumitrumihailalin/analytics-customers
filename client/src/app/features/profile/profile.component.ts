import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ProfileService } from '../../core/services/profile.service';
import { AuthService } from '../../core/services/auth.service';
import { ProfileResponse } from '../../core/models/profile.models';

interface PermItem { label: string; description: string; granted: boolean; }
interface PermGroup { title: string; items: PermItem[]; }

const ALL_PERMISSIONS: { label: string; description: string; levels: string[] }[] = [
  // Data & Analytics
  { label: 'View Dashboard',        description: 'Access revenue and order summaries',           levels: ['User', 'OrgAdmin', 'PlatformAdmin'] },
  { label: 'View Analytics',        description: 'Weekly and monthly breakdowns by category',    levels: ['User', 'OrgAdmin', 'PlatformAdmin'] },
  { label: 'View Subscription',     description: 'See active plan and API key history',          levels: ['User', 'OrgAdmin', 'PlatformAdmin'] },
  { label: 'Submit Data (Imports)', description: 'Send product analytics via ingest endpoint',   levels: ['User', 'OrgAdmin', 'PlatformAdmin'] },
  // Stores & Keys
  { label: 'View Stores',           description: 'See stores in own organization',               levels: ['OrgAdmin', 'PlatformAdmin'] },
  { label: 'Manage Stores',         description: 'Create, edit and delete stores',               levels: ['OrgAdmin', 'PlatformAdmin'] },
  { label: 'Generate API Keys',     description: 'Create and revoke API keys for stores',        levels: ['OrgAdmin', 'PlatformAdmin'] },
  // Organizations & Admin
  { label: 'View Organizations',    description: 'See all organizations on the platform',        levels: ['PlatformAdmin'] },
  { label: 'Manage Organizations',  description: 'Create, edit, delete organizations',           levels: ['PlatformAdmin'] },
  { label: 'Manage All Stores',     description: 'Administer stores across all organizations',   levels: ['PlatformAdmin'] },
  { label: 'Manage Users',          description: 'View and activate / deactivate user accounts', levels: ['PlatformAdmin'] },
  { label: 'Access Admin Panel',    description: 'View platform-wide stats and user list',       levels: ['PlatformAdmin'] },
];

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe, RouterLink],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  profile = signal<ProfileResponse | null>(null);
  loading = signal(true);
  saving = signal(false);
  saved = signal(false);

  fullName = '';

  permissionGroups = computed<PermGroup[]>(() => {
    const level = this.profile()?.level ?? 'User';

    const toItem = (p: typeof ALL_PERMISSIONS[0]): PermItem => ({
      label: p.label,
      description: p.description,
      granted: p.levels.includes(level)
    });

    return [
      {
        title: 'Data & Analytics',
        items: ALL_PERMISSIONS.slice(0, 4).map(toItem)
      },
      {
        title: 'Stores & API Keys',
        items: ALL_PERMISSIONS.slice(4, 7).map(toItem)
      },
      {
        title: 'Organizations & Administration',
        items: ALL_PERMISSIONS.slice(7).map(toItem)
      }
    ];
  });

  constructor(private profileService: ProfileService, public auth: AuthService) {}

  ngOnInit() {
    this.profileService.get().subscribe(p => {
      this.profile.set(p);
      this.fullName = p.fullName;
      this.loading.set(false);
    });
  }

  save() {
    this.saving.set(true);
    this.profileService.update({ fullName: this.fullName }).subscribe({
      next: p => {
        this.profile.set(p);
        this.saving.set(false);
        this.saved.set(true);
        setTimeout(() => this.saved.set(false), 2500);
      },
      error: () => this.saving.set(false)
    });
  }

  logout() { this.auth.logout(); }
}
