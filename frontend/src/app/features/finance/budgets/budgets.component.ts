// src/app/features/finance/budgets/budgets.component.ts
import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { finalize, forkJoin, catchError, of } from 'rxjs';

import { NzButtonModule }      from 'ng-zorro-antd/button';
import { NzModalModule }       from 'ng-zorro-antd/modal';
import { NzFormModule }        from 'ng-zorro-antd/form';
import { NzInputModule }       from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzSelectModule }      from 'ng-zorro-antd/select';
import { NzProgressModule, NzProgressStatusType } from 'ng-zorro-antd/progress';
import { NzTagModule }         from 'ng-zorro-antd/tag';
import { NzPopconfirmModule }  from 'ng-zorro-antd/popconfirm';
import { NzMessageService }    from 'ng-zorro-antd/message';
import { NzSkeletonModule }    from 'ng-zorro-antd/skeleton';
import { NzEmptyModule }       from 'ng-zorro-antd/empty';
import { NzToolTipModule }     from 'ng-zorro-antd/tooltip';
import { NzIconModule }        from 'ng-zorro-antd/icon';

import { environment } from '../../../../environments/environment';

// ── Models ─────────────────────────────────────────
export interface BudgetDto {
  id: string;
  categoryId: string;
  categoryName: string;
  month: number;
  year: number;
  amount: number;
}

export interface CategoryDto {
  id: string;
  name: string;
  type: string; // 'Income' | 'Expense'
  icon?: string;
  color?: string;
}

export interface TransactionDto {
  id: string;
  categoryId: string;
  type: string;
  amount: number;
  date: string;
}

// Budget diperkaya dengan spent & persentase
export interface BudgetRow extends BudgetDto {
  spent: number;
  remaining: number;
  percentage: number;
  categoryIcon?: string;
  categoryColor?: string;
}

@Component({
  selector: 'app-budgets',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzButtonModule,
    NzModalModule,
    NzFormModule,
    NzInputModule,
    NzInputNumberModule,
    NzSelectModule,
    NzProgressModule,
    NzTagModule,
    NzPopconfirmModule,
    NzSkeletonModule,
    NzEmptyModule,
    NzToolTipModule,
    NzIconModule,
  ],
  templateUrl: './budgets.component.html',
})
export class BudgetsComponent implements OnInit {
  private http = inject(HttpClient);
  private fb   = inject(FormBuilder);
  private msg  = inject(NzMessageService);
  private api  = environment.apiUrl;

  // ── State ──────────────────────────────────────
  loading    = signal(true);
  submitting = signal(false);
  budgets    = signal<BudgetRow[]>([]);
  categories = signal<CategoryDto[]>([]);
  transactions = signal<TransactionDto[]>([]);
  showModal  = signal(false);
  editingBudget = signal<BudgetRow | null>(null);

  // Period navigator
  selectedMonth = signal(new Date().getMonth() + 1);
  selectedYear  = signal(new Date().getFullYear());

  // Form
  form = this.fb.nonNullable.group({
    categoryId: ['', [Validators.required]],
    amount:     [0,  [Validators.required, Validators.min(1000)]],
    month:      [new Date().getMonth() + 1, [Validators.required]],
    year:       [new Date().getFullYear(),   [Validators.required]],
  });

  // ── Computed ───────────────────────────────────
  isEditing  = computed(() => !!this.editingBudget());
  modalTitle = computed(() => this.isEditing() ? 'Edit Budget' : 'Set Budget Baru');

  // Hanya kategori Expense yang bisa di-budget
  expenseCategories = computed(() =>
    this.categories().filter(c => c.type === 'Expense')
  );

  // Kategori yang belum punya budget di period ini
  availableCategories = computed(() => {
    const usedIds = this.budgets()
      .filter(b => !this.editingBudget() || b.id !== this.editingBudget()!.id)
      .map(b => b.categoryId);
    return this.expenseCategories().filter(c => !usedIds.includes(c.id));
  });

  totalBudget  = computed(() => this.budgets().reduce((s, b) => s + b.amount, 0));
  totalSpent   = computed(() => this.budgets().reduce((s, b) => s + b.spent, 0));
  totalRemaining = computed(() => this.totalBudget() - this.totalSpent());
  overallPct   = computed(() => this.totalBudget() > 0
    ? Math.min(Math.round((this.totalSpent() / this.totalBudget()) * 100), 100)
    : 0
  );

