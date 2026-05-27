import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { finalize, forkJoin, catchError, of } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

import { NzTableModule }       from 'ng-zorro-antd/table';
import { NzButtonModule }      from 'ng-zorro-antd/button';
import { NzModalModule }       from 'ng-zorro-antd/modal';
import { NzFormModule }        from 'ng-zorro-antd/form';
import { NzInputModule }       from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzSelectModule }      from 'ng-zorro-antd/select';
import { NzDatePickerModule }  from 'ng-zorro-antd/date-picker';
import { NzTagModule }         from 'ng-zorro-antd/tag';
import { NzPopconfirmModule }  from 'ng-zorro-antd/popconfirm';
import { NzMessageService }    from 'ng-zorro-antd/message';
import { NzSkeletonModule }    from 'ng-zorro-antd/skeleton';
import { NzEmptyModule }       from 'ng-zorro-antd/empty';
import { NzToolTipModule }     from 'ng-zorro-antd/tooltip';
import { NzIconModule }        from 'ng-zorro-antd/icon';
import { NzDividerModule }     from 'ng-zorro-antd/divider';

import { environment } from '../../../../environments/environment';

export interface TransactionDto {
  id: string; accountId: string; categoryId: string;
  type: 'Income' | 'Expense' | 'Transfer';
  amount: number; description: string; date: string;
  notes?: string; createdAt: string;
}

export interface PagedResult<T> {
  items: T[]; totalCount: number; page: number;
  pageSize: number; totalPages: number;
  hasNextPage: boolean; hasPreviousPage: boolean;
}

export interface AccountDto {
  id: string; name: string; type: string;
  balance: number; color?: string; isActive: boolean;
}

export interface CategoryDto {
  id: string; name: string; type: string; icon?: string; color?: string;
}

export interface TransactionRow extends TransactionDto {
  accountName: string; categoryName: string; categoryIcon: string;
}

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule,
    NzTableModule, NzButtonModule, NzModalModule, NzFormModule,
    NzInputModule, NzInputNumberModule, NzSelectModule, NzDatePickerModule,
    NzTagModule, NzPopconfirmModule, NzSkeletonModule, NzEmptyModule,
    NzToolTipModule, NzIconModule, NzDividerModule,
  ],
  templateUrl: './transactions.component.html',
})
export class TransactionsComponent implements OnInit {
  private http = inject(HttpClient);
  private fb   = inject(FormBuilder);
  private msg  = inject(NzMessageService);
  private api  = environment.apiUrl;

  // Static config arrays (untuk template)
  readonly filterOptions = [
    { key: 'All',      label: '🗂 Semua' },
    { key: 'Income',   label: '💰 Pemasukan' },
    { key: 'Expense',  label: '💸 Pengeluaran' },
    { key: 'Transfer', label: '↔ Transfer' },
  ];

  readonly txTypes = [
    { v: 'Expense',  l: '💸 Pengeluaran' },
    { v: 'Income',   l: '💰 Pemasukan' },
    { v: 'Transfer', l: '↔ Transfer' },
  ];

  // State
  loading      = signal(true);
  submitting   = signal(false);
  transactions = signal<TransactionRow[]>([]);
  accounts     = signal<AccountDto[]>([]);
  categories   = signal<CategoryDto[]>([]);
  showModal    = signal(false);
  editingTx    = signal<TransactionRow | null>(null);
  filterType   = signal<string>('All');

  // Pagination
  page       = signal(1);
  pageSize   = signal(10);
  totalCount = signal(0);

  // Form
  form = this.fb.nonNullable.group({
    accountId:   ['', [Validators.required]],
    categoryId:  [''],
    type:        ['Expense', [Validators.required]],
    amount:      [0, [Validators.required, Validators.min(1)]],
    description: ['', [Validators.required, Validators.minLength(2)]],
    date:        [new Date() as Date, [Validators.required]],
    notes:       [''],
  });

