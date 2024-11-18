import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { Observable } from "rxjs";
import { NewCompanyRequest } from "../models/in/new-company-request";
import domoviz from '../../environments/environment.local';
import { NewCompanyResponse } from "../models/out/new-company-response";

@Injectable({
    providedIn: 'root'
})

export class CompaniesApiRepository extends ApiRepository {
    constructor(http: HttpClient) {
        super(domoviz.domovizApi, 'companies', http);
    }

    createCompany(company: NewCompanyRequest): Observable<NewCompanyResponse> {
        return this.post(company);
    }

    getCompanyByOwner(ownerName: string): Observable<any> {
        return this.get(undefined,ownerName);
    }
}