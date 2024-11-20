import { Component } from '@angular/core';
import { CompaniesService } from '../../../backend/services/companies.service';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-companies-content',
  standalone: true,
  imports: [DynamicTableComponent],
  templateUrl: './companies-content.component.html',
  styleUrls: ['./companies-content.component.css']
})
export class CompaniesContentComponent {
  companies: any[] = [];
  rows: { [key: string]: string }[] = [];  // Assuring that the rows are of type string
  columns = [ 'Name', 'Owner name', 'Owner email', 'RUT'];

  constructor(private companiesService: CompaniesService, private router: Router) {}

  ngOnInit() {
    this.companiesService.getCompanies().subscribe(response => {
      this.companies = response.companies;
      console.log(response.companies);
      this.rows = this.companies.map(company => ({
        Id: company.id,
        Name: company.name.toString(),
        OwnerName: company.ownerName.toString(),
        OwnerEmail: company.ownerEmail.toString(),
        Rut: company.rut.toString()
      }));
    });
  }

  // Function to handle the click event
  onRowClick(row: any): void {
    this.router.navigate(['homes', row.Id]);
  }
}
