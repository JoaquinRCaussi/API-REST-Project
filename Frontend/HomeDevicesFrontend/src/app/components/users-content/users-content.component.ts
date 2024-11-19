import { Component } from '@angular/core';
import { UserService } from '../../../backend/services/users.service';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-users-content',
  standalone: true,
  imports: [DynamicTableComponent],
  templateUrl: './users-content.component.html',
  styleUrls: ['./users-content.component.css']
})
export class UsersContentComponent {
  users: any[] = [DynamicTableComponent];
  rows: { [key: string]: string }[] = [];
  columns = [ 'Name', 'LastName', 'Role', 'CreatedAt' ];

  constructor(private usersService: UserService, private router: Router) {}

  ngOnInit() {
    this.usersService.getUsers().subscribe(response => {
      this.users = response.users;
      this.rows = this.users.map(user => ({
        Name: user.name.toString(),
        LastName: user.lastName.toString(),
        Role: user.role.name.toString(),
        CreatedAt: user.createdAt.toString()
      }));

      console.log(this.rows);
    });
  }

  // Function to handle the click event
  onRowClick(row: any): void {
    this.router.navigate(['homes', row.Id]);
  }
}
