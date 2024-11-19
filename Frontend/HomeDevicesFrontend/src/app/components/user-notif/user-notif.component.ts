import { Component } from '@angular/core';
import { UserService } from '../../../backend/services/users.service';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';

@Component({
  selector: 'app-user-notif',
  standalone: true,
  imports: [DynamicTableComponent],
  templateUrl: './user-notif.component.html',
  styleUrl: './user-notif.component.css'
})
export class UserNotifComponent {

  columns = ['CreatedAt', 'HomeDName', 'HomeDState', 'Event', 'Read'];

  rows: { [key: string]: string }[] = [];

  userId: string | null = '';

  notifications: any[] = [];

  constructor(private userService: UserService) {
  }

  ngOnInit() {
    this.userId = localStorage.getItem('userId');

    if(this.userId){
      this.userService.getUserNotifications(this.userId).subscribe((data) => {
        this.notifications = data;
        this.rows = this.notifications.map(notification => ({
          Id: notification.id.toString(),
          CreatedAt: this.formatDate(notification.createdAt.toString()),
          HomeDName: notification.homeDevice.name.toString(),
          HomeDState: this.formatState(notification.homeDevice.state.toString()),
          Event: notification.event.toString(),
          Read: notification.isRead.toString()
        }));
        console.log(this.rows);
      });
    }
  }

  onClickNotificationRead(row: any): void {
  }

  formatDate(ISODate: string): string {
    const date = new Date(ISODate);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  formatState(state: string): string {
    return state === 'true' ? 'ON' : 'OFF';
  }
}
