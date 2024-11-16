import { Component, Input} from '@angular/core';
import { HomesService } from '../../../backend/services/homes.service';
import { HomeMemberResponse } from '../../../backend/models/out/home-member-response';
import { ActivatedRoute } from '@angular/router';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { DefaultButtonComponent } from '../buttons/default-button/default-button.component';

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

  homeMembers: HomeMemberResponse[] | null = null;

  constructor(private homesService: HomesService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.homeId = this.route.snapshot.paramMap.get('id');

    if (this.homeId) {
      this.homesService.getHomeMembers(this.homeId).subscribe((homeMembers) => {
        //TODO: Check the creation of homes, some homes dont have any member
        this.homeMembers = homeMembers;
        this.rows = homeMembers.map((homeMember) => ({
          Name: homeMember.name,
          Email: homeMember.email
      }));
    });
    }    
  }

  onRowClick(row: any): void {
    console.log(row);
  }

  onAddMemberClick(): void {
    console.log('Add member clicked');
  }

}
