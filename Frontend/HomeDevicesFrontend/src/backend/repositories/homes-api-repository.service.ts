import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HomeResponse } from '../models/home-response';
import ApiRepository from './api-repository';
import domovizApi from '../../environments/environment.local';

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
}
