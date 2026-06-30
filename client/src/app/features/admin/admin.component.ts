import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AdminService } from '../../core/services/admin.service';
import { AnalyticsService } from '../../core/services/analytics.service';
import { OrganizationService } from '../../core/services/organization.service';
import { AuthService } from '../../core/services/auth.service';
import { AdminStats } from '../../core/models/analytics.models';
import { Organization } from '../../core/models/organization.models';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})
export class AdminComponent implements OnInit {
  users = signal<any[]>([]);
  organizations = signal<Organization[]>([]);
  stats = signal<AdminStats | null>(null);
  loading = signal(true);

  showCreateUser = signal(false);
  saving = signal(false);
  formError = signal<string | null>(null);
  formFullName = '';
  formEmail = '';
  formPassword = '';
  showPassword = false;

  constructor(
    private adminService: AdminService,
    private analyticsService: AnalyticsService,
    private orgService: OrganizationService,
    public auth: AuthService
  ) {}

  ngOnInit() {
    this.adminService.getUsers().subscribe(u => this.users.set(u));
    this.orgService.getAll().subscribe(orgs => this.organizations.set(orgs));
    this.analyticsService.getAdminStats().subscribe(s => { this.stats.set(s); this.loading.set(false); });
  }

  toggle(id: string) {
    this.adminService.toggleActive(id).subscribe(() =>
      this.users.update(list => list.map(u => u.id === id ? { ...u, isActive: !u.isActive } : u))
    );
  }

  openCreateUser() {
    this.formFullName = '';
    this.formEmail = '';
    this.formPassword = '';
    this.showPassword = false;
    this.formError.set(null);
    this.showCreateUser.set(true);
  }

  closeCreateUser() {
    this.showCreateUser.set(false);
    this.formError.set(null);
  }

  createUser() {
    this.saving.set(true);
    this.formError.set(null);
    this.adminService.createUser({
      fullName: this.formFullName,
      email: this.formEmail,
      password: this.formPassword
    }).subscribe({
      next: user => {
        this.users.update(list => [...list, user]);
        this.saving.set(false);
        this.closeCreateUser();
      },
      error: err => {
        this.formError.set(err.error?.error ?? 'Failed to create user.');
        this.saving.set(false);
      }
    });
  }

  toggleOrg(org: Organization) {
    this.orgService.toggleActive(org.id).subscribe(res =>
      this.organizations.update(list => list.map(o => o.id === org.id ? { ...o, isActive: res.isActive } : o))
    );
  }

  logout() { this.auth.logout(); }
}
