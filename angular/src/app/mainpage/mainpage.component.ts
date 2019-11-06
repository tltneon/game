import { Component } from '@angular/core';
import { HttpService } from '../http.service';

@Component({
    selector: '',
    templateUrl: './mainpage.component.html',
    styleUrls: ['./mainpage.component.css'],
    providers: [HttpService]
})

export class MainPageComponent {
    data;

    constructor(private httpService: HttpService){ }

    ngOnInit() {
    }
}