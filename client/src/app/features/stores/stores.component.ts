import { Component, OnInit, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { StoreService } from '../../core/services/store.service';
import { OrganizationService } from '../../core/services/organization.service';
import { AuthService } from '../../core/services/auth.service';
import { Store } from '../../core/models/store.models';
import { ApiKey, GeneratedApiKey } from '../../core/models/apikey.models';
import { Organization } from '../../core/models/organization.models';

@Component({
  selector: 'app-stores',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe, RouterLink],
  templateUrl: './stores.component.html',
  styleUrl: './stores.component.scss'
})
export class StoresComponent implements OnInit {
  stores = signal<Store[]>([]);
  organizations = signal<Organization[]>([]);
  loading = signal(true);

  showStoreForm = signal(false);
  editingId = signal<string | null>(null);
  saving = signal(false);

  formOrgId = '';
  formName = '';
  formStreet = '';
  formCountry = '';
  formWebsite = '';

  expandedStoreId = signal<string | null>(null);
  storeKeys = signal<ApiKey[]>([]);
  keysLoading = signal(false);
  generatedKey = signal<GeneratedApiKey | null>(null);

  constructor(
    private storeService: StoreService,
    private orgService: OrganizationService,
    public auth: AuthService
  ) {}

  ngOnInit() {
    this.storeService.getAll().subscribe(s => {
      this.stores.set(s);
      this.loading.set(false);
    });
    if (this.auth.isAdmin()) {
      this.orgService.getAll().subscribe(o => this.organizations.set(o));
    }
  }

  openCreate() {
    this.formOrgId = this.organizations()[0]?.id ?? '';
    this.formName = '';
    this.formStreet = '';
    this.formCountry = '';
    this.formWebsite = '';
    this.editingId.set(null);
    this.showStoreForm.set(true);
  }

  openEdit(store: Store) {
    this.formOrgId = store.organizationId;
    this.formName = store.storeName;
    this.formStreet = store.street ?? '';
    this.formCountry = store.country ?? '';
    this.formWebsite = store.website ?? '';
    this.editingId.set(store.id);
    this.showStoreForm.set(true);
  }

  closeForm() {
    this.showStoreForm.set(false);
    this.editingId.set(null);
  }

  save() {
    this.saving.set(true);
    const id = this.editingId();
    const op = id
      ? this.storeService.update(id, {
          storeName: this.formName,
          street: this.formStreet || null,
          country: this.formCountry || null,
          website: this.formWebsite || null
        })
      : this.storeService.create({
          organizationId: this.formOrgId,
          storeName: this.formName,
          street: this.formStreet || null,
          country: this.formCountry || null,
          website: this.formWebsite || null
        });

    op.subscribe({
      next: store => {
        if (id) {
          this.stores.update(list => list.map(s => s.id === id ? { ...s, ...store } : s));
        } else {
          this.stores.update(list => [store, ...list]);
        }
        this.saving.set(false);
        this.closeForm();
      },
      error: () => this.saving.set(false)
    });
  }

  deleteStore(store: Store) {
    if (!confirm(`Delete "${store.storeName}"? This cannot be undone.`)) return;
    this.storeService.delete(store.id).subscribe(() => {
      this.stores.update(list => list.filter(s => s.id !== store.id));
      if (this.expandedStoreId() === store.id) this.expandedStoreId.set(null);
    });
  }

  toggleKeys(store: Store) {
    if (this.expandedStoreId() === store.id) {
      this.expandedStoreId.set(null);
      this.generatedKey.set(null);
      return;
    }
    this.expandedStoreId.set(store.id);
    this.generatedKey.set(null);
    this.keysLoading.set(true);
    this.storeService.getApiKeys(store.id).subscribe(keys => {
      this.storeKeys.set(keys);
      this.keysLoading.set(false);
    });
  }

  generateKey(storeId: string) {
    this.storeService.generateApiKey({ storeId, expiresAt: null }).subscribe(key => {
      this.generatedKey.set(key);
      this.storeKeys.update(keys => [key, ...keys]);
      this.stores.update(list =>
        list.map(s => s.id === storeId ? { ...s, apiKeyCount: s.apiKeyCount + 1 } : s)
      );
    });
  }

  deactivateKey(keyId: string) {
    this.storeService.deactivateApiKey(keyId).subscribe(res => {
      this.storeKeys.update(keys =>
        keys.map(k => k.id === keyId ? { ...k, isActive: res.isActive } : k)
      );
    });
  }

  dismissGeneratedKey() {
    this.generatedKey.set(null);
  }

  logout() { this.auth.logout(); }
}
