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

  statsData: StatsJSON[] = [];
  timeLeftMin: number = 4;
  timeLeftSec: number = 59;
  interval;

  constructor(private httpService: HttpService) { }

  ngOnInit() {
    this.updateStats();

    this.interval = setInterval(() => {
      this.timeLeftSec--;
      if(this.timeLeftSec < 0) {
        this.timeLeftMin--;
        this.timeLeftSec = 59;
        if(this.timeLeftSec < 0) {
          this.timeLeftMin = 4;
          this.updateStats();
        }
      }
    }, 1000)
  }
  updateStats(){
    this.httpService.getRequest('api/statistic/getPlayerList').subscribe(
      (responce: StatsJSON[]) => this.statsData = responce,
      error => console.log(error.message)
    );
  }
  loadDummyData(){
    for(let i = 0; i<29;i++)
      this.statsData[this.statsData.length] = {
        name: "username"+i,
        basename: "basename"+i,
        level: 1,
        wins: 1,
        loses: i,
        score: 10-i,
      };
  }
}