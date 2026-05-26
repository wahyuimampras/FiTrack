// src/app/features/dashboard/dashboard.component.ts
import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { catchError, of, forkJoin } from 'rxjs';

import { NzCardModule } from 'ng-zorro-antd/card';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzProgressModule, NzProgressStatusType } from 'ng-zorro-antd/progress';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzMessageService } from 'ng-zorro-antd/message';

import { DashboardService } from './dashboard.service';
import { AuthService } from '../../core/services/auth.service';
import {
  DashboardSummary, TransactionDto, AccountDto,
  BudgetDto, SavingGoalDto
} from '../../core/models/dashboard.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    DecimalPipe,
    NzCardModule,
    NzGridModule,
    NzSkeletonModule,
    NzTagModule,
    NzProgressModule,
    NzEmptyModule,
    NzToolTipModule,
    NzButtonModule,
    NzIconModule,
    NzDividerModule,
  ],
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit {
  private dashSvc = inject(DashboardService);
  private msg = inject(NzMessageService);
  readonly auth = inject(AuthService);

  // State
  loading = signal(true);
  summary = signal<DashboardSummary | null>(null);
  transactions = signal<TransactionDto[]>([]);
  accounts = signal<AccountDto[]>([]);
  budgets = signal<BudgetDto[]>([]);
  savingGoals = signal<SavingGoalDto[]>([]);

  // Date
  now = new Date();
  month = this.now.getMonth() + 1;
  year = this.now.getFullYear();
  monthLabel = this.now.toLocaleDateString('id-ID', { month: 'long', year: 'numeric' });

  // Computed
  netBalance = computed(() => {
    const s = this.summary();
    return s ? s.totalIncome - s.totalExpense : 0;
  });

  // Hitung dari semua akun aktif (tidak filter isActive karena
  // backend sudah return semua milik user)
  totalAccountBalance = computed(() =>
    this.accounts().reduce((sum, a) => sum + (a.balance ?? 0), 0)
  );

  savingsRate = computed(() => {
    const s = this.summary();
    if (!s || s.totalIncome === 0) return 0;
    return Math.round(((s.totalIncome - s.totalExpense) / s.totalIncome) * 100);
  });

  topExpenseCategories = computed(() => {
    const cats = this.summary()?.expenseByCategory ?? [];
    return [...cats].sort((a, b) => b.amount - a.amount).slice(0, 5);
  });

  activeSavingGoals = computed(() =>
    this.savingGoals().filter(g => !g.isCompleted).slice(0, 3)
  );

  activeBudgets = computed(() => this.budgets().slice(0, 4));

  activeAccounts = computed(() =>
    this.accounts().filter(a => a.isActive !== false)
  );

  ngOnInit(): void {
    this.loadAll();
  }

  loadAll(): void {
    this.loading.set(true);

    // Gunakan catchError per request agar satu gagal tidak blok semua
    const emptyDashboard: DashboardSummary = {
      totalIncome: 0, totalExpense: 0,
      totalActivities: 0, totalDistanceKm: 0, totalCaloriesBurned: 0,
      expenseByCategory: [], activityByType: []
    };

    forkJoin({
      summary:      this.dashSvc.getSummary(this.month, this.year).pipe(catchError(() => of(emptyDashboard))),
      transactions: this.dashSvc.getRecentTransactions(1, 6).pipe(catchError(() => of([]))),
      accounts:     this.dashSvc.getAccounts().pipe(catchError(() => of([]))),
      budgets:      this.dashSvc.getBudgets(this.month, this.year).pipe(catchError(() => of([]))),
      goals:        this.dashSvc.getSavingGoals().pipe(catchError(() => of([]))),
    }).subscribe({
      next: (data) => {
        this.summary.set(data.summary);

        // Handle paged result atau plain array
        const txList = Array.isArray(data.transactions)
          ? data.transactions
          : (data.transactions as any)?.items ?? [];
        this.transactions.set(txList);

        // Simpan semua accounts (aktif & tidak aktif)
        const accs = Array.isArray(data.accounts) ? data.accounts : [];
        this.accounts.set(accs);

        this.budgets.set(Array.isArray(data.budgets) ? data.budgets : []);
        this.savingGoals.set(Array.isArray(data.goals) ? data.goals : []);
        this.loading.set(false);
      },
      error: () => {
        this.msg.error('Gagal memuat dashboard');
        this.loading.set(false);
      }
    });
  }

  refresh(): void {
    this.loadAll();
  }

  // ── Helpers ───────────────────────────────────────
  formatRupiah(amount: number): string {
    if (amount === null || amount === undefined) return 'Rp 0';
    return new Intl.NumberFormat('id-ID', {
      style: 'currency', currency: 'IDR',
      minimumFractionDigits: 0, maximumFractionDigits: 0
    }).format(amount);
  }

  getBudgetPercent(budget: BudgetDto): number {
    if (!budget.spent || budget.amount === 0) return 0;
    return Math.min(Math.round((budget.spent / budget.amount) * 100), 100);
  }

  getBudgetStatus(budget: BudgetDto): NzProgressStatusType {
    const pct = this.getBudgetPercent(budget);
    if (pct >= 90) return 'exception';
    if (pct >= 75) return 'active';
    return 'success';
  }

  getBudgetColor(budget: BudgetDto): string {
    const pct = this.getBudgetPercent(budget);
    if (pct >= 90) return '#dc2626';
    if (pct >= 75) return '#f59e0b';
    return '#285A48';
  }

  getSavingPercent(goal: SavingGoalDto): number {
    if (goal.targetAmount === 0) return 0;
    return Math.min(Math.round((goal.currentAmount / goal.targetAmount) * 100), 100);
  }

  getAccountIcon(type: string): string {
    const icons: Record<string, string> = {
      bank: '🏦', cash: '💵', ewallet: '📱',
      investment: '📈', credit: '💳'
    };
    return icons[type?.toLowerCase()] ?? '💰';
  }

  getActivityIcon(type: string): string {
    const icons: Record<string, string> = {
      run: '🏃', ride: '🚴', swim: '🏊',
      walk: '🚶', hike: '🥾', workout: '💪'
    };
    return icons[type?.toLowerCase()] ?? '⚡';
  }

  getTxTypeLabel(type: string): string {
    const map: Record<string, string> = {
      Income: 'Pemasukan', Expense: 'Pengeluaran', Transfer: 'Transfer'
    };
    return map[type] ?? type;
  }

  formatDate(dateStr: string): string {
    return new Date(dateStr).toLocaleDateString('id-ID', {
      day: 'numeric', month: 'short'
    });
  }
}