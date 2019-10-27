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
    interval;

    constructor(private httpService: HttpService){ }

    ngOnInit() {
        //this.httpService.getData('http://localhost/testdata.json').subscribe((data: UserJSON) => this.userData = data);
        this.baseData = [
            {
                name: "planet 1",
                owner: "Admin",
                level: 1,
                isactive: true,
                structures: [
                    {
                        type: "lifeComplex",
                        level: 1,
                        task: {
                            action: 'upgrade',
                            result: '',
                            endsin: 123456733
                        }
                    },
                    {
                        type: "energyComplex",
                        level: 1,
                        task: {
                            action: '',
                            result: '',
                            endsin: 0
                        }
                    },
                    {
                        type: "aircraftsComplex",
                        level: 1,
                        task: {
                            action: '',
                            result: '',
                            endsin: 0
                        }
                    },
                    {
                        type: "resourceComplex",
                        level: 1,
                        task: {
                            action: '',
                            result: '',
                            endsin: 0
                        }
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
                task: {
                    action: 'build',
                    result: 'droneUnit',
                    endsin: 198765433,
                }
            }
        ]
        this.interval = setInterval(() => { this.updateProdution() }, 1000)
    }

    updateProdution() {
        for(let i in this.baseData[0].resources){
            this.baseData[0].resources[i].count+=Math.floor(Math.pow(100, Math.random()));
        }
    }
}