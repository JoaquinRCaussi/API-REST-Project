import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { HomesService } from '../../../backend/services/homes.service';
import { ActivatedRoute } from '@angular/router';
import { NewRoomRequest } from '../../../backend/models/in/new-room-request';

@Component({
  selector: 'app-new-room',
  standalone: true,
  imports: [DynamicFormComponent],
  templateUrl: './new-room.component.html',
  styleUrl: './new-room.component.css'
})
export class NewRoomComponent {
  
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
        window.alert('Room created');
      }, () => {
        window.alert('Error creating room');
      });
    }
  }
    
}
