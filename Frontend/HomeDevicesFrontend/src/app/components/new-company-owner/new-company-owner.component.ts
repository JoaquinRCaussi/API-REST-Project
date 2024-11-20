import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { CommonModule } from '@angular/common';
import { CompanyOwnerRequest } from '../../../backend/models/in/company-owner-request';
import { CompanyOwnerService } from '../../../backend/services/company-owner.service';
import { AlertComponent } from '../alert/alert.component';
import { AlertInterface } from '../../interface/alert';

@Component({
  selector: 'app-new-company-owner',
  standalone: true,
  imports: [DynamicFormComponent, AlertComponent],
  templateUrl: './new-company-owner.component.html',
  styleUrl: './new-company-owner.component.css'
})
export class NewCompanyOwnerComponent {

  alert: AlertInterface = {
    message: 'CompanyOwner created successfully',
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

  constructor(private companyOwnerService: CompanyOwnerService) { }

  submitHandler = (formData : any) => {

    const companyOwnerRequest : CompanyOwnerRequest = {
      name: formData.name,
      lastName: formData.lastName,
      email: formData.email,
      password: formData.password
    };

    this.companyOwnerService.createCompanyOwner(companyOwnerRequest).subscribe({
      next: (response) => {
        this.alert.message = 'CompanyOwner created successfully.';
        this.alert.type = 'success';
        this.alert.title = 'Success - CompanyOwner';
        this.openAlert();
      },
      error: (err) => {
        console.error('Error creating company owner:', err);
        this.alert.message = 'An error occurred while creating the company owner.';
        this.alert.type = 'error';
        this.alert.title = 'Error - CompanyOwner';
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
