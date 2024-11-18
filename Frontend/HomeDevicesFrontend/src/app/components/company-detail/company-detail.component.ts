import { Component } from '@angular/core';
import { SimpleCardComponent } from '../simple-card/simple-card.component';
import { CompaniesService } from '../../../backend/services/companies.service';

@Component({
  selector: 'app-company-detail',
  standalone: true,
  imports: [SimpleCardComponent],
  templateUrl: './company-detail.component.html',
  styleUrl: './company-detail.component.css'
})
export class CompanyDetailComponent {
  userToken: any;
  company: any;
  user: any;
  
  constructor(private companiesService:CompaniesService) {}

  ngOnInit() {

    this.userToken = localStorage.getItem('token');

    this.companiesService.getCompanyByOwner('ownerName').subscribe((data) => {
      console.log(data);
    });
  }

}
