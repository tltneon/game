import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { BaseJSON } from '../models/base';
import { Cookie } from 'ng2-cookies/ng2-cookies';

@Component({
    selector: 'app-base',
    templateUrl: './base.component.html',
    styleUrls: ['./base.component.css'],
    providers: [HttpService]
})
export class BaseComponent implements OnInit {
    structures: string[] = ["lifeComplex", "energyComplex", "aircraftsComplex", "resourceComplex"];
    baseData: BaseJSON[];
    interval;

    constructor(private httpService: HttpService){ }

    ngOnInit() {
        this.baseData = [
            {
                baseid: 1,
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
                            action: 'createUnit',
                            result: 'lincorUnit',
                            endsin: 774578585
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
                        count: 0
                    },
                    {
                        type: "energy",
                        count: 0
                    },
                ],
                units: [
                    {
                        type: "droneUnit",
                        count: 0
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
        this.httpService.postRequest("api/base/action", {baseid: this.baseData[0].baseid}, true).subscribe((responce:BaseJSON) => this.baseData[0] == responce ? console.log(responce) : console.log(responce));
        this.interval = setInterval(() => this.baseData[0].isactive ? this.updateProdution() : ()=>{}, 1000)
    }

    updateProdution() {
        for(let i in this.baseData[0].resources){
            this.baseData[0].resources[i].count += Math.floor(Math.pow(100, Math.random()));
        }
    }
    private getStructID(structure):number{
        return this.baseData[0].structures.findIndex((element) => element.type == structure.type);
    }

    upgradeStructure(structure){
        this.baseData[0].structures[this.getStructID(structure)].level++;
        this.setStructureTask(structure, 'upgrade', '', 123456789);
    }
    destroyStructure(structure){
        this.baseData[0].structures.splice(this.getStructID(structure), 1);
    }
    setStructureTask(structure, task: string, result: string = '', finishTime = 12345678){
        this.baseData[0].structures[this.getStructID(structure)].task = {
            action: task,
            result: result,
            endsin: finishTime                
        }
    }
    clearStructureTask(structure){
        this.baseData[0].structures[this.getStructID(structure)].task = {
            action: '',
            result: '',
            endsin: 0                
        }
    }

    buildStructure(structureType: string){
        this.setBaseTask('build', structureType, 1234567);
        this.baseData[0].structures[this.baseData[0].structures.length] = {
            type: structureType,
            level: 1,
            task: {
                action: '',
                result: '',
                endsin: 0
            }
        }
        this.httpService.postRequest("api/base/action", {action: "build", result: "resourceComplex", baseid: this.baseData[0].baseid}, true).subscribe((responce) => responce == "success" ? console.log(responce) : console.log(responce));
    }
    upgradeBase(){
        this.setBaseTask('upgrade', '', 12345678);
        this.httpService.postRequest("api/base/action", {action: "upgrade", baseid: this.baseData[0].baseid}, true).subscribe((responce) => responce == "success" ? this.baseData[0].level++ : console.log(responce));
    }
    toggleBaseActiveness(){
        this.baseData[0].isactive = !this.baseData[0].isactive;
        this.setBaseTask(this.baseData[0].isactive ? 'repair' : '', '', 12345678);
        this.httpService.postRequest("api/base/action", {action: "repair", baseid: this.baseData[0].baseid}, true).subscribe((responce) => responce == "success" ? console.log(responce) : console.log(responce));
    }
    setBaseTask(task: string, result: string, finishTime){
        this.baseData[0].task = {
            action: task,
            result: result,
            endsin: finishTime
        }
    }
    clearBaseTask(){
        this.baseData[0].task = {
            action: '',
            result: '',
            endsin: 0
        }
    }
    loadOnlineData(){
        this.httpService.postRequest("api/base/RetrieveBaseData", {baseid: this.baseData[0].baseid}, true).subscribe((responce) => console.log(responce));
        this.httpService.postRequest("api/base/RetrieveBaseStructures", {baseid: this.baseData[0].baseid}, true).subscribe((responce) => console.log(responce));
    }
}