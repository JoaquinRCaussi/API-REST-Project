import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { HomesService } from '../../../backend/services/homes.service';
import { ActivatedRoute } from '@angular/router';
import { HomeNameRequest } from '../../../backend/models/in/change-home-name-request';
import { AlertComponent } from '../alert/alert.component';
import { AlertInterface } from '../../interface/alert';

@Component({
  selector: 'app-home-config',
  standalone: true,
  imports: [DynamicFormComponent, AlertComponent],
  templateUrl: './home-config.component.html',
  styleUrl: './home-config.component.css'
})
export class HomeConfigComponent {

  alert: AlertInterface = {
    message: 'Home name changed successfully',
    type: 'success',
    title: 'Success'
  };

  showAlert : boolean = false;

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

    if (this.homeId) {
      this.homesService.changeHomeName(this.homeId, changeHomeNameRequest).subscribe({
        next: (data) => {
          this.home = data;
          this.actualName = this.home.name;
          this.alert.message = 'Home name changed successfully.';
          this.alert.type = 'success';
          this.alert.title = 'Home name - Success';
          this.openAlert();
        },
        error: (err) => {
          console.error('Error changing home name:', err);
          this.alert.message = 'An error occurred while changing the home name.';
          this.alert.type = 'error';
          this.alert.title = 'Home name - Error';
          this.openAlert();
        },
      });
    }
  }

  closeAlert = () => {
    this.showAlert = false;
    window.location.reload();
  }

  openAlert = () => {
    this.showAlert = true;
  }
  
}
