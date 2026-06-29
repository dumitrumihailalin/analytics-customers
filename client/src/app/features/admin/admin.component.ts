import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AdminService } from '../../core/services/admin.service';
import { AnalyticsService } from '../../core/services/analytics.service';
import { AuthService } from '../../core/services/auth.service';
import { AdminStats } from '../../core/models/analytics.models';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})
export class AdminComponent implements OnInit {
  users = signal<any[]>([]);
  stats = signal<AdminStats | null>(null);
  loading = signal(true);

  constructor(
    private adminService: AdminService,
    private analyticsService: AnalyticsService,
    public auth: AuthService
  ) {}

  ngOnInit() {
    this.adminService.getUsers().subscribe(u => this.users.set(u));
    this.analyticsService.getAdminStats().subscribe(s => { this.stats.set(s); this.loading.set(false); });
  }

  toggle(id: string) {
    this.adminService.toggleActive(id).subscribe(() =>
      this.users.update(list => list.map(u => u.id === id ? { ...u, isActive: !u.isActive } : u))
    );
  }

  logout() { this.auth.logout(); }
}
