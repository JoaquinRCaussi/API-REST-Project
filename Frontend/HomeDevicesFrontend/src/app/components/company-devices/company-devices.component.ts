import { Component } from '@angular/core';
import { DevicesService } from '../../../backend/services/devices.service';
import { ActivatedRoute,Router } from '@angular/router';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { DefaultButtonComponent } from '../buttons/default-button/default-button.component';
import { FormField } from '../../interface/form-field';
import { DynamicFormComponent } from "../form/dynamic-form/dynamic-form.component";
import { ImporterService } from '../../../backend/services/importer.service';
import { ImportDevicesRequest } from '../../../backend/models/in/import-devices-request';
import { AlertComponent } from '../alert/alert.component';
import { AlertInterface } from '../../interface/alert';

@Component({
  selector: 'app-company-devices',
  standalone: true,
  imports: [DynamicTableComponent, DefaultButtonComponent, DynamicFormComponent, AlertComponent],
  templateUrl: './company-devices.component.html',
  styleUrl: './company-devices.component.css'
})
export class CompanyDevicesComponent {

  alert: AlertInterface = {
    message: 'Devices imported successfully',
    type: 'success',
    title: 'Success'
  };

  showAlert : boolean = false;

  formFields : FormField[] = [
    {
      name: 'file',
      label: 'Path (.dll)',
      type: 'text',
      required: true,
      placeholder: "Please enter the correct path here",
    }
  ];
  devices: any;
  companyName: any;

  columns = ['Name', 'Type', 'Model', 'Description'];
  devicesType = ['Camera', 'WindowSensor', 'MovementSensor', 'SmartLamp'];
  rows: { [key: string]: string }[] = [];

  showImporter = false;

  constructor(
    private devicesService:DevicesService,
    private route:ActivatedRoute,
    private router:Router,
    private importerService:ImporterService) {}

  ngOnInit() {
    this.companyName = this.route.snapshot.paramMap.get('companyName');
    this.devicesService.getDevicesByCompanyName(this.companyName).subscribe((data) => {
      this.devices = data.devices;
      if(this.devices){
        this.rows = this.devices.map((device:any) => ({
          Name: device.name,
          Type: this.devicesType[device.deviceType],
          Model: device.model,
          Description: device.description
        }));

        console.log(this.devices);
      }else{
        this.rows = [];
      }
    });
  }

  onAddDevice = () => {
    this.router.navigate(['companies', this.companyName, 'new-device']);
  }

  onClickSeeImport = () => { 
    this.showImporter = !this.showImporter;
  }

  onSubmitted = (formData:any) => {

    console.log(formData);

    let importDevicesRequest:ImportDevicesRequest= {
      companyName: this.companyName,
      assemblyPath: formData.file
    };
  
    this.importerService.importDevices(importDevicesRequest).subscribe((data) => {
      this.openAlert('Devices imported successfully', 'Devices - Success', 'success');
    });
  }
  
  cancelAlert = () => {
    this.showAlert = false;
    window.location.reload();
  }

  openAlert = (message:string, title:string, type:string) => {
    this.alert.message = message;
    this.alert.title = title;
    this.alert.type = type;

    this.showAlert = true;

  }
  
}
