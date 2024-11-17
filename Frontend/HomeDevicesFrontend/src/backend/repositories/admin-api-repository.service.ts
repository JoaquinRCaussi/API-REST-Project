import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import domovizApi from "../../environments/environment.local";
import { Observable } from "rxjs";
import { AdminRequest } from "../models/in/admin-request";
import { AdminResponse } from "../models/out/admin-response";

@Injectable({
    providedIn: 'root'
})

export class AdminApiRepository extends ApiRepository {
    constructor(http: HttpClient) {
        super(domovizApi.domovizApi, 'admins', http);
    }

    public createAdmin(admin: AdminRequest): Observable<AdminResponse> {
      return this.post(admin);
    }

}
