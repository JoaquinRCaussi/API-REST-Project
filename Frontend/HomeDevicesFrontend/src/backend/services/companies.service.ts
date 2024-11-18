import { Injectable } from '@angular/core';
import { CompaniesApiRepository } from '../repositories/companies-api-repository.service';
import { NewCompanyRequest } from '../models/in/new-company-request';
import { Observable } from 'rxjs';
import { NewCompanyResponse } from '../models/out/new-company-response';

@Injectable({
  providedIn: 'root'
})
export class CompaniesService {

  constructor(private companiesApiRepository:CompaniesApiRepository) { }

  createCompany(company: NewCompanyRequest) : Observable<NewCompanyResponse>{
    return this.companiesApiRepository.createCompany(company);
  }

  getCompanyByOwner(ownerName: string): Observable<any> {
    return this.companiesApiRepository.getCompanyByOwner(ownerName);
  }
}
