import { Component, Input } from '@angular/core';
import { DefaultButtonComponent } from "../buttons/default-button/default-button.component";
import { AlertInterface } from '../../interface/alert';

@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [DefaultButtonComponent, DefaultButtonComponent],
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.css'
})
export class AlertComponent {
  @Input() alert: AlertInterface = {
    message: 'This is an alert',
    type: 'success',
    title: 'Alert'
  };
  @Input() show: boolean = true;
  @Input() onCloseClick = () => {};

}
