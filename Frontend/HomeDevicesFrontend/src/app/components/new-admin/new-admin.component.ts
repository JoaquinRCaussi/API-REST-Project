import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { CommonModule } from '@angular/common';
import { AdminRequest } from '../../../backend/models/in/admin-request';
import { AdminService } from '../../../backend/services/admin.service';
import { AlertComponent } from '../alert/alert.component';
import { AlertInterface } from '../../interface/alert';

@Component({
  selector: 'app-new-admin',
  standalone: true,
  imports: [DynamicFormComponent, AlertComponent],
  templateUrl: './new-admin.component.html',
  styleUrl: './new-admin.component.css'
})
export class NewAdminComponent {

  alert: AlertInterface = {
    message: 'Admin created successfully',
    type: 'success',
    title: 'Success'
  };

  showAlert : boolean = false;

  fields : FormField[] = [
    {
      type: 'text',
      name: 'name',
      label: 'Name',
      placeholder: 'Enter your name',
      required: true
    },
    {
      type: 'text',
      name: 'lastName',
      label: 'Last name',
      placeholder: 'Enter your last name',
      required: true
    },
    {
      type: 'text',
      name: 'email',
      label: 'Email',
      placeholder: 'Enter your email',
      required: true
    },
    {
      type: 'text',
      name: 'password',
      label: 'Password',
      placeholder: 'Enter your password',
      required: true
    }
  ];

  constructor(private adminsService: AdminService) { }

  submitHandler = (formData : any) => {

    const adminRequest : AdminRequest = {
      name: formData.name,
      lastName: formData.lastName,
      email: formData.email,
      password: formData.password
    };

    this.adminsService.createAdmin(adminRequest).subscribe({
      next: (response) => {
        this.alert.message = 'Admin created successfully.';
        this.alert.type = 'success';
        this.alert.title = 'Success - Admin';
        this.openAlert();
      },
      error: (err) => {
        console.error('Error creating admin:', err);
        this.alert.message = 'An error occurred while creating the admin.';
        this.alert.type = 'error';
        this.alert.title = 'Error - Admin';
        this.openAlert();
      }
    });
  }

  openAlert = () => {
    this.showAlert = true;
  }

  closeAlert = () => {
    this.showAlert = false;
  }
}
