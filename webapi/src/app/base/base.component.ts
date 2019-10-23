import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { UserJSON } from '../testdata';

@Component({
    selector: 'app-base',
    templateUrl: './base.component.html',
    styleUrls: ['./base.component.css'],
    providers: [HttpService]
})
export class BaseComponent implements OnInit {
    userData: UserJSON;

    constructor(private httpService: HttpService){ }

    ngOnInit() {
        this.httpService.getData().subscribe((data: UserJSON) => this.userData = data);
    }
}