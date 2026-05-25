// src/app/features/dashboard/dashboard.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DashboardSummary, TransactionDto, AccountDto, BudgetDto, SavingGoalDto } from '../../core/models/dashboard.model';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private http = inject(HttpClient);
  private api = environment.apiUrl;

  getSummary(month: number, year: number): Observable<DashboardSummary> {
    const params = new HttpParams()
      .set('month', month)
      .set('year', year);
    return this.http.get<DashboardSummary>(`${this.api}/dashboard/summary`, { params });
  }

  getRecentTransactions(page = 1, pageSize = 5): Observable<any> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<any>(`${this.api}/transactions`, { params });
  }

  getAccounts(): Observable<AccountDto[]> {
    return this.http.get<AccountDto[]>(`${this.api}/account`);
  }

  getBudgets(month: number, year: number): Observable<BudgetDto[]> {
    const params = new HttpParams()
      .set('month', month)
      .set('year', year);
    return this.http.get<BudgetDto[]>(`${this.api}/budget`, { params });
  }

  getSavingGoals(): Observable<SavingGoalDto[]> {
    return this.http.get<SavingGoalDto[]>(`${this.api}/savinggoal`);
  }
}