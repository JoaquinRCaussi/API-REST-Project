import { Component, OnInit } from '@angular/core';
import { ActivationStart, Router } from '@angular/router';
import { MainContentComponent } from '../main-content/main-content.component';
import { HomesContentComponent } from '../homes-content/homes-content.component';

@Component({
  selector: 'app-content-area',
  standalone: true,
  imports: [MainContentComponent, HomesContentComponent],
  templateUrl: './content-area.component.html',
  styleUrl: './content-area.component.css'
})
export class ContentAreaComponent implements OnInit{
  private routeData: any;
  route: string = 'main-option';

  constructor(private router: Router) {}

  ngOnInit() {
      this.router.events.subscribe(data => {
        if (data instanceof ActivationStart) {
          console.log(`Custom data`, data.snapshot.data);
          this.routeData = data.snapshot.data;
          console.log(this.routeData);
          if (this.routeData) {
            this.route = this.routeData.option;
          }
        }
    });
  }
}
