import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { UserService } from '../../../backend/services/users.service';
import { GetUserByMailResponse } from '../../../backend/models/out/get-user-by-mail-response';
import { DefaultButtonComponent } from '../buttons/default-button/default-button.component';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { HomesService } from '../../../backend/services/homes.service';
import { AddMemberRequest } from '../../../backend/models/in/add-member-request';
import { ActivatedRoute } from '@angular/router';
import { AlertComponent } from '../alert/alert.component';
import { AlertInterface } from '../../interface/alert';

@Component({
  selector: 'app-new-member',
  standalone: true,
  imports: [DynamicFormComponent,DefaultButtonComponent,DynamicTableComponent, AlertComponent],
  templateUrl: './new-member.component.html',
  styleUrl: './new-member.component.css'
})
export class NewMemberComponent {

  alert: AlertInterface = {
    message: 'User added to home successfully',
    type: 'success',
    title: 'Success'
  };

  showAlert : boolean = false;

  rows: { [key: string]: string }[] = [];
  columns = ['Name', 'Email'];
  homeId: string | null = null;
  user: GetUserByMailResponse | null = null;

  formFields: FormField[] = [
    {
      type: 'text',
      name: 'email',
      label: 'Email',
      placeholder: 'Email of the user',
      required: true
    }
  ]

  constructor(private userService: UserService, private homeService:HomesService, private route:ActivatedRoute) {}

  ngOnInit(): void {
    this.homeId = this.route.snapshot.paramMap.get('id');
  }

  onSearch = (formData: any): void => {
    this.userService.getUserByMail(formData.email).subscribe((user) => {
      this.user = user;
      this.rows = [{ Name: user.name, Email: user.email }];
    }, error => {
      console.error('Error fetching user by email', error);
    });
  }

  onAccept = (): void => {
    if (this.user) {
      const addMemberRequest: AddMemberRequest = {
        userId: this.user.id
      };
      if (this.homeId) {
        this.homeService.addMember(this.homeId, addMemberRequest).subscribe(() => {
          this.alert.message = `User added to home successfully`;
          this.alert.title = 'Success - Member';
          this.alert.type = 'success';
          this.openAlert();
        }, error => {
          console.error('Error adding member to home', error);
          this.alert.message = `Error adding user to home`;
          this.alert.title = 'Error - Member';
          this.alert.type = 'error';
          this.openAlert();
        });
      } else {
        console.error('Home ID is null');
      }
    }
  }

  openAlert = () => {
    this.showAlert = true;
  }

  closeAlert = () => {
    this.showAlert = false;
  }

}
