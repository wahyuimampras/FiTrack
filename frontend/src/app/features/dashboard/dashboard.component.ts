// src/app/features/dashboard/dashboard.component.ts
import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

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
    CurrencyPipe,
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

  // Computed values
  netBalance = computed(() => {
    const s = this.summary();
    return s ? s.totalIncome - s.totalExpense : 0;
  });

  totalAccountBalance = computed(() =>
    this.accounts().reduce((sum, a) => sum + a.balance, 0)
  );

  savingsRate = computed(() => {
    const s = this.summary();
    if (!s || s.totalIncome === 0) return 0;
    return Math.round(((s.totalIncome - s.totalExpense) / s.totalIncome) * 100);
  });

  topExpenseCategories = computed(() =>
    this.summary()?.expenseByCategory
      .sort((a, b) => b.amount - a.amount)
      .slice(0, 5) ?? []
  );

  activeSavingGoals = computed(() =>
    this.savingGoals().filter(g => !g.isCompleted).slice(0, 3)
  );

  activeBudgets = computed(() =>
    this.budgets().slice(0, 4)
  );

  ngOnInit(): void {
    this.loadAll();
  }

  loadAll(): void {
    this.loading.set(true);

    forkJoin({
      summary: this.dashSvc.getSummary(this.month, this.year),
      transactions: this.dashSvc.getRecentTransactions(1, 6),
      accounts: this.dashSvc.getAccounts(),
      budgets: this.dashSvc.getBudgets(this.month, this.year),
      goals: this.dashSvc.getSavingGoals(),
    }).subscribe({
      next: (data) => {
        this.summary.set(data.summary);
        // Handle paged result or plain array
        const txList = Array.isArray(data.transactions)
          ? data.transactions
          : data.transactions?.items ?? [];
        this.transactions.set(txList);
        this.accounts.set(data.accounts.filter(a => a.isActive));
        this.budgets.set(data.budgets);
        this.savingGoals.set(data.goals);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  refresh(): void {
    this.loadAll();
  }

  // Helpers
  formatRupiah(amount: number): string {
    return new Intl.NumberFormat('id-ID', {
      style: 'currency',
      currency: 'IDR',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(amount);
  }

  formatPace(pace: number): string {
    const min = Math.floor(pace);
    const sec = Math.round((pace - min) * 60);
    return `${min}:${sec.toString().padStart(2, '0')}/km`;
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