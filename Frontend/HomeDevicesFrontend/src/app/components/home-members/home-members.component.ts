import { Component, Input} from '@angular/core';
import { HomesService } from '../../../backend/services/homes.service';
import { HomeMemberResponse } from '../../../backend/models/out/home-member-response';
import { ActivatedRoute, Router } from '@angular/router';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { DefaultButtonComponent } from '../buttons/default-button/default-button.component';
import { UserService } from '../../../backend/services/users.service';
import { GetUserByMailResponse } from '../../../backend/models/out/get-user-by-mail-response';

@Component({
  selector: 'app-home-members',
  standalone: true,
  imports: [DynamicTableComponent, DefaultButtonComponent],
  templateUrl: './home-members.component.html',
  styleUrl: './home-members.component.css'
})
export class HomeMembersComponent {
  @Input() homeId: string | null = null;

  columns = ['Name', 'Email'];
  rows: { [key: string]: string }[] = [];
  userId: string | null = null;
  userResponse: any = [DynamicTableComponent];

  homeMembers: HomeMemberResponse[] | null = null;

  constructor(private homesService: HomesService, private userService:UserService, private route: ActivatedRoute, private router:Router) {}

  ngOnInit(): void {
    this.homeId = this.route.snapshot.paramMap.get('id');

    if (this.homeId) {
      this.homesService.getHomeMembers(this.homeId).subscribe((homeMembers) => {
        this.homeMembers = homeMembers;
        this.rows = homeMembers.map((homeMember) => ({
          Name: homeMember.name,
          Email: homeMember.email
      }));
    });
    }
  }

  onRowClick= (row: any): void => {

    if (row.Email) {
      this.userService.getUserByMail(row.Email).subscribe((user) => {
        this.userResponse = user;
        this.userId = user.id;
        this.router.navigate(['/homes', this.homeId, 'members', this.userId]);
      }, error => {
        console.error('Error fetching user by email', error);
      });
    } else {
      console.error('No email found in row', row);
    }
  }


  onAddMemberClick = (): void => {
    this.router.navigate(['/homes', this.homeId, 'new-member']);
  }

}
