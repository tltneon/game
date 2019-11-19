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
  isDataLoaded:boolean = false;
  statsData: StatsJSON[] = [];
  timeLeftMin: number = 4;
  timeLeftSec: number = 59;
  interval;
  index:number = 0;

  constructor(private httpService: HttpService) { }

  ngOnInit() {
    this.loadOnlineData();

    this.interval = setInterval(() => {
      this.timeLeftSec--;
      if(this.timeLeftSec < 0) {
        this.timeLeftMin--;
        this.timeLeftSec = 59;
        if(this.timeLeftSec < 0) {
          this.timeLeftMin = 4;
          this.loadOnlineData();
        }
      }
    }, 1000)
  }

  loadOnlineData(){
    this.httpService.getRequest('api/statistic/GetStats').subscribe(
      (responce: StatsJSON[]) => { 
        console.log(responce);
        this.statsData = responce.sort((a,b) => (a.level > b.level) ? -1 : ((b.level > a.level) ? 1 : 0));
        this.isDataLoaded = true;
        this.timeLeftMin = 4;
        this.timeLeftSec = 59;
      },
      error => console.log(error.message)
    );
  }

  loadOfflineData(){
    this.isDataLoaded = true;
    const t = 29;
    for(let i = 0; i<t; i++)
      this.statsData[this.statsData.length] = {
        playername: "username" + i,
        basename: "basename" + i,
        level: t-i,
        wins: t-i,
        loses: i
      };
  }
}