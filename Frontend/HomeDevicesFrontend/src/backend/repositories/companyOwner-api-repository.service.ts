import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import domovizApi from "../../environments/environment.local";
import { Observable } from "rxjs";
import { CompanyOwnerRequest } from "../models/in/company-owner-request";
import { CompanyOwnerResponse } from "../models/out/company-owner-response";

@Injectable({
    providedIn: 'root'
})

export class CompanyOwnerApiRepository extends ApiRepository {
    constructor(http: HttpClient) {
        super(domovizApi.domovizApi, 'company-owner', http);
    }

    public createCompanyOwner(companyOwner: CompanyOwnerRequest): Observable<CompanyOwnerResponse> {
      return this.post(companyOwner);
    }

}
