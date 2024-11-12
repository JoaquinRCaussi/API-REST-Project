import { Component } from '@angular/core';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';

@Component({
  selector: 'app-homes-content',
  standalone: true,
  imports: [DynamicTableComponent],
  templateUrl: './homes-content.component.html',
  styleUrl: './homes-content.component.css'
})
export class HomesContentComponent {

  homes = [
    {
      name: 'Casa 1',
      price: 100000,
      description: 'Casa en la playa',
      image: 'https://via.placeholder.com/150'
    },
    {
      name: 'Casa 2',
      price: 200000,
      description: 'Casa en la montaña',
      image: 'https://via.placeholder.com/150'
    },
    {
      name: 'Casa 3',
      price: 300000,
      description: 'Casa en la ciudad',
      image: 'https://via.placeholder.com/150'
    }
  ];

  columns = [ 'name', 'price', 'description', 'image' ];

}
