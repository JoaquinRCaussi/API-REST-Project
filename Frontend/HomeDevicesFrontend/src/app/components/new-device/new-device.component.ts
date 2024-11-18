import { Component } from '@angular/core';
import { FormField } from '../../interface/form-field';
import { DeviceType } from '../../../enum/DeviceType';
import { DynamicFormComponent } from "../form/dynamic-form/dynamic-form.component";
import { DevicesService } from '../../../backend/services/devices.service';
import { NewDeviceRequest } from '../../../backend/models/in/new-device-request';

@Component({
  selector: 'app-new-device',
  standalone: true,
  imports: [DynamicFormComponent],
  templateUrl: './new-device.component.html',
  styleUrl: './new-device.component.css'
})
export class NewDeviceComponent {

  options : DeviceType[] = [DeviceType.Camera, DeviceType.WindowSensor, DeviceType.MovementSensor, DeviceType.SmartLamp];

  formFields: FormField[] = [
    {
      name: 'name',
      type: 'text',
      label: 'Name',
      required: true,
      placeholder: 'Enter the name of the device'
    },
    {
      name: 'type',
      type: 'select',
      label: 'Type',
      options: this.options.map(option => ({ value: option, label: option })),
      required: true,
      placeholder: 'Select the type of the device'
    },
    {
      name: 'model',
      type: 'text',
      label: 'Model',
      required: true,
      placeholder: 'Enter the model of the device'
    },
    {
      name: 'description',
      type: 'text',
      label: 'Description',
      required: true,
      placeholder: 'Enter the description of the device'
    },
    {
      name:'photo',
      type:'text',
      label:'Photo',
      required:true,
      placeholder:'Enter the photo of the device'
    }
  ];

  constructor(private devicesService:DevicesService) {}

  onSubmit = (formValue: any) => {

    const deviceRequest : NewDeviceRequest = {
      name: formValue.name,
      model: formValue.model,
      description: formValue.description,
      photo: formValue.photo
    };

    switch(formValue.type){
      case DeviceType.Camera:
        this.devicesService.createCamera(deviceRequest).subscribe((data) => {
          console.log(data);
        });
        break;
      case DeviceType.WindowSensor:
        this.devicesService.createWindowSensor(deviceRequest).subscribe((data) => {
          console.log(data);
        });
        break;
      case DeviceType.MovementSensor:
        this.devicesService.createMovementSensor(deviceRequest).subscribe((data) => {
          console.log(data);
        });
        break;
      case DeviceType.SmartLamp:
        this.devicesService.createSmartLamp(deviceRequest).subscribe((data) => {
          console.log(data);
        });
        break;
    }
  }

}
