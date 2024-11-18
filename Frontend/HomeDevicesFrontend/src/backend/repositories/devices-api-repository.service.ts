import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import domovizApi from '../../environments/environment.local';
import { NewDeviceRequest } from '../models/in/new-device-request';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DevicesApiRepositoryService extends ApiRepository{

  constructor(http:HttpClient) { 
    super(domovizApi.domovizApi, 'devices', http);
  }

  getDevicesByCompanyName(companyName: string):Observable<any>{
    return this.get(undefined,"companyName=" + companyName);
  }

  createSmartLamp(device: NewDeviceRequest):Observable<any>{
    return this.post(device, 'smartLamp');
  }

  createMovementSensor(device: NewDeviceRequest):Observable<any> {
    return this.post(device, 'movementSensor');
  }

  createWindowSensor(device: NewDeviceRequest):Observable<any> {
    return this.post(device, 'windowSensor');
  }

  createCamera(device: NewDeviceRequest):Observable<any> {
    return this.post(device, 'camera');
  }

}
