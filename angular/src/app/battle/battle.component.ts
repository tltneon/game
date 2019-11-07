import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { HttpService } from '../http.service';
import { BattlesJSON } from '../models/battles';
import { BaseJSON } from '../models/base';

@Component({
  selector: 'app-battle',
  templateUrl: './battle.component.html',
  styleUrls: ['./battle.component.css'],
  providers: [HttpService]
})
export class BattleComponent implements OnInit {

  battlesData: BattlesJSON[];
  basesData: BaseJSON[];
  estiamatedTime: number = 0;
  form;
  destination;

  constructor(private httpService: HttpService) { }

  ngOnInit() {
    this.battlesData = [
      {
        key: "23rdf",
        action: "attacking",
        from: "Planet 1",
        to: "Planet 2",
        departure: 123456612,
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
        key: "fe3df",
        action: "returning",
        from: "Planet 3",
        to: "Planet 1",
        departure: 1234566144,
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
    this.basesData = [
      {
        baseid: 0,
        name: "Planet 2",
        owner: "Test",
        level: 1,
        isactive: true,
        structures: [],
        resources: [],
        units: [],
        task: {action:'',result:'',endsin:0},
      },
      {
        baseid: 0,
        name: "Planet 3",
        owner: "Test2",
        level: 1,
        isactive: true,
        structures: [],
        resources: [],
        units: [],
        task: {action:'',result:'',endsin:0},
      }
    ];
  }

  recalculateTime(){
    this.estiamatedTime = new Date(Date.now()).getSeconds();
  }
  returnSquad(squad){
    let index = this.battlesData.findIndex((element) => element.key == squad.key);
    this.battlesData[index].action ="returning";
    let destination = this.battlesData[index].from;
    this.battlesData[index].from = this.battlesData[index].to;
    this.battlesData[index].to = destination;
    this.battlesData[index].arrival += this.battlesData[index].arrival - this.battlesData[index].departure;
  }
  sendSquad(){
    console.log();
    this.battlesData[this.battlesData.length] = {
      key: "fh6hrtf"+Math.random(),
      action: "attacking",
      from: "Planet 1",
      to: this.destination,
      departure: Math.floor(Math.random()*100000000000),
      arrival: Math.floor(Math.random()*1000000000000),
      units: [
        {
          type: "droneUnit",
          count: Math.floor(Math.random()*10)
        },
        {
          type: "jetUnit",
          count: Math.floor(Math.random()*10)
        },
      ]
    };
    //console.log(this.form.get('first').setValue('some value'));
  }
}

export class SimpleFormGroup {
  form = new FormGroup({
    first: new FormControl('Nancy'),
    last: new FormControl('Drew'),
  });

  get first(): any { return this.form.get('first'); }

  onSubmit(): void {
    console.log(this.form.value);  // {first: 'Nancy', last: 'Drew'}
  }

  setValue() { this.form.setValue({first: 'Carson', last: 'Drew'}); }
}