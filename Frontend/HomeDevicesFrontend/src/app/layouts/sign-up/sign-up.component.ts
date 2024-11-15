import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HomeOwnerService } from '../../../backend/services/home-owner.service';
import { DynamicFormComponent } from "../../components/form/dynamic-form/dynamic-form.component";
import { DefaultButtonComponent } from "../../components/buttons/default-button/default-button.component";


@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [DynamicFormComponent, DefaultButtonComponent],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent {
  
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
        window.alert('Sign up successful');
        this.router.navigate(['login']);
      },
      (error) => {
        window.alert(error);
        console.error('Error in sign up:', error);
      }
    );

  }

  goToLogin = () => {
    this.router.navigate(['login']);
  }

}
