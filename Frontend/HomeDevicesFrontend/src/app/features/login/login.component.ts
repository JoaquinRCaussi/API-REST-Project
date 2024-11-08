import { Component } from '@angular/core';
import { DynamicFormComponent } from '../../components/form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { LoginRequest } from '../../interface/login-request';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [DynamicFormComponent, CommonModule],
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
        console.log('Login exitoso:', response);
        this.router.navigate(['main']);
      },
      (error) => {
        console.error('Error en login:', error);
      }
    );
  }
}
