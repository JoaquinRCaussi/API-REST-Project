import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AsideMenuComponent } from "./features/aside-menu/aside-menu.component";
import { ContentAreaComponent } from './features/content-area/content-area.component';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AsideMenuComponent, ContentAreaComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'HomeDevicesFrontend';
  isLoginRoute = false;

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event) => {
      this.isLoginRoute = (event as NavigationEnd).urlAfterRedirects === '/login';
    });
  }
}
