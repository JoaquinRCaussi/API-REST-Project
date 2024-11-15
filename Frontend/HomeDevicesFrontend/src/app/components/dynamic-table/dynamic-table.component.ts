import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dynamic-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dynamic-table.component.html',
  styleUrl: './dynamic-table.component.css'
})
export class DynamicTableComponent {
  @Input() columns: string[] = [];
  @Input() rows: any[] = [];
  @Input() interacts: boolean = true;
  @Input() actionToPerform: (row: any) => void = () => {};

  constructor(private router : Router) {}
}
