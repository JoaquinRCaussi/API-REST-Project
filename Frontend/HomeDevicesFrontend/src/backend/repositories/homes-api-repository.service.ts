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
import { RoomResponse } from '../models/out/rooms-response';
import { NewRoomRequest } from '../models/in/new-room-request';
import { NewRoomResponse } from '../models/out/new-room-response';
import { AddHomeDeviceRequest } from '../models/in/add-home-device-request';
import { AddDeviceToRoomRequest } from '../models/in/add-device-to-room-request';
import { HomeDeviceNameRequest } from '../models/in/change-hdevice-name.request';
import { HomeNameRequest } from '../models/in/change-home-name-request';

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
  
  public getRooms(homeId: string): Observable<RoomResponse[]> {
    return this.get(homeId + '/rooms');
  }

  public addRoom(homeId: string, roomRequest: NewRoomRequest): Observable<NewRoomResponse> {
    return this.post(roomRequest,`${homeId}/rooms`);
  }

  public getHomeDevices(homeId: string, roomId?:string): Observable<any> {
    return this.get(`${homeId}/devices`, roomId ? `roomId=${roomId}` : '');
  }

  public addDeviceToHome(addDeviceRequest: AddHomeDeviceRequest, homeId: string): Observable<any> {
    return this.post(addDeviceRequest, `${homeId}/devices`);
  }

  public addDeviceToRoom(addDeviceRequest: AddDeviceToRoomRequest, homeId: string, roomId: string): Observable<any> {
    return this.putById(`${homeId}/rooms/${roomId}`, addDeviceRequest);
  }

  public changeHomeDeviceName(homeId: string, hardwareId: string, changeDeviceNameRequest: HomeDeviceNameRequest): Observable<any> {
    return this.putById(`${homeId}/devices/${hardwareId}`, changeDeviceNameRequest );
  }

  public changeHomeName(homeId: string, HomeNameRequest: HomeNameRequest): Observable<any> {
    return this.putById(homeId, HomeNameRequest);
  }

}
