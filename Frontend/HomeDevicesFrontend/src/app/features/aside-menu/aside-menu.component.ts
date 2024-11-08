import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-aside-menu',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './aside-menu.component.html',
  styleUrl: './aside-menu.component.css'
})
export class AsideMenuComponent {
  activeButton: string = '';

  setActiveButton(button: string) {
    this.activeButton = button;
  }

  isActiveSvg: boolean = false;

  changeColor(): void {
    this.isActiveSvg = !this.isActiveSvg;
  }
}
