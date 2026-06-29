import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  fullName = '';
  email = '';
  password = '';
  confirmPassword = '';
  error = signal('');
  loading = signal(false);

  constructor(private auth: AuthService, private router: Router) {}

  submit() {
    if (this.password !== this.confirmPassword) {
      this.error.set('Passwords do not match.');
      return;
    }
    if (this.password.length < 6) {
      this.error.set('Password must be at least 6 characters.');
      return;
    }

    this.error.set('');
    this.loading.set(true);

    this.auth.register({ fullName: this.fullName, email: this.email, password: this.password }).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (err) => {
        this.error.set(err.status === 409 ? 'Email already registered.' : 'Registration failed. Please try again.');
        this.loading.set(false);
      }
    });
  }
}
