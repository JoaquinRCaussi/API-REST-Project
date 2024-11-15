import { Routes } from '@angular/router';
import { LoginComponent } from './layouts/login/login.component';
import { ContentAreaComponent } from './components/content-area/content-area.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignUpComponent},
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'main', component: ContentAreaComponent, data: { option: 'main-option' }, canActivate: [AuthGuard] },
  { path: 'homes', component: ContentAreaComponent, data: { option: 'homes-option' }, canActivate: [AuthGuard] },
  { path: 'add-home', component: ContentAreaComponent, data: { option: 'add-home-option' }, canActivate: [AuthGuard] },
  { path: 'homes/:id', component: HomeDetailComponent,  data: { option: 'home-detail-option' }, canActivate: [AuthGuard] }
];

