import { Component } from '@angular/core';
import { HomesService } from '../../../backend/services/homes.service';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-homes-content',
  standalone: true,
  imports: [DynamicTableComponent],
  templateUrl: './homes-content.component.html',
  styleUrls: ['./homes-content.component.css']
})
export class HomesContentComponent {
  homes: any[] = [];
  rows: { [key: string]: string }[] = [];  // Assuring that the rows are of type string
  columns = [ 'Name', 'Location', 'Members', 'Owner', 'Devices', 'Rooms' ];

  constructor(private homesService: HomesService, private router: Router) {}

  ngOnInit() {
    this.homesService.getHomes().subscribe(homes => {
      this.homes = homes;
      console.log(homes);

      this.rows = homes.map(home => ({
        Id: home.id.toString(),
        Name: home.name.toString(),
        Location: home.location.toString(),
        Members: home.memberSettings?.length?.toString() || '',
        Owner: home.owner.name.toString(),
        Devices: home.devices?.length?.toString() || '',
        Rooms: home.rooms?.length?.toString() || ''
      }));

      
    });
  }

  // Function to handle the click event
  onRowClick(row: any): void {
    this.router.navigate(['homes', row.Id]);
  }
}
