import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { BattlesJSON } from '../models/battles';

@Component({
  selector: 'app-battle',
  templateUrl: './battle.component.html',
  styleUrls: ['./battle.component.css'],
  providers: [HttpService]
})
export class BattleComponent implements OnInit {

  battlesData: BattlesJSON[];

  constructor(private httpService: HttpService) { }

  ngOnInit() {
    this.battlesData = [
      {
        action: "attacking",
        from: "planet 1",
        to: "planet 2",
        arrival: 123456712,
        units: [
          {
            type: "droneUnit",
            count: 3
          },
          {
            type: "jetUnit",
            count: 5
          },
          {
            type: "lincorUnit",
            count: 5
          },
        ],
      },
      {
        action: "returning",
        from: "planet 3",
        to: "planet 1",
        arrival: 1234567144,
        units: [
          {
            type: "droneUnit",
            count: 1
          },
          {
            type: "jetUnit",
            count: 1
          },
        ],
      }
    ];
  }

}