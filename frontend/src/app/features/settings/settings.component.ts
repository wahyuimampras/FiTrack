import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../core/services/auth.service';
import { finalize } from 'rxjs';
import { environment } from '../../../environments/environment';

function passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
  const pw = control.get('newPassword')?.value;
  const cf = control.get('confirmPassword')?.value;
  return pw === cf ? null : { passwordMismatch: true };
}

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzCardModule,
    NzTabsModule,
    NzAlertModule,
    NzDividerModule,
    NzPopconfirmModule,
    NzTagModule,
    NzIconModule,
  ],
  templateUrl: './settings.component.html',
})
export class SettingsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private http = inject(HttpClient);
  private msg = inject(NzMessageService);
  readonly auth = inject(AuthService);

  private readonly API = environment.apiUrl;

  // State
  loadingProfile = signal(false);
  loadingPassword = signal(false);
  loadingRegister = signal(false);
  sessions = signal<any[]>([]);

  // Profile form
  profileForm = this.fb.nonNullable.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    email:    ['', [Validators.required, Validators.email]],
  });

  // Change password form
  passwordForm = this.fb.nonNullable.group(
    {
      currentPassword: ['', [Validators.required]],
      newPassword:     ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
    },
    { validators: passwordMatchValidator }
  );

  // Register new user form (admin buat akun baru)
  registerForm = this.fb.nonNullable.group(
    {
      username:        ['', [Validators.required, Validators.minLength(3), Validators.pattern(/^[a-zA-Z0-9_]+$/)]],
      email:           ['', [Validators.required, Validators.email]],
      password:        ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
    },
    { validators: (c: AbstractControl) => {
      const pw = c.get('password')?.value;
      const cf = c.get('confirmPassword')?.value;
      return pw === cf ? null : { passwordMismatch: true };
    }}
  );

  ngOnInit(): void {
    // Isi profile form dari current user
    const user = this.auth.currentUser();
    if (user) {
      this.profileForm.patchValue({
        username: user.username,
        email: user.email,
      });
    }
    this.loadSessions();
  }

  // ── Update Profile ────────────────────────────
  saveProfile(): void {
    if (this.profileForm.invalid) return;
    this.loadingProfile.set(true);

    this.http.put(`${this.API}/user/profile`, this.profileForm.getRawValue())
      .pipe(finalize(() => this.loadingProfile.set(false)))
      .subscribe({
        next: () => this.msg.success('Profil berhasil diperbarui'),
        error: (e) => this.msg.error(e?.error?.message ?? 'Gagal memperbarui profil'),
      });
  }

  // ── Change Password ───────────────────────────
  changePassword(): void {
    if (this.passwordForm.invalid) return;
    this.loadingPassword.set(true);

    const { currentPassword, newPassword } = this.passwordForm.getRawValue();
    this.http.put(`${this.API}/user/change-password`, { currentPassword, newPassword })
      .pipe(finalize(() => this.loadingPassword.set(false)))
      .subscribe({
        next: () => {
          this.msg.success('Password berhasil diubah');
          this.passwordForm.reset();
        },
        error: (e) => this.msg.error(e?.error?.message ?? 'Gagal mengubah password'),
      });
  }

  // ── Register New User ─────────────────────────
  registerNewUser(): void {
    if (this.registerForm.invalid) {
      Object.values(this.registerForm.controls).forEach(c => { c.markAsDirty(); c.updateValueAndValidity(); });
      return;
    }
    this.loadingRegister.set(true);

    const { username, email, password } = this.registerForm.getRawValue();
    this.http.post(`${this.API}/auth/register`, { username, email, password })
      .pipe(finalize(() => this.loadingRegister.set(false)))
      .subscribe({
        next: () => {
          this.msg.success(`Akun "${username}" berhasil dibuat!`);
          this.registerForm.reset();
        },
        error: (e) => this.msg.error(e?.error?.message ?? 'Gagal membuat akun'),
      });
  }

  // ── Sessions ──────────────────────────────────
  loadSessions(): void {
    this.http.get<any[]>(`${this.API}/auth/sessions`).subscribe({
      next: (data) => this.sessions.set(data),
      error: () => {}
    });
  }

  revokeSession(id: string): void {
    this.http.delete(`${this.API}/auth/sessions/${id}`).subscribe({
      next: () => {
        this.msg.success('Sesi berhasil diakhiri');
        this.loadSessions();
      },
      error: () => this.msg.error('Gagal mengakhiri sesi'),
    });
  }

  revokeAllSessions(): void {
    this.http.delete(`${this.API}/auth/sessions`).subscribe({
      next: () => {
        this.msg.success('Semua sesi berhasil diakhiri');
        this.auth.logout();
      },
      error: () => this.msg.error('Gagal mengakhiri semua sesi'),
    });
  }

  logout(): void {
    this.auth.logout();
  }

  get confirmPasswordError(): string {
    if (this.passwordForm.controls.confirmPassword.hasError('required')) return 'Wajib diisi';
    if (this.passwordForm.hasError('passwordMismatch') && this.passwordForm.controls.confirmPassword.dirty) return 'Password tidak cocok';
    return '';
  }

  get registerConfirmError(): string {
    if (this.registerForm.controls.confirmPassword.hasError('required')) return 'Wajib diisi';
    if (this.registerForm.hasError('passwordMismatch') && this.registerForm.controls.confirmPassword.dirty) return 'Password tidak cocok';
    return '';
  }
}