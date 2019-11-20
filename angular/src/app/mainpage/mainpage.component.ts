import { Component } from '@angular/core';
import { HttpService } from '../http.service';
import { GameVars } from '../gamevars';

@Component({
    selector: '',
    templateUrl: './mainpage.component.html',
    styleUrls: ['./mainpage.component.css'],
    providers: [HttpService, GameVars]
})

export class MainPageComponent {
    data;

    constructor(private httpService: HttpService, private gameVars: GameVars) {}

    ngOnInit() {}
}