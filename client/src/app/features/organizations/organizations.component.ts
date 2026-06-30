import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { OrganizationService } from '../../core/services/organization.service';
import { AuthService } from '../../core/services/auth.service';
import { Organization } from '../../core/models/organization.models';

@Component({
  selector: 'app-organizations',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './organizations.component.html',
  styleUrl: './organizations.component.scss'
})
export class OrganizationsComponent implements OnInit {
  organizations = signal<Organization[]>([]);
  myOrg = signal<Organization | null>(null);
  loading = signal(true);
  errorMsg = signal<string | null>(null);

  search = '';

  filtered = computed(() => {
    const q = this.search.trim().toLowerCase();
    return q
      ? this.organizations().filter(o =>
          o.name.toLowerCase().includes(q) ||
          (o.country ?? '').toLowerCase().includes(q) ||
          (o.address ?? '').toLowerCase().includes(q))
      : this.organizations();
  });

  showForm = signal(false);
  editingId = signal<string | null>(null);
  saving = signal(false);
  formError = signal<string | null>(null);

  formName = '';
  formCountry = '';
  formAddress = '';
  formWebSite = '';

  constructor(private orgService: OrganizationService, public auth: AuthService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading.set(true);
    if (this.auth.isAdmin()) {
      this.orgService.getAll().subscribe(orgs => {
        this.organizations.set(orgs);
        this.loading.set(false);
      });
    } else {
      this.orgService.getMine().subscribe({
        next: org => { this.myOrg.set(org); this.loading.set(false); },
        error: () => { this.myOrg.set(null); this.loading.set(false); }
      });
    }
  }

  openCreate() {
    this.formName = '';
    this.formCountry = '';
    this.formAddress = '';
    this.formWebSite = '';
    this.formError.set(null);
    this.editingId.set(null);
    this.showForm.set(true);
  }

  openEdit(org: Organization) {
    this.formName = org.name;
    this.formCountry = org.country ?? '';
    this.formAddress = org.address ?? '';
    this.formWebSite = org.webSite ?? '';
    this.formError.set(null);
    this.editingId.set(org.id);
    this.showForm.set(true);
  }

  closeForm() {
    this.showForm.set(false);
    this.editingId.set(null);
    this.formError.set(null);
  }

  save() {
    const req = {
      name: this.formName.trim(),
      country: this.formCountry.trim() || null,
      address: this.formAddress.trim() || null,
      webSite: this.formWebSite.trim() || null
    };
    this.saving.set(true);
    this.formError.set(null);
    const id = this.editingId();
    const op = !this.auth.isAdmin() && id
      ? this.orgService.updateMine(req)
      : id ? this.orgService.update(id, req) : this.orgService.create(req);

    op.subscribe({
      next: org => {
        if (id) {
          this.organizations.update(list => list.map(o => o.id === id ? { ...o, ...org } : o));
          if (this.myOrg()?.id === id) this.myOrg.update(cur => cur ? { ...cur, ...org } : cur);
        } else {
          this.organizations.update(list => [...list, org].sort((a, b) => a.name.localeCompare(b.name)));
        }
        this.saving.set(false);
        this.closeForm();
      },
      error: (err: HttpErrorResponse) => {
        this.formError.set(err.error?.error ?? 'A apărut o eroare. Încearcă din nou.');
        this.saving.set(false);
      }
    });
  }

  toggleActive(org: Organization) {
    this.errorMsg.set(null);
    this.orgService.toggleActive(org.id).subscribe({
      next: res => {
        this.organizations.update(list =>
          list.map(o => o.id === org.id ? { ...o, isActive: res.isActive } : o)
        );
      },
      error: () => this.errorMsg.set('Actualizarea statusului a eșuat.')
    });
  }

  delete(org: Organization) {
    if (!confirm(`Ștergi organizația "${org.name}"?\nAceastă acțiune va șterge și toate magazinele și datele asociate.`)) return;
    this.errorMsg.set(null);
    this.orgService.delete(org.id).subscribe({
      next: () => this.organizations.update(list => list.filter(o => o.id !== org.id)),
      error: (err: HttpErrorResponse) => {
        this.errorMsg.set(err.error?.error ?? 'Ștergerea a eșuat.');
      }
    });
  }

  logout() { this.auth.logout(); }
}
