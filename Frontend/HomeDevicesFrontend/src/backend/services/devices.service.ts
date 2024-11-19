import { Injectable } from '@angular/core';
import { DevicesApiRepositoryService } from '../repositories/devices-api-repository.service';
import { NewDeviceRequest } from '../models/in/new-device-request';

@Injectable({
  providedIn: 'root'
})
export class DevicesService {

  constructor(private devicesApiRepository:DevicesApiRepositoryService) { }

  getDevicesByCompanyName(companyName: string){
    return this.devicesApiRepository.getDevicesByCompanyName(companyName);
  }

  createSmartLamp(device: NewDeviceRequest){
    return this.devicesApiRepository.createSmartLamp(device);
  }

  createMovementSensor(device: NewDeviceRequest){
    return this.devicesApiRepository.createMovementSensor(device);
  }

  createWindowSensor(device: NewDeviceRequest){
    return this.devicesApiRepository.createWindowSensor(device);
  }

  createCamera(device: NewDeviceRequest){
    return this.devicesApiRepository.createCamera(device);
  }
  
  getDevicesByCompanyAndModel(companyName: string, model: string){
    return this.devicesApiRepository.getDevicesByCompanyAndModel(companyName, model);
  }
}
