import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzResultModule } from 'ng-zorro-antd/result';
import { AuthService } from '../../../core/services/auth.service';
import { finalize } from 'rxjs';

// Custom validator: pastikan password & konfirmasi sama
function passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
  const password = control.get('password')?.value;
  const confirm = control.get('confirmPassword')?.value;
  return password === confirm ? null : { passwordMismatch: true };
}

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzAlertModule,
    NzIconModule,
    NzResultModule,
  ],
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  loading = signal(false);
  errorMsg = signal<string | null>(null);
  success = signal(false);
  showPassword = signal(false);
  showConfirm = signal(false);

  form = this.fb.nonNullable.group(
    {
      username:        ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50), Validators.pattern(/^[a-zA-Z0-9_]+$/)]],
      email:           ['', [Validators.required, Validators.email]],
      password:        ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
    },
    { validators: passwordMatchValidator }
  );

  // Getter helpers untuk template
  get usernameErrors() {
    const c = this.form.controls.username;
    if (c.hasError('required')) return 'Username wajib diisi';
    if (c.hasError('minlength')) return 'Username minimal 3 karakter';
    if (c.hasError('pattern')) return 'Hanya huruf, angka, dan underscore (_)';
    return '';
  }

  get passwordErrors() {
    const c = this.form.controls.password;
    if (c.hasError('required')) return 'Password wajib diisi';
    if (c.hasError('minlength')) return 'Password minimal 8 karakter';
    return '';
  }

  get confirmErrors() {
    if (this.form.controls.confirmPassword.hasError('required')) return 'Konfirmasi password wajib diisi';
    if (this.form.hasError('passwordMismatch') && this.form.controls.confirmPassword.dirty) return 'Password tidak cocok';
    return '';
  }

  submit(): void {
    if (this.form.invalid) {
      Object.values(this.form.controls).forEach(c => {
        c.markAsDirty();
        c.updateValueAndValidity();
      });
      return;
    }

    this.loading.set(true);
    this.errorMsg.set(null);

    const { username, email, password } = this.form.getRawValue();

    this.auth.register({ username, email, password }).pipe(
      finalize(() => this.loading.set(false))
    ).subscribe({
      next: () => this.success.set(true),
      error: (err) => {
        const msg = err?.error?.message ?? err?.error?.errors?.join(', ') ?? 'Registrasi gagal. Coba lagi.';
        this.errorMsg.set(msg);
      }
    });
  }

  goToLogin(): void {
    this.router.navigate(['/auth/login']);
  }
}