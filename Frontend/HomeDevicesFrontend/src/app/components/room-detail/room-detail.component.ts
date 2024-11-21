import { Component } from '@angular/core';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { HomesService } from '../../../backend/services/homes.service';
import { ActivatedRoute } from '@angular/router';
import { DefaultButtonComponent } from "../buttons/default-button/default-button.component";
import { AddDeviceToRoomRequest } from '../../../backend/models/in/add-device-to-room-request';
import { AlertComponent } from '../alert/alert.component';
import { AlertInterface } from '../../interface/alert';

@Component({
  selector: 'app-room-detail',
  standalone: true,
  imports: [DynamicTableComponent, DefaultButtonComponent, AlertComponent],
  templateUrl: './room-detail.component.html',
  styleUrl: './room-detail.component.css'
})
export class RoomDetailComponent {

  alert: AlertInterface = {
    message: 'Device added to room successfully',
    type: 'success',
    title: 'Success'
  };

  showAlert : boolean = false;

  rooms: any = [];
  room: any;

  homeId: string | null = null;

  roomId: string | null = null;

  columns = ['Name', 'Type', 'Model', 'State'];

  deviceType = ['Camera', 'WindowSensor', 'MovementSensor', 'SmartLamp'];

  rows: { [key: string]: string }[] = [];
  homeDevicesRows: { [key: string]: string }[] = [];

  allDevices: any;
  filtredDevices: any;
  selectedDevice: any;

  addDevice:boolean = false;


  constructor(private homesService:HomesService, private route:ActivatedRoute) {}

  ngOnInit() {

    this.homeId = this.route.snapshot.paramMap.get('homeId');
    this.roomId = this.route.snapshot.paramMap.get('roomId');

    if(this.homeId && this.roomId){
      this.homesService.getHomeDevices(this.homeId, this.roomId).subscribe((data) => {
        this.filtredDevices = data;
        this.homeDevicesRows = this.filtredDevices.map((device:any) => {
          return {
            HardwareId: device.HardwareId,
            Name: device.name ? device.name : 'No name',
            Type: this.deviceType[device.device.deviceType],
            Model: device.device.model,
            State: device.state
          };
        });
      });

      this.homesService.getRooms(this.homeId).subscribe(rooms => {
        this.rooms = rooms;
        this.room = this.rooms.rooms.find((room: any) => room.id === this.roomId);
        });
    }

    if(this.homeId){
      if(this.homeId){
        this.homesService.getHomeDevices(this.homeId).subscribe((data) => {
          this.allDevices = data;
          this.rows = this.allDevices.map((device:any) => {
            return {
              HardwareId: device.hardwareId,
              Name: device.name ? device.name : 'No name',
              Type: this.deviceType[device.device.deviceType],
              Model: device.device.model,
              State: device.state
            };
          });
        });
      }
    }
  }

  onAddDevice = () => {
    this.addDevice = true;
  }

  onAccept = () => {
    this.addDevice = false;

    if(!this.selectedDevice){
      this.alert.message = 'Please select a device';
      this.alert.type = 'error';
      this.alert.title = 'Select a device';
      this.openAlert();
      return;
    }

    if(this.homeId && this.roomId){

      const addDeviceToRoomRequest : AddDeviceToRoomRequest = {
        hardwareId: this.selectedDevice.HardwareId
      };

      this.homesService.addDeviceToRoom(addDeviceToRoomRequest, this.homeId, this.roomId).subscribe({
        next: (data) => {
          this.alert.message = 'Device added to room successfully.';
          this.alert.type = 'success';
          this.alert.title = 'Success - Device added';
          this.openAlert();
        },
        error: (err) => {
          console.error('Error adding device to room:', err);
          this.alert.message = 'An error occurred while adding the device to the room.';
          this.alert.type = 'error';
          this.alert.title = 'Error - Device not added';
          this.openAlert();
        }
      });
    }
  }

  onCancel = () => {
    this.addDevice = false;
    this.selectedDevice = null;
  }

  onClickRow = (row: any) => {
    this.selectedDevice = row;
    console.log(this.selectedDevice);
  }

  closeAlert = () => {
    this.showAlert = false;
    window.location.reload();
  }

  openAlert = () => {
    this.showAlert = true;
  }

}
