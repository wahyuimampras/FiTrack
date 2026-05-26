  // src/app/features/accounts/accounts.component.ts
  import { Component, OnInit, inject, signal, computed } from '@angular/core';
  import { CommonModule } from '@angular/common';
  import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
  import { HttpClient } from '@angular/common/http';
  import { finalize } from 'rxjs';

  import { NzCardModule } from 'ng-zorro-antd/card';
  import { NzButtonModule } from 'ng-zorro-antd/button';
  import { NzModalModule } from 'ng-zorro-antd/modal';
  import { NzFormModule } from 'ng-zorro-antd/form';
  import { NzInputModule } from 'ng-zorro-antd/input';
  import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
  import { NzSelectModule } from 'ng-zorro-antd/select';
  import { NzTagModule } from 'ng-zorro-antd/tag';
  import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
  import { NzMessageService } from 'ng-zorro-antd/message';
  import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
  import { NzSwitchModule } from 'ng-zorro-antd/switch';
  import { NzEmptyModule } from 'ng-zorro-antd/empty';
  import { NzIconModule } from 'ng-zorro-antd/icon';
  import { NzDividerModule } from 'ng-zorro-antd/divider';
  import { NzToolTipModule } from 'ng-zorro-antd/tooltip';

  import { environment } from '../../../environments/environment';
  import { AccountDto } from '../../core/models/dashboard.model';

  interface AccountType {
    value: string;
    label: string;
    icon: string;
    color: string;
  }

  @Component({
    selector: 'app-accounts',
    standalone: true,
    imports: [
      CommonModule,
      ReactiveFormsModule,
      NzCardModule,
      NzButtonModule,
      NzModalModule,
      NzFormModule,
      NzInputModule,
      NzInputNumberModule,
      NzSelectModule,
      NzTagModule,
      NzPopconfirmModule,
      NzSkeletonModule,
      NzSwitchModule,
      NzEmptyModule,
      NzIconModule,
      NzDividerModule,
      NzToolTipModule,
    ],
    templateUrl: './accounts.component.html',
  })
  export class AccountsComponent implements OnInit {
    private http = inject(HttpClient);
    private fb = inject(FormBuilder);
    private msg = inject(NzMessageService);
    private api = environment.apiUrl;

    // State
    loading = signal(true);
    submitting = signal(false);
    accounts = signal<AccountDto[]>([]);
    showModal = signal(false);
    editingAccount = signal<AccountDto | null>(null);

    // Account types config
    accountTypes: AccountType[] = [
      { value: 'Cash',       label: 'Tunai',      icon: '💵', color: '#059669' },
      { value: 'Bank',       label: 'Bank',        icon: '🏦', color: '#0284c7' },
      { value: 'EWallet',    label: 'E-Wallet',    icon: '📱', color: '#7c3aed' },
      { value: 'Investment', label: 'Investasi',   icon: '📈', color: '#d97706' },
    ];

    // Preset colors
    colorPresets = [
      '#285A48', '#059669', '#0284c7', '#7c3aed',
      '#d97706', '#dc2626', '#db2777', '#0891b2',
    ];

    // Form
    form = this.fb.nonNullable.group({
      name:           ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      type:           ['Bank', [Validators.required]],
      initialBalance: [0, [Validators.required, Validators.min(0)]],
      color:          ['#285A48'],
      icon:           [''],
      isActive:       [true],
    });

    // Computed
    totalBalance = computed(() =>
      this.accounts().filter(a => a.isActive).reduce((s, a) => s + a.balance, 0)
    );

    activeAccounts = computed(() => this.accounts().filter(a => a.isActive));
    inactiveAccounts = computed(() => this.accounts().filter(a => !a.isActive));

    isEditing = computed(() => !!this.editingAccount());
    modalTitle = computed(() => this.isEditing() ? 'Edit Rekening' : 'Tambah Rekening Baru');
    submitButtonText = computed(() => {
      if (this.submitting()) return 'Menyimpan...';
      if (this.isEditing()) return 'Simpan Perubahan';
      return 'Tambah Rekening';
    });

    ngOnInit(): void {
      this.loadAccounts();
    }

    loadAccounts(): void {
      this.loading.set(true);
      this.http.get<AccountDto[]>(`${this.api}/account`)
        .pipe(finalize(() => this.loading.set(false)))
        .subscribe({
          next: data => this.accounts.set(data),
          error: () => this.msg.error('Gagal memuat data rekening'),
        });
    }

    // ── MODAL ──────────────────────────────────────
    openAdd(): void {
      this.editingAccount.set(null);
      this.form.reset({
        name: '', type: 'Bank', initialBalance: 0,
        color: '#285A48', icon: '', isActive: true,
      });
      this.showModal.set(true);
    }

    openEdit(account: AccountDto): void {
      this.editingAccount.set(account);
      this.form.patchValue({
        name:           account.name,
        type:           account.type,
        initialBalance: account.balance,
        color:          account.color ?? '#285A48',
        icon:           account.icon ?? '',
        isActive:       account.isActive,
      });
      this.showModal.set(true);
    }

    closeModal(): void {
      this.showModal.set(false);
      this.editingAccount.set(null);
      this.form.reset();
    }

    submit(): void {
      if (this.form.invalid) {
        Object.values(this.form.controls).forEach(c => {
          c.markAsDirty();
          c.updateValueAndValidity();
        });
        return;
      }

      this.submitting.set(true);
      const val = this.form.getRawValue();

      if (this.isEditing()) {
        const acc = this.editingAccount()!;
        const payload = {
          id:       acc.id,
          name:     val.name,
          type:     val.type,
          balance:  val.initialBalance,
          color:    val.color || null,
          icon:     val.icon || null,
          isActive: val.isActive,
        };

        this.http.put(`${this.api}/account/${acc.id}`, payload)
          .pipe(finalize(() => this.submitting.set(false)))
          .subscribe({
            next: () => {
              this.msg.success('Rekening berhasil diperbarui');
              this.closeModal();
              this.loadAccounts();
            },
            error: e => this.msg.error(e?.error?.message ?? 'Gagal memperbarui rekening'),
          });
      } else {
        const payload = {
          name:           val.name,
          type:           val.type,
          initialBalance: val.initialBalance,
          color:          val.color || null,
          icon:           val.icon || null,
        };

        this.http.post(`${this.api}/account`, payload)
          .pipe(finalize(() => this.submitting.set(false)))
          .subscribe({
            next: () => {
              this.msg.success('Rekening berhasil ditambahkan');
              this.closeModal();
              this.loadAccounts();
            },
            error: e => this.msg.error(e?.error?.message ?? 'Gagal menambah rekening'),
          });
      }
    }

    delete(id: string): void {
      this.http.delete(`${this.api}/account/${id}`).subscribe({
        next: () => {
          this.msg.success('Rekening berhasil dihapus');
          this.loadAccounts();
        },
        error: e => this.msg.error(e?.error?.message ?? 'Gagal menghapus rekening'),
      });
    }

    // ── HELPERS ────────────────────────────────────
    getTypeConfig(type: string): AccountType {
      return this.accountTypes.find(t => t.value === type)
        ?? { value: type, label: type, icon: '💰', color: '#285A48' };
    }

    formatRupiah(amount: number): string {
      return new Intl.NumberFormat('id-ID', {
        style: 'currency', currency: 'IDR',
        minimumFractionDigits: 0, maximumFractionDigits: 0
      }).format(amount);
    }

    getTypeTotal(typeValue: string): number {
    return this.accounts()
      .filter(a => a.type === typeValue && a.isActive)
      .reduce((s, a) => s + a.balance, 0);
    }

    getTypeCount(typeValue: string): number {
      return this.accounts().filter(a => a.type === typeValue).length;
    }

    selectColor(color: string): void {
      this.form.patchValue({ color });
    }

    get selectedColor(): string {
      return this.form.get('color')?.value ?? '#285A48';
    }
  }