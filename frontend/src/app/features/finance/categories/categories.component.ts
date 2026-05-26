// src/app/features/finance/categories/categories.component.ts
import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { finalize } from 'rxjs';

import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzBadgeModule } from 'ng-zorro-antd/badge';

import { environment } from '../../../../environments/environment';

export interface CategoryDto {
  id: string;
  name: string;
  type: string; // 'Income' | 'Expense'
  icon?: string;
  color?: string;
  isDefault: boolean;
}

// Emoji icon options
const EXPENSE_ICONS = ['🍔','🚗','🏠','💊','📚','👕','⚡','💧','📱','🎬','✈️','🏋️','🐾','🎁','💼','🛒','🔧','🎮'];
const INCOME_ICONS  = ['💰','💼','📈','🏦','🎯','🏆','💡','🤝','🎓','🏡','💻','🎤'];

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzButtonModule,
    NzModalModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    NzTagModule,
    NzPopconfirmModule,
    NzSkeletonModule,
    NzToolTipModule,
    NzEmptyModule,
    NzTabsModule,
    NzBadgeModule,
  ],
  templateUrl: './categories.component.html',
})
export class CategoriesComponent implements OnInit {
  private http   = inject(HttpClient);
  private fb     = inject(FormBuilder);
  private msg    = inject(NzMessageService);
  private api    = environment.apiUrl;

  // State
  loading       = signal(true);
  submitting    = signal(false);
  categories    = signal<CategoryDto[]>([]);
  showModal     = signal(false);
  editingCat    = signal<CategoryDto | null>(null);
  activeTab     = signal<'Expense' | 'Income'>('Expense');

  // Icon picker
  expenseIcons  = EXPENSE_ICONS;
  incomeIcons   = INCOME_ICONS;
  colorPresets  = ['#285A48','#059669','#dc2626','#d97706','#0284c7','#7c3aed','#db2777','#0891b2','#64748b','#ea580c'];

  // Form
  form = this.fb.nonNullable.group({
    name:  ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
    type:  ['Expense', [Validators.required]],
    icon:  [''],
    color: ['#285A48'],
  });

  // Computed
  expenseCategories = computed(() => this.categories().filter(c => c.type === 'Expense'));
  incomeCategories  = computed(() => this.categories().filter(c => c.type === 'Income'));
  isEditing         = computed(() => !!this.editingCat());
  modalTitle        = computed(() => this.isEditing() ? 'Edit Kategori' : 'Tambah Kategori Baru');

  currentIcons = computed(() =>
    this.form.get('type')?.value === 'Income' ? this.incomeIcons : this.expenseIcons
  );

  get selectedColor(): string { return this.form.get('color')?.value ?? '#285A48'; }
  get selectedIcon():  string { return this.form.get('icon')?.value  ?? ''; }

  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading.set(true);
    this.http.get<CategoryDto[]>(`${this.api}/categories`)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next:  data => this.categories.set(data),
        error: ()   => this.msg.error('Gagal memuat kategori'),
      });
  }

  // ── Modal ──────────────────────────────────────────
  openAdd(defaultType?: 'Income' | 'Expense'): void {
    this.editingCat.set(null);
    this.form.reset({
      name: '', type: defaultType ?? this.activeTab(), icon: '', color: '#285A48'
    });
    this.showModal.set(true);
  }

  openEdit(cat: CategoryDto): void {
    this.editingCat.set(cat);
    this.form.patchValue({
      name: cat.name, type: cat.type,
      icon: cat.icon ?? '', color: cat.color ?? '#285A48',
    });
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
    this.editingCat.set(null);
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
      const cat = this.editingCat()!;
      const payload = { id: cat.id, name: val.name, icon: val.icon || null, color: val.color || null };

      this.http.put(`${this.api}/categories/${cat.id}`, payload)
        .pipe(finalize(() => this.submitting.set(false)))
        .subscribe({
          next:  () => { this.msg.success('Kategori diperbarui'); this.closeModal(); this.load(); },
          error: e  => this.msg.error(e?.error?.message ?? 'Gagal memperbarui kategori'),
        });
    } else {
      const payload = { name: val.name, type: val.type, icon: val.icon || null, color: val.color || null };

      this.http.post(`${this.api}/categories`, payload)
        .pipe(finalize(() => this.submitting.set(false)))
        .subscribe({
          next:  () => { this.msg.success('Kategori ditambahkan'); this.closeModal(); this.load(); },
          error: e  => this.msg.error(e?.error?.message ?? 'Gagal menambah kategori'),
        });
    }
  }

  delete(id: string): void {
    this.http.delete(`${this.api}/categories/${id}`).subscribe({
      next:  () => { this.msg.success('Kategori dihapus'); this.load(); },
      error: e  => this.msg.error(e?.error?.message ?? 'Gagal menghapus kategori'),
    });
  }

  // ── Helpers ────────────────────────────────────────
  selectIcon(icon: string):   void { this.form.patchValue({ icon }); }
  selectColor(color: string): void { this.form.patchValue({ color }); }

  setTab(tab: 'Expense' | 'Income'): void { this.activeTab.set(tab); }

  getTypeLabel(type: string): string { return type === 'Income' ? 'Pemasukan' : 'Pengeluaran'; }
  getTypeColor(type: string): string { return type === 'Income' ? '#059669' : '#dc2626'; }
}