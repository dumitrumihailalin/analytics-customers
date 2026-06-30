import { Component, OnInit, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { SubscriptionService } from '../../core/services/subscription.service';
import { StoreService } from '../../core/services/store.service';
import { AuthService } from '../../core/services/auth.service';
import { SubscriptionInfo, SubscriptionKey } from '../../core/models/subscription.models';
import { Store } from '../../core/models/store.models';

@Component({
  selector: 'app-subscription',
  standalone: true,
  imports: [CommonModule, DatePipe, FormsModule, RouterLink],
  templateUrl: './subscription.component.html',
  styleUrl: './subscription.component.scss'
})
export class SubscriptionComponent implements OnInit {
  info = signal<SubscriptionInfo | null>(null);
  history = signal<SubscriptionKey[]>([]);
  stores = signal<Store[]>([]);
  loading = signal(true);
  keyCopied = signal(false);
  storeIdCopied = signal(false);

  showModal = signal(false);
  generating = signal(false);
  generatedKey = signal<string | null>(null);
  genCopied = signal(false);
  selectedStoreId = '';

  constructor(
    private subService: SubscriptionService,
    private storeService: StoreService,
    public auth: AuthService
  ) {}

  ngOnInit() {
    this.subService.getInfo().subscribe(i => { this.info.set(i); this.loading.set(false); });
    this.subService.getKeyHistory().subscribe(h => this.history.set(h));
    this.storeService.getAll().subscribe(s => this.stores.set(s));
  }

  openModal() {
    this.generatedKey.set(null);
    this.genCopied.set(false);
    this.selectedStoreId = this.info()?.storeId ?? '';
    this.showModal.set(true);
  }

  closeModal() {
    this.showModal.set(false);
    this.generatedKey.set(null);
    if (this.generating()) return;
    this.subService.getInfo().subscribe(i => this.info.set(i));
    this.subService.getKeyHistory().subscribe(h => this.history.set(h));
  }

  generate() {
    this.generating.set(true);
    this.subService.generateKey(this.selectedStoreId || undefined).subscribe({
      next: res => {
        this.generatedKey.set(res.key);
        this.generating.set(false);
        this.subService.getInfo().subscribe(i => this.info.set(i));
        this.subService.getKeyHistory().subscribe(h => this.history.set(h));
      },
      error: () => this.generating.set(false)
    });
  }

  copyStoreId(id: string) {
    navigator.clipboard.writeText(id).then(() => {
      this.storeIdCopied.set(true);
      setTimeout(() => this.storeIdCopied.set(false), 2000);
    });
  }

  copyKey(key: string) {
    navigator.clipboard.writeText(key).then(() => {
      this.keyCopied.set(true);
      setTimeout(() => this.keyCopied.set(false), 2000);
    });
  }

  copyGenerated() {
    const k = this.generatedKey();
    if (!k) return;
    navigator.clipboard.writeText(k).then(() => {
      this.genCopied.set(true);
      setTimeout(() => this.genCopied.set(false), 2000);
    });
  }

  urgencyClass(days: number): string {
    if (days <= 7) return 'urgent';
    if (days <= 30) return 'warning';
    return 'ok';
  }

  logout() { this.auth.logout(); }
}
