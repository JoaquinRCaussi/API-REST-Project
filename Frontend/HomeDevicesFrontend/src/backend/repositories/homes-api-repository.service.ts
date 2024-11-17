import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HomeResponse } from '../models/out/home-response';
import ApiRepository from './api-repository';
import domovizApi from '../../environments/environment.local';
import { HomeRequest } from '../models/in/home-request';
import { HomeMemberResponse } from '../models/out/home-member-response';
import { MemberSettingsResponse } from '../models/out/member-settings-response';
import { AddMemberRequest } from '../models/in/add-member-request';
import { AddMemberResponse } from '../models/out/add-member-response';

@Injectable({
  providedIn: 'root'
})
export class HomesApiRepositoryService extends ApiRepository {
  constructor(http: HttpClient) {
    super(domovizApi.domovizApi, 'homes', http);
  }

  public getHomes(): Observable<HomeResponse[]> {
    return this.get();
  }

  public getHome(id: string): Observable<HomeResponse> {
    return this.get(id);
  }

  public createHome(home: HomeRequest): Observable<HomeResponse> {
    return this.post(home);
  }

  public getHomeMembers(homeId: string): Observable<HomeMemberResponse[]> {
    return this.get(homeId + '/members');
  }

  public getMemberSettings(homeId: string, userId: string): Observable<MemberSettingsResponse[]> {
    return this.get(homeId + '/members/' + userId);
  }

  public updatePermission(homeId: string, userId: string, permissionRequest: any): Observable<any> {
    return this.putById(`${homeId}/members/${userId}`, permissionRequest);
  }

  public addMember(homeId: string, addMemberRequest: AddMemberRequest): Observable<AddMemberResponse> {
    return this.putById(`${homeId}/members`, addMemberRequest);
  }
  


}
