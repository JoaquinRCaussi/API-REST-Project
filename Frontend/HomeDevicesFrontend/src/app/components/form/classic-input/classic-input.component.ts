import { Component, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-classic-input',
  standalone: true,
  imports: [],
  templateUrl: './classic-input.component.html',
  styleUrl: './classic-input.component.css'
})
export class ClassicInputComponent {
  @Input() label: string | null = null;
  @Input() type: 'text' | 'number' = 'text';
  @Input() value: string | null = null;
  @Input() placeholder: string | null = 'Placeholder...';

  @Output() valueChange = new EventEmitter<string>();

  public onValueChange(event: any): void {
    this.valueChange.emit(event.target.value);
  }
}
