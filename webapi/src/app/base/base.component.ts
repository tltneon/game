import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { BaseJSON } from '../models/base';

@Component({
    selector: 'app-base',
    templateUrl: './base.component.html',
    styleUrls: ['./base.component.css'],
    providers: [HttpService]
})
export class BaseComponent implements OnInit {
    baseData: BaseJSON[];

    constructor(private httpService: HttpService){ }

    ngOnInit() {
        //this.httpService.getData('http://localhost/testdata.json').subscribe((data: UserJSON) => this.userData = data);
        this.baseData = [
            {
                name: "planet 1",
                owner: "Admin",
                level: 1,
                structures: [
                    {
                        type: "lifeComplex",
                        level: 1
                    },
                    {
                        type: "energyComplex",
                        level: 1
                    },
                    {
                        type: "aircraftsComplex",
                        level: 1
                    },
                    {
                        type: "resourceComplex",
                        level: 1
                    }
                ],
                resources: [
                    {
                        type: "credits",
                        count: 1000
                    },
                    {
                        type: "energy",
                        count: 1000
                    },
                ],
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
            }
        ]
    }
}