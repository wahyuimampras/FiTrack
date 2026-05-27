import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { LayoutService } from '../../services/layout.service';
import { AuthService } from '../../../core/services/auth.service';

interface NavItem {
  label: string;
  icon: string;
  route?: string;
  badge?: { text: string; type: 'success' | 'warning' | 'strava' };
  children?: NavItem[];
  isExpanded?: boolean;
}

interface NavGroup {
  label: string;
  items: NavItem[];
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent {
  layoutService = inject(LayoutService);
  authService = inject(AuthService);

  navGroups: NavGroup[] = [
    {
      label: 'GENERAL',
      items: [
        { label: 'Dashboard',    icon: '📊', route: '/dashboard' },
        { label: 'Accounts',     icon: ' 💳', route: '/accounts' },
        { label: 'Transactions', icon: '💸', route: '/transactions' },
      ]
    },
    {
      label: 'CORE',
      items: [
        {
          label: 'Finance',
          icon: '💰',
          isExpanded: false,
          children: [
            { label: 'Categories',      icon: '🗂️', route: '/categories' },
            { label: 'Budgets',         icon: '📋', route: '/budgets' },
            { label: 'Saving Goals',    icon: '🎯', route: '/saving-goals' },
          ]
        },
        {
          label: 'Workout',
          icon: '🏋️',
          isExpanded: false,
          children: [
            { label: 'Activities',      icon: '🏃', route: '/activities' },
            { label: 'Stats',           icon: '📈', route: '/workout/stats' },
            { label: 'Strava Sync',     icon: '⚡', route: '/strava', badge: { text: 'Connect', type: 'strava' } },
            { label: 'Training Plans',  icon: '🗓️', route: '/trainings' }
          ]
        }
      ]
    },
    {
      label: 'INSIGHTS',
      items: [
        { label: 'Reports', icon: '📑', route: '/reports' },
      ]
    },
  ];

  toggleExpand(item: NavItem) {
    if (item.children) {
      item.isExpanded = !item.isExpanded;
    }
  }
}