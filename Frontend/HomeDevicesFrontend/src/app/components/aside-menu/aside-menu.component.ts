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
  userId: string | null = '';
  
  isActiveSvg: boolean = false;

  constructor() {
  }

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
}
