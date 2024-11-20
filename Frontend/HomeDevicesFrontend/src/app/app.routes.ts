import { Routes } from '@angular/router';
import { LoginComponent } from './layouts/login/login.component';
import { ContentAreaComponent } from './components/content-area/content-area.component';
import { AuthGuard } from './guards/auth.guard';
import { SignUpComponent } from './layouts/sign-up/sign-up.component';
import { HomeDetailComponent } from './components/home-detail/home-detail.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignUpComponent},
  { path: 'logout', redirectTo: 'login', pathMatch: 'full' },
  { path: 'main', component: ContentAreaComponent, data: { option: 'main-option' }, canActivate: [AuthGuard] },
  { path: 'homes', component: ContentAreaComponent, data: { option: 'homes-option' }, canActivate: [AuthGuard] },
  { path: 'new-home', component: ContentAreaComponent, data: { option: 'new-home-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id', component: ContentAreaComponent,  data: { option: 'home-detail-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id/config', component: ContentAreaComponent, data: { option: 'home-config-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id/members', component: ContentAreaComponent, data: { option: 'members-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id/members/:userId', component: ContentAreaComponent, data: { option: 'member-permissions-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id/new-member', component: ContentAreaComponent, data: { option: 'new-member-option' }, canActivate: [AuthGuard] },
  { path: 'new-admin', component: ContentAreaComponent, data: { option: 'new-admin-option' }, canActivate: [AuthGuard] },
  { path: 'new-company-owner', component: ContentAreaComponent, data: { option: 'new-company-owner-option' }, canActivate: [AuthGuard] },
  { path: 'users', component: ContentAreaComponent, data: { option: 'users-option' }, canActivate: [AuthGuard] },
  { path: 'users/:userId/notifications', component: ContentAreaComponent, data: { option: 'user-notifications-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id/rooms', component: ContentAreaComponent, data: { option: 'rooms-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id/new-room', component: ContentAreaComponent, data: { option: 'new-room-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:homeId/devices', component: ContentAreaComponent, data: { option: 'home-devices-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:homeId/new-home-device', component: ContentAreaComponent, data: { option: 'new-home-device-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:homeId/rooms/:roomId', component: ContentAreaComponent, data: { option: 'room-detail-option' }, canActivate: [AuthGuard] },
  { path: 'companies', component: ContentAreaComponent, data: { option: 'companies-option' }, canActivate: [AuthGuard] },
  { path: 'companies-list', component: ContentAreaComponent, data: { option: 'companies-list-option' }, canActivate: [AuthGuard] },
  { path: 'new-company', component: ContentAreaComponent, data: { option: 'new-company-option' }, canActivate: [AuthGuard] },
  { path: 'companies/:companyName/devices', component: ContentAreaComponent, data: { option: 'company-devices-option' }, canActivate: [AuthGuard] },
  { path: 'companies/:companyName/new-device', component: ContentAreaComponent, data: { option: 'new-device-option' }, canActivate: [AuthGuard] }
];

