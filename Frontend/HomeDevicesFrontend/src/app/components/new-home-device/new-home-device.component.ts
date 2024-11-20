import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { DefaultButtonComponent } from '../buttons/default-button/default-button.component';
import { DevicesService } from '../../../backend/services/devices.service';
import { HomesService } from '../../../backend/services/homes.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AddHomeDeviceRequest } from '../../../backend/models/in/add-home-device-request';
import { AlertComponent } from '../alert/alert.component';
import { AlertInterface } from '../../interface/alert';

@Component({
  selector: 'app-new-home-device',
  standalone: true,
  imports: [DynamicFormComponent, DynamicTableComponent, DefaultButtonComponent, AlertComponent],
  templateUrl: './new-home-device.component.html',
  styleUrl: './new-home-device.component.css'
})
export class NewHomeDeviceComponent {

  alert: AlertInterface = {
    message: 'Device added to home successfully',
    type: 'success',
    title: 'Success'
  };

  showAlert : boolean = false;


  homeId: string | null = null;
  companyName:string | null = null;
  model: string | null = null;
  rows: { [key: string]: string }[] = [];
  columns:string[] = ['Name','Type', 'Model', 'Description']
  devicesType = ['Camera', 'WindowSensor', 'MovementSensor', 'SmartLamp'];

  devices:any;
  selectedDevice:any;

  formFields : FormField[] = [
    {
      type: 'text',
      name: 'company',
      label: 'Company',
      placeholder: 'Enter the name of the company',
      required: true
    },
    {
      type: 'text',
      name: 'model',
      label: 'Model',
      placeholder: 'Enter the model of the device to add',
      required: false
    }
  ];

  constructor(private devicesService:DevicesService, private homesService:HomesService, private route:ActivatedRoute, private router:Router) {}

  onAccept = () => {

    this.homeId = this.route.snapshot.paramMap.get('homeId');

    this.devices.forEach((device:any) => {
      if(device.id === this.selectedDevice.id){
        const addDeviceToHomeRequest : AddHomeDeviceRequest = {
          deviceId: this.selectedDevice.id
        };

        if (this.homeId) {
          this.homesService.addDeviceToHome(addDeviceToHomeRequest, this.homeId).subscribe({
            next: (data) => {
              this.alert.message = 'Device added to home successfully.';
              this.alert.type = 'success';
              this.alert.title = 'Success - Device added';
              this.openAlert();
            },
            error: (err) => {
              console.error('Error adding device to home:', err);
              this.alert.message = 'An error occurred while adding the device to the home.';
              this.alert.type = 'error';
              this.alert.title = 'Error - Device added';
              this.openAlert();
            }
          });
        }

      }
    });

    this.devices = null;
    this.selectedDevice = null;
  }

  onSearch = (formData: any) => {
    this.companyName = formData.company;
    this.model = formData.model;

    if(this.companyName && !this.model){
      this.devicesService.getDevicesByCompanyName(this.companyName).subscribe((data) => {
        this.devices = data.devices;
        if(this.devices){
          this.rows = this.devices.map((device:any) => ({
            Name: device.name,
            Type: this.devicesType[device.deviceType],
            Model: device.model,
            Description: device.description
          }));
        }else{
          this.rows = [];
        }
      });
    }

    if(this.companyName && this.model){
      this.devicesService.getDevicesByCompanyAndModel(this.companyName, this.model).subscribe((data) => {
        this.devices = data.devices;
        if(this.devices){
          this.rows = this.devices.map((device:any) => ({
            Name: device.name,
            Type: this.devicesType[device.deviceType],
            Model: device.model,
            Description: device.description
          }));
        }else{
          this.rows = [];
        }
      });
    }
  }

  onClickRow = (row: any) => {
    this.selectedDevice = this.devices.find((device:any) => device.model === row.Model && device.name === row.Name);
  }

  openAlert = () => {
    this.showAlert = true;
  }

  closeAlert = () => {
    this.showAlert = false;
  }
}
