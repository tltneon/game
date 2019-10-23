import { Component, OnInit } from '@angular/core';
import { HttpService } from './http.service';
import { UserJSON } from './testdata';
import { BaseJSON } from './models/base';
import { BattlesJSON } from './models/battles';
import { StatsJSON } from './models/stats';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    /*template: `<div>
                    <p>Имя пользователя: {{userData?.name}}</p>
                    <p>Имя пользователя: {{userData?.level}}</p>
               </div>`,*/
    providers: [HttpService]
})
export class AppComponent implements OnInit { 
   
    userData: UserJSON;
    /*baseData: BaseJSON;
    battlesData: BattlesJSON;
    statsData: StatsJSON;*/
 
    constructor(private httpService: HttpService){}
      
    ngOnInit() {
        this.httpService.getData().subscribe((data: UserJSON) => this.userData = data);
    }
}