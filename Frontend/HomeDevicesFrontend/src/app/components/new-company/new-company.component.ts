import { Component } from '@angular/core';
import { DynamicFormComponent } from '../form/dynamic-form/dynamic-form.component';
import { FormField } from '../../interface/form-field';

@Component({
  selector: 'app-new-company',
  standalone: true,
  imports: [DynamicFormComponent],
  templateUrl: './new-company.component.html',
  styleUrl: './new-company.component.css'
})
export class NewCompanyComponent {

  //call controller / make controller for validatorService
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
      options: [{
        label: 'Validator Model Name',
        value: 'Validator Model Name'
      }],
      required: true
    }
  ]

  constructor() {}
}
