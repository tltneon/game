import { Component } from '@angular/core';
import { HttpService } from '../http.service';

@Component({
    selector: '',
    templateUrl: './mainpage.component.html',
    styleUrls: ['./mainpage.component.css'],
    providers: [HttpService]
})

export class MainPageComponent {

    constructor(private httpService: HttpService){ }

    ngOnInit() {
        //this.httpService.postRequest("api/statistic/dbstatus", {}).subscribe(res => console.log(res)); // requesting DB status
    }
}