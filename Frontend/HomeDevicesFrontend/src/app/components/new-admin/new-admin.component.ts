import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { CommonModule } from '@angular/common';
import { AdminRequest } from '../../../backend/models/in/admin-request';
import { AdminService } from '../../../backend/services/admin.service';

@Component({
  selector: 'app-new-admin',
  standalone: true,
  imports: [DynamicFormComponent],
  templateUrl: './new-admin.component.html',
  styleUrl: './new-admin.component.css'
})
export class NewAdminComponent {

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
      next: (response) => window.alert("Admin " + adminRequest.name + " created successfully"),
      error: (err) => window.alert("Error creating admin")
    });
  }
}