  // Track form type value
  formType = toSignal(this.form.controls.type.valueChanges, { initialValue: 'Expense' });

  // Computed
  isEditing  = computed(() => !!this.editingTx());
  modalTitle = computed(() => this.isEditing() ? 'Edit Transaksi' : 'Tambah Transaksi');

  activeAccounts = computed(() => this.accounts().filter(a => a.isActive));

  filteredCategories = computed(() => {
    const type = this.formType();
    if (type === 'Transfer') return [];
    return this.categories().filter(c => c.type === (type === 'Income' ? 'Income' : 'Expense'));
  });

  pageSummary = signal({ income: 0, expense: 0 });

  ngOnInit(): void {
    forkJoin({
      accounts:   this.http.get<AccountDto[]>(`${this.api}/account`).pipe(catchError(() => of([]))),
      categories: this.http.get<CategoryDto[]>(`${this.api}/categories`).pipe(catchError(() => of([]))),
    }).subscribe(({ accounts, categories }) => {
      this.accounts.set(accounts);
      this.categories.set(categories);
      this.loadTransactions();
      this.loadSummary();
    });
  }

  loadSummary(): void {
    this.http.get<{totalIncome: number, totalExpense: number}>(`${this.api}/transactions/summary`)
      .pipe(catchError(() => of({ totalIncome: 0, totalExpense: 0 })))
      .subscribe(res => {
        this.pageSummary.set({ income: res.totalIncome, expense: res.totalExpense });
      });
  }

