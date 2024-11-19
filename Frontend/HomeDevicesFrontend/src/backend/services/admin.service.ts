import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AdminApiRepository } from '../repositories/admin-api-repository.service';
import { AdminRequest } from '../models/in/admin-request';
import { AdminResponse } from '../models/out/admin-response';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  constructor(private adminApiRepository: AdminApiRepository) {}

  createAdmin(admin: AdminRequest): Observable<AdminResponse> {
    return this.adminApiRepository.createAdmin(admin);
  }

}
