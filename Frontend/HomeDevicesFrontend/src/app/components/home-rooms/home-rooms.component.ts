import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { HomesService } from '../../../backend/services/homes.service';
import { DefaultButtonComponent } from "../buttons/default-button/default-button.component";

@Component({
  selector: 'app-home-rooms',
  standalone: true,
  imports: [DynamicTableComponent, DefaultButtonComponent],
  templateUrl: './home-rooms.component.html',
  styleUrl: './home-rooms.component.css'
})
export class HomeRoomsComponent {
  rooms: any = [];
  homeId: string | null = null;
  columns = ['Name', 'Devices'];
  rows: { [key: string]: string }[] = [];
  selectedRoom: any;

  constructor(
    private route: ActivatedRoute,
    private homesService: HomesService,
    private router: Router
  ) {}

  ngOnInit() {
    this.homeId = this.route.snapshot.paramMap.get('id');

    if (this.homeId) {
      this.homesService.getRooms(this.homeId).subscribe(rooms => {
        this.rooms = rooms;
        this.rows = this.rooms.rooms.map((room: any) => {
          return {
            Id : room.id,
            Name: room.name,
            Devices: room.devices.length.toString()
          };
        });

        console.log(this.rooms);
      });
    }
  }

  onAddRoom = () => {
    this.router.navigate(['homes', this.homeId, 'new-room']);
  }

  onClickRoom = (row: any) => {
    this.selectedRoom = row;
    this.router.navigate(['homes', this.homeId, 'rooms', row.Id]);
  }
}
