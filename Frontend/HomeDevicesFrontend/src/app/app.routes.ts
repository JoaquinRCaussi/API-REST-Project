import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';
import { ContentAreaComponent } from './features/content-area/content-area.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'main', component: ContentAreaComponent, data: { option: 'main-option' }, canActivate: [AuthGuard] },
  { path: 'homes', component: ContentAreaComponent, data: { option: 'homes-option' }, canActivate: [AuthGuard] },
  { path: 'add-home', component: ContentAreaComponent, data: { option: 'add-home-option' }, canActivate: [AuthGuard] },
];

