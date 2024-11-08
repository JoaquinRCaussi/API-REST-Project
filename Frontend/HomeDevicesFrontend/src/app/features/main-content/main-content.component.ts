import { Component } from '@angular/core';
import { DefaultButtonComponent } from '../../components/buttons/default-button/default-button.component';
import { ClassicInputComponent } from '../../components/form/classic-input/classic-input.component';

@Component({
  selector: 'app-main-content',
  standalone: true,
  imports: [DefaultButtonComponent, ClassicInputComponent],
  templateUrl: './main-content.component.html',
  styleUrl: './main-content.component.css'
})
export class MainContentComponent {

}
