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

  createHome(home: HomeRequest): Observable<HomeResponse> {
    return this.homesApiRepository.createHome(home);
  }

  getHomeMembers(homeId: string): Observable<HomeMemberResponse[]> {
    return this.homesApiRepository.getHomeMembers(homeId);
  }

  getMemberSettings(homeId: string, userId: string): Observable<MemberSettingsResponse[]> {
    return this.homesApiRepository.getMemberSettings(homeId, userId);
  }

  updatePermission(homeId: string, userId: string, permissionRequest: PermissionRequest): Observable<any> {
    return this.homesApiRepository.updatePermission(homeId, userId, permissionRequest);
  }

  addMember(homeId: string, addMemberRequest:AddMemberRequest): Observable<AddMemberResponse> {
    return this.homesApiRepository.addMember(homeId, addMemberRequest);
  }

  getRooms(homeId: string): Observable<RoomResponse[]> {
    return this.homesApiRepository.getRooms(homeId);
  }

  addRoom(homeId: string, roomRequest:NewRoomRequest): Observable<NewRoomResponse> {
    return this.homesApiRepository.addRoom(homeId, roomRequest);
  }
}
