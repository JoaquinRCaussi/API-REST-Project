import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import domovizApi from "../../environments/environment.local";
import { GetUserByMailResponse } from "../models/out/get-user-by-mail-response";
import { Observable } from "rxjs";
import { NewCompanyRequest } from "../models/in/new-company-request";
import { NewCompanyResponse } from "../models/out/new-company-response";

@Injectable({
    providedIn: 'root'
})

export class CompaniesApiRepository extends ApiRepository {
    constructor(http: HttpClient) {
        super(domovizApi.domovizApi, 'companies', http);
    }

    createCompany(company: NewCompanyRequest): Observable<NewCompanyResponse> {
        return this.post(company);
    }
}