import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UserRole } from '../../../enum/UserRole';

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
  userRole: string | null = '' ;

  setActiveButton(button: string) {
    this.activeButton = button;
  }

  isActiveSvg: boolean = false;

  changeColor(): void {
    this.isActiveSvg = !this.isActiveSvg;
  }

  ngOnInit() {
    this.setActiveButton('main');
    this.userRole = localStorage.getItem('userRole');
  }
}
