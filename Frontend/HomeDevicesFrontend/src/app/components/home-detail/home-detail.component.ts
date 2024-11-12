import { ActivatedRoute } from '@angular/router';
import { Component } from '@angular/core';

@Component({
  selector: 'app-home-detail',
  templateUrl: './home-detail.component.html',
  standalone: true,
  styleUrls: ['./home-detail.component.css']
})
export class HomeDetailComponent {
  homeId: string | null = null;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    // Obtén el parámetro 'id' de la URL
    this.homeId = this.route.snapshot.paramMap.get('id');
    console.log('Home ID:', this.homeId);
  }
}