  budgetsOnTrack  = computed(() => this.budgets().filter(b => b.percentage < 75).length);
  budgetsWarning  = computed(() => this.budgets().filter(b => b.percentage >= 75 && b.percentage < 90).length);
  budgetsDanger   = computed(() => this.budgets().filter(b => b.percentage >= 90).length);

  periodLabel = computed(() => {
    const d = new Date(this.selectedYear(), this.selectedMonth() - 1, 1);
    return d.toLocaleDateString('id-ID', { month: 'long', year: 'numeric' });
  });

  months = [
    { v: 1, l: 'Januari' }, { v: 2,  l: 'Februari' }, { v: 3,  l: 'Maret' },
    { v: 4, l: 'April'   }, { v: 5,  l: 'Mei'      }, { v: 6,  l: 'Juni'  },
    { v: 7, l: 'Juli'    }, { v: 8,  l: 'Agustus'  }, { v: 9,  l: 'September' },
    { v: 10, l: 'Oktober' }, { v: 11, l: 'November' }, { v: 12, l: 'Desember' },
  ];

  years = Array.from({ length: 5 }, (_, i) => new Date().getFullYear() - 2 + i);

  ngOnInit(): void {
    this.loadAll();
  }

  loadAll(): void {
    this.loading.set(true);

    // Load categories + transactions sekaligus, baru load budgets
    forkJoin({
      categories: this.http.get<CategoryDto[]>(`${this.api}/categories`).pipe(catchError(() => of([]))),
      transactions: this.loadTransactionsForPeriod(),
    }).subscribe(({ categories, transactions }) => {
      this.categories.set(categories);
      this.transactions.set(transactions);
      this.loadBudgets();
    });
  }

  private loadTransactionsForPeriod() {
    // Ambil transaksi bulan ini untuk hitung spent
    const params = new HttpParams()
      .set('page', 1)
      .set('pageSize', 500); // ambil banyak untuk kalkulasi
    return this.http.get<any>(`${this.api}/transactions`, { params }).pipe(
      catchError(() => of({ items: [] }))
    );
  }

