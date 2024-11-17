import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../../backend/services/users.service';
import { HomesService } from '../../../backend/services/homes.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home-member-permissions',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home-member-permissions.component.html',
  styleUrl: './home-member-permissions.component.css'
})
export class HomeMemberPermissionsComponent {
  userId: string | null = null;
  homeId: string | null = null;
  user: any = null;
  userName: string | null = null;
  settings: any = null;
  permissions: any = null;
  availablePermissions = [
    'CanAddMembers',
    'CanCreateRoom',
    'CanAsociateDeviceToRoom',
    'CanChangeHomeName',
    'CanChangeHomeDevicesNames',
    'CanAsociateDevices',
    'CanListDevices',
    'CanGetNotifications'
  ];

  
  constructor(private route:ActivatedRoute, private userService:UserService, private homesService:HomesService) {}

  ngOnInit() {
    this.userId = this.route.snapshot.paramMap.get('userId');
    this.homeId = this.route.snapshot.paramMap.get('id');

    if (this.userId) {
      this.userService.getUser(this.userId).subscribe((user) => {
        this.user = user;
        this.userName = user.name;
      });
    }

    if(this.userId && this.homeId) {
      this.homesService.getMemberSettings(this.homeId, this.userId).subscribe((settings) => {
        this.settings = settings;
        this.permissions = this.settings.permissionsValue;
      });
    }
  }

  hasPermission(permissionToCheck: string): boolean {
    if (this.permissions) {
      return this.permissions.includes(permissionToCheck);
    }
    return false;
  }

  changePermission(permission: string, add:boolean): void {
    console.log('Changing permission', permission, add);
    if (this.userId && this.homeId) {
      const permissionRequest = {
        value: permission,
        enable: add
      };
      this.homesService.updatePermission(this.homeId, this.userId, permissionRequest).subscribe(() => {
        if (this.permissions) {
          if (add) {
            this.permissions.push(permission);
          } else {
            this.permissions = this.permissions.filter((p: string) => p !== permission);
          }
        }
      });
    }
  }
}
