import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { HomesApiRepositoryService } from '../repositories/homes-api-repository.service';
import { HomeResponse } from '../models/home-response';
import { HomeRequest } from '../models/in/home-request';

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
    console.log('Sending home creation request:', home);
    return this.homesApiRepository.createHome(home);
  }
}