  loadBudgets(): void {
    const params = new HttpParams()
      .set('month', this.selectedMonth())
      .set('year', this.selectedYear());

    this.http.get<BudgetDto[]>(`${this.api}/budget`, { params })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: data => {
          const rows = this.enrichBudgets(data);
          this.budgets.set(rows);
        },
        error: () => this.msg.error('Gagal memuat data budget'),
      });
  }

  // Enrich budget dengan spent dari transaksi
  private enrichBudgets(budgets: BudgetDto[]): BudgetRow[] {
    const txs = Array.isArray(this.transactions())
      ? this.transactions()
      : (this.transactions() as any)?.items ?? [];

    // Filter transaksi bulan & tahun yang dipilih
    const periodTxs = txs.filter((tx: TransactionDto) => {
      const d = new Date(tx.date);
      return d.getMonth() + 1 === this.selectedMonth()
        && d.getFullYear() === this.selectedYear()
        && tx.type === 'Expense';
    });

    return budgets.map(budget => {
      const cat = this.categories().find(c => c.id === budget.categoryId);
      const spent = periodTxs
        .filter((tx: TransactionDto) => tx.categoryId === budget.categoryId)
        .reduce((s: number, tx: TransactionDto) => s + tx.amount, 0);

      const percentage = budget.amount > 0
        ? Math.min(Math.round((spent / budget.amount) * 100), 100)
        : 0;

      return {
        ...budget,
        spent,
        remaining: budget.amount - spent,
        percentage,
        categoryIcon:  cat?.icon  ?? '',
        categoryColor: cat?.color ?? '#285A48',
      };
    }).sort((a, b) => b.percentage - a.percentage); // sort: paling kritis di atas
  }

  // ── Period navigation ──────────────────────────
  prevMonth(): void {
    if (this.selectedMonth() === 1) {
      this.selectedMonth.set(12);
      this.selectedYear.update(y => y - 1);
    } else {
      this.selectedMonth.update(m => m - 1);
    }
    this.reloadBudgets();
  }

  nextMonth(): void {
    if (this.selectedMonth() === 12) {
      this.selectedMonth.set(1);
      this.selectedYear.update(y => y + 1);
    } else {
      this.selectedMonth.update(m => m + 1);
    }
    this.reloadBudgets();
  }

  reloadBudgets(): void {
    this.loading.set(true);
    forkJoin({
      transactions: this.loadTransactionsForPeriod(),
    }).subscribe(({ transactions }) => {
      this.transactions.set(transactions);
      this.loadBudgets();
    });
  }

  // ── Modal ──────────────────────────────────────
  openAdd(): void {
    this.editingBudget.set(null);
    this.form.reset({
      categoryId: '',
      amount: 0,
      month: this.selectedMonth(),
      year: this.selectedYear(),
    });
    this.showModal.set(true);
  }

  openEdit(budget: BudgetRow): void {
    this.editingBudget.set(budget);
    this.form.patchValue({
      categoryId: budget.categoryId,
      amount:     budget.amount,
      month:      budget.month,
      year:       budget.year,
    });
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
    this.editingBudget.set(null);
    this.form.reset();
  }

  submit(): void {
    if (this.form.invalid) {
      Object.values(this.form.controls).forEach(c => { c.markAsDirty(); c.updateValueAndValidity(); });
      return;
    }

    this.submitting.set(true);
    const val = this.form.getRawValue();

    if (this.isEditing()) {
      const budget = this.editingBudget()!;
      const payload = {
        id:         budget.id,
        categoryId: val.categoryId,
        amount:     val.amount,
        month:      val.month,
        year:       val.year,
      };

      this.http.put(`${this.api}/budget/${budget.id}`, payload)
        .pipe(finalize(() => this.submitting.set(false)))
        .subscribe({
          next:  () => { this.msg.success('Budget diperbarui'); this.closeModal(); this.reloadBudgets(); },
          error: e  => this.msg.error(e?.error?.message ?? 'Gagal memperbarui budget'),
        });
    } else {
      const payload = {
        categoryId: val.categoryId,
        amount:     val.amount,
        month:      val.month,
        year:       val.year,
      };

      this.http.post(`${this.api}/budget`, payload)
        .pipe(finalize(() => this.submitting.set(false)))
        .subscribe({
          next:  () => { this.msg.success('Budget berhasil ditambahkan'); this.closeModal(); this.reloadBudgets(); },
          error: e  => this.msg.error(e?.error?.message ?? 'Gagal menambah budget'),
        });
    }
  }

  delete(id: string): void {
    this.http.delete(`${this.api}/budget/${id}`).subscribe({
      next:  () => { this.msg.success('Budget dihapus'); this.reloadBudgets(); },
      error: e  => this.msg.error(e?.error?.message ?? 'Gagal menghapus budget'),
    });
  }

  // ── Helpers ────────────────────────────────────
  getProgressStatus(budget: BudgetRow): NzProgressStatusType {
    if (budget.percentage >= 100) return 'exception';
    if (budget.percentage >= 75)  return 'active';
    return 'success';
  }

  getProgressColor(budget: BudgetRow): string {
    if (budget.percentage >= 100) return '#dc2626';
    if (budget.percentage >= 90)  return '#dc2626';
    if (budget.percentage >= 75)  return '#f59e0b';
    return budget.categoryColor ?? '#285A48';
  }

  getStatusLabel(budget: BudgetRow): string {
    if (budget.percentage >= 100) return 'Melebihi budget!';
    if (budget.percentage >= 90)  return 'Hampir habis';
    if (budget.percentage >= 75)  return 'Perlu perhatian';
    return 'Aman';
  }

  getStatusClass(budget: BudgetRow): string {
    if (budget.percentage >= 100) return 'status-danger';
    if (budget.percentage >= 90)  return 'status-danger';
    if (budget.percentage >= 75)  return 'status-warning';
    return 'status-safe';
  }

  formatRupiah(amount: number): string {
    return new Intl.NumberFormat('id-ID', {
      style: 'currency', currency: 'IDR',
      minimumFractionDigits: 0, maximumFractionDigits: 0,
    }).format(amount ?? 0);
  }

  isCurrentMonth(): boolean {
    const now = new Date();
    return this.selectedMonth() === now.getMonth() + 1
      && this.selectedYear() === now.getFullYear();
  }
}