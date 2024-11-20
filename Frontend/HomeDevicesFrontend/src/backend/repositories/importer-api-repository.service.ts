import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import domoviz from '../../environments/environment.local';
import { Observable } from 'rxjs';
import { ImportDevicesRequest } from '../models/in/import-devices-request';


@Injectable({
  providedIn: 'root'
})
export class ImporterApiRepositoryService extends ApiRepository {

  constructor(http: HttpClient) {
    super(domoviz.domovizApi, 'import-devices', http);
  }

  public importDevices(importDevicesRequest:ImportDevicesRequest):Observable<any> {
    return this.post(importDevicesRequest);
  }
}
