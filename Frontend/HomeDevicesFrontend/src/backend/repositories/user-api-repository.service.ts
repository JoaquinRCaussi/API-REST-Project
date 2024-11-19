import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import domovizApi from "../../environments/environment.local";
import { GetUserByMailResponse } from "../models/out/get-user-by-mail-response";
import { UsersResponse } from "../models/out/users-response";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})

export class UserApiRepository extends ApiRepository {
    constructor(http: HttpClient) {
        super(domovizApi.domovizApi, 'users', http);
    }

    public getUserByMail(mail: string): Observable<GetUserByMailResponse> {
        return this.get("by-email/" + mail);
    }

    public getUser(userId: string): Observable<GetUserByMailResponse> {
        return this.get(userId);
    }

    public getUsers(): Observable<UsersResponse> {
        return this.get();
    }

    public getUserNotifications(userId: string): Observable<any> {
        return this.get(userId + '/notifications');
    }

}
