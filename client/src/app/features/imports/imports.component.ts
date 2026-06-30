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

  singleEndpoint: EndpointRow = {
    method: 'POST',
    url: `${this.baseUrl}/api/ingest/analytics`,
    description: 'Ingest a single analytics record — authenticated via apiKey in the payload.'
  };

  bulkEndpoint: EndpointRow = {
    method: 'POST',
    url: `${this.baseUrl}/api/ingest/analytics/bulk`,
    description: 'Ingest multiple records in one request.'
  };

  payloadExample = '';
  bulkPayloadExample = '';

  constructor(
    private subService: SubscriptionService,
    public auth: AuthService
  ) {}

  ngOnInit() {
    this.subService.getInfo().subscribe(info => {
      this.apiKey.set(info.key);
      this.updatePayloadExamples(info.key ?? 'YOUR_API_KEY');
    });
  }

  private updatePayloadExamples(key: string) {
    this.payloadExample = JSON.stringify({
      apiKey: key,
      productId: 'PROD-001',
      price: 49.99,
      quantitySold: 3,
      stock: 120
    }, null, 2);

    this.bulkPayloadExample = JSON.stringify({
      apiKey: key,
      items: [
        { productId: 'PROD-001', price: 49.99, quantitySold: 3, stock: 120 },
        { productId: 'PROD-002', price: 19.99, quantitySold: 10, stock: 45 }
      ]
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
