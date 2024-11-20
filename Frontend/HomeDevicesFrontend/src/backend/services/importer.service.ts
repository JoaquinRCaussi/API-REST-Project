import { Injectable } from '@angular/core';
import { ImporterApiRepositoryService } from '../repositories/importer-api-repository.service';
import { ImportDevicesRequest } from '../models/in/import-devices-request';

@Injectable({
  providedIn: 'root'
})

export class ImporterService {

  constructor(private importerApiRepository:ImporterApiRepositoryService) { }

  public importDevices(importDevicesRequest:ImportDevicesRequest){
    return this.importerApiRepository.importDevices(importDevicesRequest);
  }
}
