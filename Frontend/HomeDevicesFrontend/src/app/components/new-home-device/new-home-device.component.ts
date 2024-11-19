import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { DefaultButtonComponent } from '../buttons/default-button/default-button.component';
import { DevicesService } from '../../../backend/services/devices.service';
import { HomesService } from '../../../backend/services/homes.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AddHomeDeviceRequest } from '../../../backend/models/in/add-home-device-request';

@Component({
  selector: 'app-new-home-device',
  standalone: true,
  imports: [DynamicFormComponent, DynamicTableComponent, DefaultButtonComponent],
  templateUrl: './new-home-device.component.html',
  styleUrl: './new-home-device.component.css'
})
export class NewHomeDeviceComponent {
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

        if(this.homeId)
        {
          this.homesService.addDeviceToHome(addDeviceToHomeRequest, this.homeId).subscribe((data) => {
            window.alert("Device added to home successfully");
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
        console.log(this.devices);
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
        console.log(this.devices);
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
}
