import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { StoreService } from '../../core/services/store.service';
import { OrganizationService } from '../../core/services/organization.service';
import { AuthService } from '../../core/services/auth.service';
import { Store } from '../../core/models/store.models';
import { Organization } from '../../core/models/organization.models';

@Component({
  selector: 'app-stores',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './stores.component.html',
  styleUrl: './stores.component.scss'
})
export class StoresComponent implements OnInit {
  stores = signal<Store[]>([]);
  organizations = signal<Organization[]>([]);
  myOrgId = signal<string | null>(null);
  copiedId = signal<string | null>(null);
  loading = signal(true);
  errorMsg = signal<string | null>(null);

  showStoreForm = signal(false);
  editingId = signal<string | null>(null);
  saving = signal(false);
  formError = signal<string | null>(null);

  formOrgId = '';
  formName = '';
  formStreet = '';
  formCountry = '';
  formWebsite = '';

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
    } else {
      this.orgService.getMine().subscribe({
        next: org => this.myOrgId.set(org.id),
        error: () => {}
      });
    }
  }

  openCreate() {
    this.formOrgId = this.auth.isAdmin()
      ? (this.organizations()[0]?.id ?? '')
      : (this.myOrgId() ?? '');
    this.formName = '';
    this.formStreet = '';
    this.formCountry = '';
    this.formWebsite = '';
    this.formError.set(null);
    this.editingId.set(null);
    this.showStoreForm.set(true);
  }

  openEdit(store: Store) {
    this.formOrgId = store.organizationId;
    this.formName = store.storeName;
    this.formStreet = store.street ?? '';
    this.formCountry = store.country ?? '';
    this.formWebsite = store.website ?? '';
    this.formError.set(null);
    this.editingId.set(store.id);
    this.showStoreForm.set(true);
  }

  closeForm() {
    this.showStoreForm.set(false);
    this.editingId.set(null);
    this.formError.set(null);
  }

  save() {
    this.saving.set(true);
    this.formError.set(null);
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
          this.stores.update(list => [...list, store].sort((a, b) => a.storeName.localeCompare(b.storeName)));
        }
        this.saving.set(false);
        this.closeForm();
      },
      error: (err) => {
        this.formError.set(err.error?.error ?? 'A apărut o eroare. Încearcă din nou.');
        this.saving.set(false);
      }
    });
  }

  deleteStore(store: Store) {
    if (!confirm(`Ștergi magazinul "${store.storeName}"? Această acțiune nu poate fi anulată.`)) return;
    this.errorMsg.set(null);
    this.storeService.delete(store.id).subscribe({
      next: () => {
        this.stores.update(list => list.filter(s => s.id !== store.id));
      },
      error: () => this.errorMsg.set('Ștergerea a eșuat.')
    });
  }

  copyId(id: string, key: string) {
    navigator.clipboard.writeText(id).then(() => {
      this.copiedId.set(key);
      setTimeout(() => this.copiedId.set(null), 2000);
    });
  }

  logout() { this.auth.logout(); }
}
