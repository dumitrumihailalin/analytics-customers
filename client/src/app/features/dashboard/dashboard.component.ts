import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AnalyticsService } from '../../core/services/analytics.service';
import { AuthService } from '../../core/services/auth.service';
import { DashboardSummary } from '../../core/models/analytics.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, FormsModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  data = signal<DashboardSummary | null>(null);
  loading = signal(true);

  currentYear = new Date().getFullYear();
  selectedYear = this.currentYear;
  years = Array.from({ length: 5 }, (_, i) => this.currentYear - i);

  viewMode: 'weekly' | 'monthly' = 'weekly';
  selectedWeek: number | null = null;
  selectedMonth: number | null = null;

  readonly MONTHS = ['Ian', 'Feb', 'Mar', 'Apr', 'Mai', 'Iun', 'Iul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

  availableWeeks = computed(() =>
    [...new Set(this.data()?.weeklyRevenue.map(w => w.week) ?? [])].sort((a, b) => a - b)
  );

  availableMonths = computed(() =>
    [...new Set(this.data()?.monthlyRevenue.map(m => m.month) ?? [])].sort((a, b) => a - b)
  );

  filteredWeekly = computed(() => {
    const rows = this.data()?.weeklyRevenue ?? [];
    return this.selectedWeek ? rows.filter(r => r.week === this.selectedWeek) : rows;
  });

  filteredMonthly = computed(() => {
    const rows = this.data()?.monthlyRevenue ?? [];
    return this.selectedMonth ? rows.filter(r => r.month === this.selectedMonth) : rows;
  });

  periodRevenue = computed(() =>
    this.viewMode === 'weekly'
      ? this.filteredWeekly().reduce((s, r) => s + r.totalRevenue, 0)
      : this.filteredMonthly().reduce((s, r) => s + r.totalRevenue, 0)
  );

  periodQty = computed(() =>
    this.viewMode === 'weekly'
      ? this.filteredWeekly().reduce((s, r) => s + r.quantitySold, 0)
      : this.filteredMonthly().reduce((s, r) => s + r.quantitySold, 0)
  );

  periodOrders = computed(() =>
    this.viewMode === 'weekly'
      ? this.filteredWeekly().reduce((s, r) => s + r.orderCount, 0)
      : this.filteredMonthly().reduce((s, r) => s + r.orderCount, 0)
  );

  constructor(private analyticsService: AnalyticsService, public auth: AuthService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading.set(true);
    this.selectedWeek = null;
    this.selectedMonth = null;
    this.analyticsService.getDashboard(this.selectedYear).subscribe({
      next: d => { this.data.set(d); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  setView(mode: 'weekly' | 'monthly') {
    this.viewMode = mode;
    this.selectedWeek = null;
    this.selectedMonth = null;
  }

  monthName(m: number): string {
    return this.MONTHS[m - 1];
  }

  logout() { this.auth.logout(); }
}
