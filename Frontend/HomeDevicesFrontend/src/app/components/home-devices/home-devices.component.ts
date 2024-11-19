import { Component } from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { DefaultButtonComponent } from '../buttons/default-button/default-button.component';
import { HomesService } from '../../../backend/services/homes.service';
import { FormField } from '../../interface/form-field';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { HomeDeviceNameRequest } from '../../../backend/models/in/change-hdevice-name.request';

@Component({
  selector: 'app-home-devices',
  standalone: true,
  imports: [DynamicTableComponent, DefaultButtonComponent, DynamicFormComponent],
  templateUrl: './home-devices.component.html',
  styleUrl: './home-devices.component.css'
})
export class HomeDevicesComponent {
  changeHomeDeviceName: boolean = true;

  devices: any;
  homeId: string | null = null;

  columns = ['Name', 'Type', 'Model', 'State'];
  devicesType = ['Camera', 'WindowSensor', 'MovementSensor', 'SmartLamp'];
  rows: { [key: string]: string }[] = [];

  selectedHomeDevice: any;

  formFields : FormField[] = [
    {
      type: 'text',
      name: 'name',
      label: 'Name',
      placeholder: 'Enter the name of the device',
      required: true
    }
  ];

  constructor(private homesService:HomesService, private route:ActivatedRoute, private router:Router) {}

  ngOnInit() {
    this.homeId = this.route.snapshot.paramMap.get('homeId');

    if(this.homeId){
      this.homesService.getHomeDevices(this.homeId).subscribe((data) => {
        this.devices = data;
        this.rows = this.devices.map((device:any) => {
          return {
            HardwareId: device.hardwareId,
            Name: device.name ? device.name : 'No name',
            Type: device.device.deviceType,
            Model: device.device.model,
            State: device.state
          };
        });
      });
    }
  }

  onAddDevice = () => {
    this.router.navigate(['homes', this.homeId, 'new-home-device']);
  }

  onChangeName = (formData:any) => {
    if(this.homeId && this.selectedHomeDevice){
      const changeDeviceNameRequest:HomeDeviceNameRequest = {
        changeDeviceNameRequest: formData.name
      };
      this.homesService.changeHomeDeviceName(this.homeId, this.selectedHomeDevice.HardwareId, changeDeviceNameRequest).subscribe((data) => {
        window.alert("Name updated successfully");
        this.changeHomeDeviceName = false;
        window.location.reload();
      });
    }
  }

  onClickDevice = (row: any) => {
    this.selectedHomeDevice = row;
  }

}