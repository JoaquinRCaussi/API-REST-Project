import { Component } from '@angular/core';
import { DynamicFormComponent } from '../../components/form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { LoginRequest } from '../../../backend/models/login-request';
import { AuthService } from '../../../backend/services/auth.service';
import { CommonModule } from '@angular/common';
import { DefaultButtonComponent } from '../../components/buttons/default-button/default-button.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [DynamicFormComponent, CommonModule, DefaultButtonComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

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
        window.alert('Login successful');
        console.log('Login exitoso:', response);
        this.router.navigate(['main']);
      },
      (error) => {
        window.alert('Error in login');
        console.error('Error in login:', error);
      }
    );
  }

  goToRegister = () => {
    this.router.navigate(['signup']);
  }
}
