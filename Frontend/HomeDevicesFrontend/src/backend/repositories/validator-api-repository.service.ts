import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import domoviz from '../../environments/environment.local';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ValidatorApiRepository extends ApiRepository {

  constructor(http: HttpClient) {
    super(domoviz.domovizApi, 'validator', http);
  }

  public getValidatorModels():Observable<any> {
    return this.get('load-validators');
  }
}
