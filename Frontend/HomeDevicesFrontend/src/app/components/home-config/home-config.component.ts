import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { HomesService } from '../../../backend/services/homes.service';
import { ActivatedRoute } from '@angular/router';
import { HomeNameRequest } from '../../../backend/models/in/change-home-name-request';

@Component({
  selector: 'app-home-config',
  standalone: true,
  imports: [DynamicFormComponent],
  templateUrl: './home-config.component.html',
  styleUrl: './home-config.component.css'
})
export class HomeConfigComponent {

  actualName:string | null = null;

  homeId: string | null = null;

  home: any;

  formFields:FormField[] = [
    {
      type: 'text',
      name: 'name',
      label: 'Name',
      placeholder: 'Enter the new name of the home',
      required: true
    }
  ];

  constructor(private homesService:HomesService, private route:ActivatedRoute) {}

  ngOnInit() {
    this.homeId = this.route.snapshot.paramMap.get('id');

    if(this.homeId){
      this.homesService.getHome(this.homeId).subscribe((data) => {
        this.home = data;
        this.actualName = this.home.name;
      });
    }
  }

  onChangeName = (formData:any) => {
    const changeHomeNameRequest : HomeNameRequest = {
      homeName: formData.name
    };

    if(this.homeId){
      this.homesService.changeHomeName( this.homeId,changeHomeNameRequest).subscribe((data) => {
        this.home = data;
        this.actualName = this.home.name;
        window.alert('Home name changed successfully');
        window.location.reload();
      });
    }
  }
  
}
