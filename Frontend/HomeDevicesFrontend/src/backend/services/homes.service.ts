import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { HomesApiRepositoryService } from '../repositories/homes-api-repository.service';
import { HomeResponse } from '../models/home-response';

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
}
