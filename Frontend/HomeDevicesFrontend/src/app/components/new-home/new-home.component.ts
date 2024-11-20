import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { CommonModule } from '@angular/common';
import { HomesService } from '../../../backend/services/homes.service';
import { HomeRequest } from '../../../backend/models/in/home-request';
import { AlertComponent } from '../alert/alert.component';
import { AlertInterface } from '../../interface/alert';

@Component({
  selector: 'app-new-home',
  standalone: true,
  imports: [DynamicFormComponent, AlertComponent],
  templateUrl: './new-home.component.html',
  styleUrl: './new-home.component.css'
})
export class NewHomeComponent {

  alert: AlertInterface = {
    message: 'Home created successfully',
    type: 'success',
    title: 'Success'
  };
  
  showAlert : boolean = false;

  fields : FormField[] = [
    {
      type: 'text',
      name: 'name',
      label: 'Name',
      placeholder: 'Enter your home name',
      required: true
    },
    {
      type: 'text',
      name: 'location',
      label: 'Location',
      placeholder: 'Enter your home location',
      required: true
    },
    {
      type: 'text',
      name: 'latitude',
      label: 'Latitude',
      placeholder: 'Enter your home latitude',
      required: true
    },
    {
      type: 'text',
      name: 'longitude',
      label: 'Longitude',
      placeholder: 'Enter your home longitude',
      required: true
    },
    {
      type: 'number',
      name: 'membercount',
      label: 'Member Count',
      placeholder: 'Enter how many members are going to be in the home',
      required: true
    }
  ];

  constructor(private homesService: HomesService) { }

  submitHandler = (formData : any) => {

    const homeRequest : HomeRequest = {
      name: formData.name,
      location: formData.location,
      latitude: formData.latitude,
      longitude: formData.longitude,
      membercount: formData.membercount
    };

    this.homesService.createHome(homeRequest).subscribe({
      next: (response) => this.handleCallAlert('Home created successfully', 'Success', 'success'),
      error: (err) => this.handleCallAlert('Error creating home', 'Error', 'error'),
    });
  }

  cancelHandler = () => {
    this.showAlert = false;
  }

  handleCallAlert = (message:string, title:string, type:string) => {
    this.alert.message = message;
    this.alert.title = title;
    this.alert.type = type;
    this.showAlert = true;
  }
}
