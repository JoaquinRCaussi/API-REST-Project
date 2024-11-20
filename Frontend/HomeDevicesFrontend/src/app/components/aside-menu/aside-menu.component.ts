import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { UserRole } from '../../../enum/UserRole';
import { AuthService } from '../../../backend/services/auth.service';

@Component({
  selector: 'app-aside-menu',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './aside-menu.component.html',
  styleUrl: './aside-menu.component.css'
})
export class AsideMenuComponent {
  UserRole = UserRole;
  activeButton: string = '';
  userRole: string | null = '';
  userId: string | null = '';
  isActiveSvg: boolean = false;

  constructor(private authService: AuthService, private router: Router) {}

  setActiveButton(button: string) {
    this.activeButton = button;
  }

  changeColor(): void {
    this.isActiveSvg = !this.isActiveSvg;
  }

  ngOnInit() {
    this.setActiveButton('main');
    this.userRole = localStorage.getItem('userRole');
    this.userId = localStorage.getItem('userId');
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
