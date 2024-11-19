import { Injectable } from '@angular/core';
import { ValidatorApiRepository } from '../repositories/validator-api-repository.service';

@Injectable({
  providedIn: 'root'
})
export class ValidatorsService {

  constructor(private validatorApiRepository: ValidatorApiRepository) { }

  getValidatorModels() {
    return this.validatorApiRepository.getValidatorModels();
  }
}
