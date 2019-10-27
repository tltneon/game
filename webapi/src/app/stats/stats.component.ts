import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { StatsJSON } from '../models/stats';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.css'],
  providers: [HttpService]
})
export class StatsComponent implements OnInit {

  statsData: StatsJSON[];
  timeLeftMin: number = 5;
  timeLeftSec: number = 0;
  interval;

  constructor(private httpService: HttpService) { }

  ngOnInit() {
    //this.httpService.getData().subscribe(data => this.statsData = data);


    this.interval = setInterval(() => {
      this.timeLeftSec--;
      if(this.timeLeftSec < 0) {
        this.timeLeftMin--;
        this.timeLeftSec = 59;
      }
    }, 1000)

    this.statsData = [
      {
        name: "test1",
        basename: "testbase",
        level: 1,
        wins: 1,
        loses: 1,
        score: 2,
      },
      {
        name: "test2",
        basename: "testbase",
        level: 1,
        wins: 1,
        loses: 1,
        score: 2,
      },
      {
        name: "test3",
        basename: "testbase",
        level: 1,
        wins: 1,
        loses: 1,
        score: 2,
      },
      {
        name: "test4",
        basename: "testbase",
        level: 1,
        wins: 1,
        loses: 1,
        score: 2,
      },
      {
        name: "test3",
        basename: "testbase",
        level: 1,
        wins: 1,
        loses: 1,
        score: 2,
      },
      {
        name: "test4",
        basename: "testbase",
        level: 1,
        wins: 1,
        loses: 1,
        score: 2,
      },
      {
        name: "test3",
        basename: "testbase",
        level: 1,
        wins: 1,
        loses: 1,
        score: 2,
      },
      {
        name: "test4",
        basename: "testbase",
        level: 1,
        wins: 1,
        loses: 1,
        score: 2,
      }
    ];
  }

}