import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { HomesService } from '../../../backend/services/homes.service';
import { ActivatedRoute } from '@angular/router';
import { NewRoomRequest } from '../../../backend/models/in/new-room-request';
import { AlertComponent } from '../alert/alert.component';
import { AlertInterface } from '../../interface/alert';

@Component({
  selector: 'app-new-room',
  standalone: true,
  imports: [DynamicFormComponent, AlertComponent],
  templateUrl: './new-room.component.html',
  styleUrl: './new-room.component.css'
})
export class NewRoomComponent {

  alert: AlertInterface = {
    message: 'Room created successfully',
    type: 'success',
    title: 'Success'
  };

  showAlert : boolean = false;
  
  formFields: FormField[] = [
    {
      type: 'text',
      name: 'name',
      label: 'Name',
      placeholder: 'Name of the room',
      required: true
    }
  ];

  constructor(private homesService:HomesService, private route:ActivatedRoute) {}

  onCreateRoom = (formData: any): void => {
    const homeId = this.route.snapshot.paramMap.get('id');
    const roomRequest : NewRoomRequest = {
      roomName: formData.name
    };
    console.log(roomRequest);
    if(homeId)
    {
      this.homesService.addRoom(homeId,roomRequest).subscribe(() => {
        this.alert.message = 'Room created successfully';
        this.alert.type = 'success';
        this.alert.title = 'Success - Room';
        this.openAlert();
      }, () => {
        this.alert.message = 'Error creating room';
        this.alert.type = 'error';
        this.alert.title = 'Error - Room';
        this.openAlert();
      });
    }
  }

  openAlert = () => {
    this.showAlert = true;
  }

  closeAlert = () => {
    this.showAlert = false;
  }
    
}
