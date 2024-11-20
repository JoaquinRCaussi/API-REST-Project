import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';
import { ValidatorsService } from '../../../backend/services/validator-service.service';
import { CompaniesService } from '../../../backend/services/companies.service';

@Component({
  selector: 'app-new-company',
  standalone: true,
  imports: [DynamicFormComponent],
  templateUrl: './new-company.component.html',
  styleUrl: './new-company.component.css'
})
export class NewCompanyComponent {

  validators: string[] = [];

  formFields: FormField[] = [
    {
      name: 'name',
      type: 'text',
      placeholder: 'Company name',
      label: 'Name',
      required: true
    },
    {
      name: 'rut',
      type: 'text',
      label: 'Rut',
      placeholder: 'Rut',
      required: true
    },
    {
      name: 'logo',
      type: 'text',
      label: 'Logo',
      placeholder: 'Logo',
      required: true
    },
    {
      name: 'validatorModelName',
      type: 'select',
      label: 'Validator Model',
      options: this.validators.map(validator => ({ label: validator, value: validator })),
      required: true
    }
  ];

  constructor(private validatorsService: ValidatorsService, private companiesService: CompaniesService) {}

  ngOnInit() {
    this.validatorsService.getValidatorModels().subscribe((data) => {
      this.validators = data;
      this.formFields[3].options = this.validators.map(validator => ({
        label: validator,
        value: validator
      }));
    });
  }

  onSubmit = (formData: any) => {

    const newCompanyRequest = {
      name: formData.name,
      rut: formData.rut,
      logo: formData.logo,
      validatorModelName: formData.validatorModelName
    };

    this.companiesService.createCompany(newCompanyRequest).subscribe((data) => {
      console.log(data);
    });
  }
}