  loadTransactions(): void {
    this.loading.set(true);
    let params = new HttpParams()
      .set('page', this.page())
      .set('pageSize', this.pageSize());

    if (this.filterType() !== 'All') {
      params = params.set('type', this.filterType());
    }

    this.http.get<PagedResult<TransactionDto>>(`${this.api}/transactions`, { params })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: result => {
          this.totalCount.set(result.totalCount);
          this.transactions.set(result.items.map(tx => this.enrich(tx)));
        },
        error: () => this.msg.error('Gagal memuat transaksi'),
      });
  }

  // Modal
  openAdd(): void {
    this.editingTx.set(null);
    this.form.reset({
      accountId: this.activeAccounts()[0]?.id ?? '',
      categoryId: '', type: 'Expense', amount: 0,
      description: '', date: new Date(), notes: '',
    });
    this.showModal.set(true);
  }

  openEdit(tx: TransactionRow): void {
    this.editingTx.set(tx);
    this.form.patchValue({
      accountId: tx.accountId, categoryId: tx.categoryId,
      type: tx.type, amount: tx.amount,
      description: tx.description,
      date: new Date(tx.date), notes: tx.notes ?? '',
    });
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
    this.editingTx.set(null);
    this.form.reset();
  }

  submit(): void {
    // Validasi categoryId hanya jika bukan Transfer
    const type = this.form.get('type')?.value;
    if (type !== 'Transfer') {
      this.form.get('categoryId')?.setValidators([Validators.required]);
    } else {
      this.form.get('categoryId')?.clearValidators();
    }
    this.form.get('categoryId')?.updateValueAndValidity();

    if (this.form.invalid) {
      Object.values(this.form.controls).forEach(c => { c.markAsDirty(); c.updateValueAndValidity(); });
      return;
    }

    this.submitting.set(true);
    const val = this.form.getRawValue();
    const payload = {
      accountId:   val.accountId,
      categoryId:  type !== 'Transfer' ? val.categoryId : null,
      type:        val.type,
      amount:      val.amount,
      description: val.description,
      date:        (val.date as Date).toISOString(),
      notes:       val.notes || null,
    };

    if (this.isEditing()) {
      const tx = this.editingTx()!;
      this.http.put(`${this.api}/transactions/${tx.id}`, { id: tx.id, ...payload })
        .pipe(finalize(() => this.submitting.set(false)))
        .subscribe({
          next:  () => { this.msg.success('Transaksi diperbarui'); this.closeModal(); this.loadTransactions(); },
          error: e  => this.msg.error(e?.error?.message ?? 'Gagal memperbarui transaksi'),
        });
    } else {
      this.http.post(`${this.api}/transactions`, payload)
        .pipe(finalize(() => this.submitting.set(false)))
        .subscribe({
          next:  () => { this.msg.success('Transaksi ditambahkan'); this.closeModal(); this.loadTransactions(); },
          error: e  => this.msg.error(e?.error?.message ?? 'Gagal menambah transaksi'),
        });
    }
  }

  delete(id: string): void {
    this.http.delete(`${this.api}/transactions/${id}`).subscribe({
      next:  () => { this.msg.success('Transaksi dihapus'); this.loadTransactions(); },
      error: e  => this.msg.error(e?.error?.message ?? 'Gagal menghapus transaksi'),
    });
  }

  // Pagination
  onPageChange(p: number): void { this.page.set(p); this.loadTransactions(); }
  onPageSizeChange(size: number): void { this.pageSize.set(size); this.page.set(1); this.loadTransactions(); }

  pagesToShow(): number[] {
    const total = Math.ceil(this.totalCount() / this.pageSize());
    const current = this.page();
    const pages: number[] = [];
    const start = Math.max(1, current - 2);
    const end   = Math.min(total, current + 2);
    for (let i = start; i <= end; i++) pages.push(i);
    return pages;
  }

  paginationLabel(): string {
    const from = (this.page() - 1) * this.pageSize() + 1;
    const to   = Math.min(this.page() * this.pageSize(), this.totalCount());
    return `Menampilkan ${from}–${to} dari ${this.totalCount()} transaksi`;
  }

  // Filter
  setFilter(type: string): void { this.filterType.set(type); this.page.set(1); this.loadTransactions(); }

  // Helpers
  onTypeChange(_: string): void { this.form.patchValue({ categoryId: '' }); }

  private enrich(tx: TransactionDto): TransactionRow {
    const account  = this.accounts().find(a => a.id === tx.accountId);
    const category = this.categories().find(c => c.id === tx.categoryId);
    return { ...tx, accountName: account?.name ?? '—', categoryName: category?.name ?? '—', categoryIcon: category?.icon ?? '' };
  }

  formatRupiah(amount: number): string {
    return new Intl.NumberFormat('id-ID', {
      style: 'currency', currency: 'IDR',
      minimumFractionDigits: 0, maximumFractionDigits: 0,
    }).format(amount);
  }

  formatDate(dateStr: string): string {
    return new Date(dateStr).toLocaleDateString('id-ID', { day: 'numeric', month: 'short', year: 'numeric' });
  }

  // getTypeLabel(type: string): string {
  //   return type;
  // }

  // getTypeClass(type: string): string {
  //   return ({ Income: 'tx-badge-income', Expense: 'tx-badge-expense', Transfer: 'tx-badge-transfer' } as any)[type] ?? '';
  // }

  getTypeLabel(type: string): string {
    // Kapitalisasi huruf pertama, sisa huruf kecil
    if (!type) return '';
    return type.charAt(0).toUpperCase() + type.slice(1).toLowerCase();
  }

  getTypeClass(type: string): string {
    // Normalisasi input ke huruf kecil agar cocok dengan kondisi
    const normalizedType = type?.toLowerCase();
    return ({ income: 'tx-badge-income', expense: 'tx-badge-expense', transfer: 'tx-badge-transfer' } as any)[normalizedType] ?? '';
  }

  getAmountClass(type: string): string {
    return type === 'Income' ? 'amount-income' : type === 'Expense' ? 'amount-expense' : 'amount-transfer';
  }

  getAmountPrefix(type: string): string {
    return type === 'Income' ? '+' : type === 'Expense' ? '-' : '↔';
  }
}