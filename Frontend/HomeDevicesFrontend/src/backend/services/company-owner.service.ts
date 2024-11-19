import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CompanyOwnerApiRepository } from '../repositories/companyOwner-api-repository.service';
import { CompanyOwnerRequest } from '../models/in/company-owner-request';
import { CompanyOwnerResponse } from '../models/out/company-owner-response';

@Injectable({
  providedIn: 'root'
})
export class CompanyOwnerService {
  constructor(private companyOwnerApiRepository: CompanyOwnerApiRepository) {}

  createCompanyOwner(companyOwner: CompanyOwnerRequest): Observable<CompanyOwnerResponse> {
    return this.companyOwnerApiRepository.createCompanyOwner(companyOwner);
  }

}
