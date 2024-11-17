import { Component, Input } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-simple-card',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './simple-card.component.html',
  styleUrl: './simple-card.component.css'
})
export class SimpleCardComponent {
  @Input() title = 'Simple Card';
  @Input() routerLink = '';
  @Input() description = '';
  constructor() { }
}
