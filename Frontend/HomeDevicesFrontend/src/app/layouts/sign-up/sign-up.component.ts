import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HomeOwnerService } from '../../../backend/services/home-owner.service';
import { DynamicFormComponent } from "../../components/form/dynamic-form/dynamic-form.component";
import { DefaultButtonComponent } from "../../components/buttons/default-button/default-button.component";
import { AlertComponent } from '../../components/alert/alert.component';
import { AlertInterface } from '../../interface/alert';


@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [DynamicFormComponent, DefaultButtonComponent, AlertComponent],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent {

  alert: AlertInterface = {
    message: 'Sign up successful',
    type: 'success',
    title: 'Success'
  };

  showAlert: boolean = false;
  
  constructor(private homeOwnerService: HomeOwnerService,private router: Router) {}

  signUpFields = [
    { type: 'text',placeholder: "Name" ,name: 'name', label: 'Name', required: true },
    { type: 'text', placeholder: "Last Name",name: 'last-name', label: 'Last Name', required: true },
    { type: 'email',  placeholder: "helloWorld@gmail.com",name: 'email', label: 'Email', required: true },
    { type: 'text', name: 'image-path', label: 'Image Path', required: true },
    { type: 'password', name: 'password', label: 'Password', required: true }
  ];

  submitHandler = (formData: any) => {
    console.log('Form data:', formData);

    const signUpRequest = {
      name: formData.name,
      lastName: formData['last-name'],
      email: formData.email,
      imagePath: formData['image-path'],
      password: formData.password
    };

    this.homeOwnerService.signup(signUpRequest).subscribe(
      (response) => {
        console.log('Sign up exitoso:', response);
        this.alert.message = 'Sign up successful';
        this.alert.type = 'success';
        this.alert.title = 'Success - Sign up';
        this.openAlert();
      },
      (error) => {
        this.alert.message = 'Sign up failed';
        this.alert.type = 'error';
        this.alert.title = 'Error - Sign up';
        this.openAlert();
        console.error('Error in sign up:', error);
      }
    );

  }

  goToLogin = () => {
    this.router.navigate(['login']);
  }

  closeAlert = () => {
    if(this.alert.type === 'success') {
      this.showAlert = false;
      this.router.navigate(['login']);
    }
    this.showAlert = false;
  }

  openAlert = () => {
    this.showAlert = true;
  }

}
