import { Routes } from '@angular/router';
import { LoginComponent } from './layouts/login/login.component';
import { ContentAreaComponent } from './components/content-area/content-area.component';
import { AuthGuard } from './guards/auth.guard';
import { SignUpComponent } from './layouts/sign-up/sign-up.component';
import { HomeDetailComponent } from './components/home-detail/home-detail.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignUpComponent},
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'main', component: ContentAreaComponent, data: { option: 'main-option' }, canActivate: [AuthGuard] },
  { path: 'homes', component: ContentAreaComponent, data: { option: 'homes-option' }, canActivate: [AuthGuard] },
  { path: 'new-home', component: ContentAreaComponent, data: { option: 'new-home-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id', component: ContentAreaComponent,  data: { option: 'home-detail-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id/members', component: ContentAreaComponent, data: { option: 'members-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id/members/:userId', component: ContentAreaComponent, data: { option: 'member-permissions-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id/new-member', component: ContentAreaComponent, data: { option: 'new-member-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id/rooms', component: ContentAreaComponent, data: { option: 'rooms-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id/new-room', component: ContentAreaComponent, data: { option: 'new-room-option' }, canActivate: [AuthGuard] },
  { path: 'companies', component: ContentAreaComponent, data: { option: 'companies-option' }, canActivate: [AuthGuard] },
];

