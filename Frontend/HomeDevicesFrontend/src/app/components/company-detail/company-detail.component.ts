import { Component } from '@angular/core';
import { SimpleCardComponent } from '../simple-card/simple-card.component';
import { CompaniesService } from '../../../backend/services/companies.service';
import { UserService } from '../../../backend/services/users.service';

@Component({
  selector: 'app-company-detail',
  standalone: true,
  imports: [SimpleCardComponent],
  templateUrl: './company-detail.component.html',
  styleUrl: './company-detail.component.css'
})
export class CompanyDetailComponent {
  userId: any;
  company: any;
  user: any;
  
  constructor(private companiesService:CompaniesService, private userService:UserService) {}

  ngOnInit() {

    this.userId = localStorage.getItem('userId');
  
    this.userService.getUser(this.userId).subscribe((data) => {
      this.user = data;
      this.companiesService.getCompanyByOwner(this.user.name).subscribe((data) => {
        this.company = data.companies[0];
        console.log(this.company);
      });
    });
  }

}
