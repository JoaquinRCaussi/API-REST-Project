import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ActivationStart, Router } from '@angular/router';
import { MainContentComponent } from '../main-content/main-content.component';
import { HomesContentComponent } from '../homes-content/homes-content.component';
import { HomeDetailComponent } from '../home-detail/home-detail.component';
import { CommonModule } from '@angular/common';
import { NewHomeComponent } from '../new-home/new-home.component';

@Component({
  selector: 'app-content-area',
  standalone: true,
  imports: [MainContentComponent, HomesContentComponent, HomeDetailComponent,CommonModule, NewHomeComponent],
  templateUrl: './content-area.component.html',
  styleUrl: './content-area.component.css'
})
export class ContentAreaComponent implements OnInit {
  option: string | null = null;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.option = data['option'];
    });
  }
}
