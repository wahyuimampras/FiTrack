import { Component, inject, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LayoutService } from '../../services/layout.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './topbar.component.html',
})
export class TopbarComponent {
  layoutService = inject(LayoutService);
  router = inject(Router);
  authService = inject(AuthService);
  
  today = new Date().toLocaleDateString('id-ID', {
    day: '2-digit', month: 'short', year: 'numeric'
  });

  isDropdownOpen = false;
  isNotificationOpen = false;

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
    if (this.isDropdownOpen) this.isNotificationOpen = false;
  }

  toggleNotification() {
    this.isNotificationOpen = !this.isNotificationOpen;
    if (this.isNotificationOpen) this.isDropdownOpen = false;
  }

  @HostListener('document:click', ['$event'])
  closeDropdown(event: Event) {
    const target = event.target as HTMLElement;
    if (!target.closest('.avatar-container')) {
      this.isDropdownOpen = false;
    }
    if (!target.closest('.notification-container')) {
      this.isNotificationOpen = false;
    }
  }

  goToSettings() {
    this.isDropdownOpen = false;
    this.router.navigate(['/settings']);
  }

  signOut() {
    this.isDropdownOpen = false;
    this.authService.logout();
  }
}