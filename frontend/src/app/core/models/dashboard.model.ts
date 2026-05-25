// src/app/core/models/dashboard.model.ts

export interface CategoryExpense {
  category: string;
  amount: number;
}

export interface ActivitySummary {
  type: string;
  count: number;
}

export interface DashboardSummary {
  totalIncome: number;
  totalExpense: number;
  totalActivities: number;
  totalDistanceKm: number;
  totalCaloriesBurned: number;
  expenseByCategory: CategoryExpense[];
  activityByType: ActivitySummary[];
}

export interface TransactionDto {
  id: string;
  accountId: string;
  categoryId: string;
  type: 'Income' | 'Expense' | 'Transfer';
  amount: number;
  description: string;
  date: string;
  notes?: string;
  createdAt: string;
  // enriched fields (joined di frontend dari accounts/categories)
  accountName?: string;
  categoryName?: string;
}

export interface AccountDto {
  id: string;
  name: string;
  type: string;
  balance: number;
  color?: string;
  icon?: string;
  isActive: boolean;
}

export interface BudgetDto {
  id: string;
  categoryId: string;
  categoryName: string;
  month: number;
  year: number;
  amount: number;
  spent?: number; // dihitung di frontend dari transaksi
}

export interface SavingGoalDto {
  id: string;
  name: string;
  targetAmount: number;
  currentAmount: number;
  targetDate?: string;
  isCompleted: boolean;
}