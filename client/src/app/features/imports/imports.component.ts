import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SubscriptionService } from '../../core/services/subscription.service';
import { AuthService } from '../../core/services/auth.service';
import { environment } from '../../../environments/environment';

interface EndpointRow {
  method: string;
  url: string;
  description: string;
}

@Component({
  selector: 'app-imports',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './imports.component.html',
  styleUrl: './imports.component.scss'
})
export class ImportsComponent implements OnInit {
  apiKey = signal<string | null>(null);
  copiedField = signal<string | null>(null);
  baseUrl = environment.apiUrl.replace('/api', '');

  endpoint: EndpointRow = {
    method: 'POST',
    url: `${this.baseUrl}/api/ingest`,
    description: 'Submit product sales data — no JWT required, authenticated via apiKey in the payload.'
  };

  payloadExample = '';

  constructor(
    private subService: SubscriptionService,
    public auth: AuthService
  ) {}

  ngOnInit() {
    this.subService.getInfo().subscribe(info => {
      this.apiKey.set(info.key);
      this.updatePayloadExample(info.key ?? 'YOUR_API_KEY');
    });
  }

  private updatePayloadExample(key: string) {
    this.payloadExample = JSON.stringify({
      apiKey: key,
      productId: 'PROD-001',
      categoryId: 'Electronics',
      price: 49.99,
      quantitySold: 3,
      stock: 120
    }, null, 2);
  }

  copy(text: string, field: string) {
    navigator.clipboard.writeText(text).then(() => {
      this.copiedField.set(field);
      setTimeout(() => this.copiedField.set(null), 2000);
    });
  }

  logout() { this.auth.logout(); }
}
