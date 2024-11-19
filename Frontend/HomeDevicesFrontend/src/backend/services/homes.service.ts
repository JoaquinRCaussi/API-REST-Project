import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { HomesApiRepositoryService } from '../repositories/homes-api-repository.service';
import { HomeResponse } from '../models/out/home-response';
import { HomeRequest } from '../models/in/home-request';
import { HomeMemberResponse } from '../models/out/home-member-response';
import { MemberSettingsResponse } from '../models/out/member-settings-response';
import { PermissionRequest } from '../models/in/permission-request';
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
export class HomesService {
  constructor(private homesApiRepository: HomesApiRepositoryService) {}

  getHomes(): Observable<HomeResponse[]> {
    return this.homesApiRepository.getHomes().pipe(
      map((homes: any[]) => homes.map(home => {
        // filtering out memberSettings
        const { memberSettings, ...filteredHome } = home;
        return filteredHome;
      }))
    );
  }

  getHome(id: string): Observable<HomeResponse> {
    return this.homesApiRepository.getHome(id).pipe(
      map((home: any) => {
        // filtering out memberSettings
        const { memberSettings, ...filteredHome } = home;
        return filteredHome;
      })
    );
  }

  public createHome(home: HomeRequest): Observable<HomeResponse> {
    return this.homesApiRepository.createHome(home);
  }

  public getHomeMembers(homeId: string): Observable<HomeMemberResponse[]> {
    return this.homesApiRepository.getHomeMembers(homeId);
  }

  public getMemberSettings(homeId: string, userId: string): Observable<MemberSettingsResponse[]> {
    return this.homesApiRepository.getMemberSettings(homeId, userId);
  }

  public updatePermission(homeId: string, userId: string, permissionRequest: PermissionRequest): Observable<any> {
    return this.homesApiRepository.updatePermission(homeId, userId, permissionRequest);
  }

  public addMember(homeId: string, addMemberRequest:AddMemberRequest): Observable<AddMemberResponse> {
    return this.homesApiRepository.addMember(homeId, addMemberRequest);
  }

  public getRooms(homeId: string): Observable<RoomResponse[]> {
    return this.homesApiRepository.getRooms(homeId);
  }

  public addRoom(homeId: string, roomRequest:NewRoomRequest): Observable<NewRoomResponse> {
    return this.homesApiRepository.addRoom(homeId, roomRequest);
  }

  public getHomeDevices(homeId: string, roomId?:string): Observable<any> {
    return this.homesApiRepository.getHomeDevices(homeId, roomId);
  }

  public addDeviceToHome(addDeviceRequest: AddHomeDeviceRequest, homeId: string): Observable<any> {
    return this.homesApiRepository.addDeviceToHome(addDeviceRequest, homeId);
  }

  public addDeviceToRoom(addDeviceRequest: AddDeviceToRoomRequest, homeId: string, roomId: string): Observable<any> {
    return this.homesApiRepository.addDeviceToRoom(addDeviceRequest, homeId, roomId);
  }

  public changeHomeDeviceName(homeId: string, hardwareId: string, changeDeviceNameRequest: HomeDeviceNameRequest): Observable<any> {
    return this.homesApiRepository.changeHomeDeviceName(homeId, hardwareId, changeDeviceNameRequest);
  }

  public changeHomeName(homeId: string, HomeNameRequest: HomeNameRequest): Observable<any> {
    return this.homesApiRepository.changeHomeName(homeId, HomeNameRequest);
  }
}
