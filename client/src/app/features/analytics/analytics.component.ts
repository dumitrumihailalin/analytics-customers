import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AnalyticsService } from '../../core/services/analytics.service';
import { AuthService } from '../../core/services/auth.service';
import { DashboardSummary, ProductBreakdown } from '../../core/models/analytics.models';

@Component({
  selector: 'app-analytics',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, DecimalPipe, FormsModule, RouterLink],
  templateUrl: './analytics.component.html',
  styleUrl: './analytics.component.scss'
})
export class AnalyticsComponent implements OnInit {
  data = signal<DashboardSummary | null>(null);
  loading = signal(true);

  currentYear = new Date().getFullYear();
  selectedYear = this.currentYear;
  years = Array.from({ length: 5 }, (_, i) => this.currentYear - i);

  totalProductRevenue = computed(() =>
    this.data()?.productBreakdown.reduce((s, p) => s + p.totalRevenue, 0) ?? 0
  );

  constructor(private analyticsService: AnalyticsService, public auth: AuthService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading.set(true);
    this.analyticsService.getDashboard(this.selectedYear).subscribe({
      next: d => { this.data.set(d); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  onYearChange() {
    this.load();
  }

  share(revenue: number): number {
    const total = this.totalProductRevenue();
    return total > 0 ? Math.round((revenue / total) * 100) : 0;
  }

  logout() { this.auth.logout(); }
}
