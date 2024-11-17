import { ActivatedRoute } from '@angular/router';
import { Component } from '@angular/core';
import { HomesService } from '../../../backend/services/homes.service';
import { HomeResponse } from '../../../backend/models/out/home-response';
import { CommonModule } from '@angular/common';
import { SimpleCardComponent } from '../simple-card/simple-card.component';
import { SignUpComponent } from "../../layouts/sign-up/sign-up.component";

@Component({
  selector: 'app-home-detail',
  templateUrl: './home-detail.component.html',
  imports: [CommonModule, SimpleCardComponent, SignUpComponent],
  standalone: true,
  styleUrls: ['./home-detail.component.css']
})
export class HomeDetailComponent {
  homeId: string | null = null;
  home: HomeResponse | null = null;

  constructor(private route: ActivatedRoute, private homesService:HomesService) {}

  ngOnInit(): void {
    this.homeId = this.route.snapshot.paramMap.get('id');

    if (this.homeId) {
      this.homesService.getHome(this.homeId).subscribe((home) => {
        //TODO: Check what is returned here, maybe we need yo check Rooms in Repo at Backend
        this.home = home;
      });
    }
  }
}
