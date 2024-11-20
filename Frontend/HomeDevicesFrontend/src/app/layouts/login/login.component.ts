import { Component, output } from '@angular/core';
import { DynamicFormComponent } from '../../components/form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { LoginRequest } from '../../../backend/models/in/login-request';
import { AuthService } from '../../../backend/services/auth.service';
import { CommonModule } from '@angular/common';
import { DefaultButtonComponent } from '../../components/buttons/default-button/default-button.component';
import { Router } from '@angular/router';
import { AlertComponent } from '../../components/alert/alert.component';
import { AlertInterface } from '../../interface/alert';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [DynamicFormComponent, CommonModule, DefaultButtonComponent, AlertComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent {
  alert: AlertInterface = {
    message: 'Login successful',
    type: 'success',
    title: 'Success'
  };

  showAlert: boolean = false;

  constructor(private authService: AuthService, private router: Router) {}

  loginFields: FormField[] = [
    { type: 'email', name: 'email', label: 'Email', required: true },
    { type: 'password', name: 'password', label: 'Password', required: true }
  ];

  submitHandler = (formData: any) => {
    console.log('Form data:', formData);
  
    const loginRequest: LoginRequest = {
      email: formData.email,
      password: formData.password
    };
  
    this.authService.login(loginRequest).subscribe(
      (response) => {
        this.alert.message = 'Login successful';
        this.alert.type = 'success';
        this.alert.title = 'Welcome!';
        this.router.navigate(['main']);
      },
      (error) => {
        console.error('Error logging in', error);
        this.alert.message = 'Login failed';
        this.alert.type = 'error';
        this.alert.title = 'Error - Login';
        this.openAlert();
      }
    );
  }

  goToRegister = () => {
    this.router.navigate(['signup']);
  }

  closeAlert = () => {
    this.showAlert = false;
  }

  openAlert = () => {
    this.showAlert = true;
  }
}
