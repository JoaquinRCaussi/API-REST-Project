import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormField } from '../../../interface/form-field';
import { DefaultButtonComponent } from '../../buttons/default-button/default-button.component';
import { CommonModule } from '@angular/common';
import { AlertInterface } from '../../../interface/alert';
import { AlertComponent } from '../../alert/alert.component';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, DefaultButtonComponent, CommonModule, AlertComponent]
})
export class DynamicFormComponent implements OnInit {
  @Input() fields: FormField[] = [];
  @Input() submitHandler: (formData: any) => void = () => {};
  @Input() buttonText: string = 'Submit';

  alert: AlertInterface = {
    message: 'Not valid form. Please check the fields',
    type: 'error',
    title: 'Form validation'
  };

  showAlert : boolean = false;

  form: FormGroup = new FormGroup({});

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.form = this.fb.group({});
    this.fields.forEach(field => {
      this.form.addControl(
        field.name,
        this.fb.control('', field.required ? Validators.required : null)
      );
    });
  }

  onSubmit() {
    if (this.form.valid && this.submitHandler) {
      this.submitHandler(this.form.value);
      this.clearForm();

    } else {
      this.openAlert();
    }
  }

  clearForm() {
    this.form.reset();
  }

  closeAlert = () => {
    this.showAlert = false;
  }

  openAlert = () => {
    this.showAlert = true;
  }
}
