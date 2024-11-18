import { Component } from '@angular/core';
import { SimpleCardComponent } from '../simple-card/simple-card.component';

@Component({
  selector: 'app-company-detail',
  standalone: true,
  imports: [SimpleCardComponent],
  templateUrl: './company-detail.component.html',
  styleUrl: './company-detail.component.css'
})
export class CompanyDetailComponent {
  
  constructor() {}


}
