import { Component } from '@angular/core';
import { DevicesService } from '../../../backend/services/devices.service';
import { ActivatedRoute,Router } from '@angular/router';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { DefaultButtonComponent } from '../buttons/default-button/default-button.component';

@Component({
  selector: 'app-company-devices',
  standalone: true,
  imports: [DynamicTableComponent, DefaultButtonComponent],
  templateUrl: './company-devices.component.html',
  styleUrl: './company-devices.component.css'
})
export class CompanyDevicesComponent {
  devices: any;
  companyName: any;

  columns = ['Name', 'Type', 'Model', 'Description'];
  devicesType = ['Camera', 'WindowSensor', 'MovementSensor', 'SmartLamp'];
  rows: { [key: string]: string }[] = [];

  constructor(private devicesService:DevicesService, private route:ActivatedRoute, private router:Router) {}

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
}
