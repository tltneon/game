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
    isDataLoaded:boolean = false;
    structuresList: string[] = ["lifeComplex", "energyComplex", "aircraftsComplex", "resourceComplex"];
    baseData:any = {};
    interval;

    constructor(private httpService: HttpService){ }

    ngOnInit() {
        //this.httpService.postRequest("api/base/action", {baseid: this.baseData.baseid}, true).subscribe((responce:BaseJSON) => this.baseData == responce ? console.log(responce) : console.log(responce));
        this.loadOnlineData();
        this.interval = setInterval(() => this.baseData.isactive ? this.updateProdution() : ()=>{}, 1000)
    }

    updateProdution() {
        for(let i in this.baseData.resources){
            this.baseData.resources[i].count += Math.floor(Math.pow(100, Math.random()));
        }
    }
    private getStructID(structure):number {
        if(this.baseData.structures)
            return this.baseData.structures.findIndex((element) => element.type == structure.type);
        else
            return null;
    }

    upgradeStructure(structure){
        this.baseData.structures[this.getStructID(structure)].level++;
        this.setStructureTask(structure, 'upgrade', '', 123456789);
    }
    destroyStructure(structure){
        this.baseData.structures.splice(this.getStructID(structure), 1);
    }
    setStructureTask(structure, task: string, result: string = '', finishTime = 12345678){
        this.baseData.structures[this.getStructID(structure)].task = {
            action: task,
            result: result,
            endsin: finishTime                
        }
    }
    clearStructureTask(structure){
        this.baseData.structures[this.getStructID(structure)].task = {
            action: '',
            result: '',
            endsin: 0                
        }
    }

    buildStructure(structureType: string){
        this.setBaseTask('build', structureType, 1234567);
        this.baseData.structures[this.baseData.structures.length] = {
            type: structureType,
            level: 1,
            task: {
                action: '',
                result: '',
                endsin: 0
            }
        }
        this.httpService.postRequest("api/base/action", {action: "build", result: "resourceComplex", baseid: this.baseData.baseID}, true).subscribe((responce) => responce == "success" ? console.log(responce) : console.log(responce));
    }
    upgradeBase(){
        this.setBaseTask('upgrade', '', 12345678);
        this.httpService.postRequest("api/base/action", {action: "upgrade", baseid: this.baseData.baseID}, true).subscribe((responce) => responce == "success" ? this.baseData.level++ : console.log(responce));
    }
    toggleBaseActiveness(){
        this.baseData.isactive = !this.baseData.isactive;
        this.setBaseTask(this.baseData.isactive ? 'repair' : '', '', 12345678);
        this.httpService.postRequest("api/base/action", {action: "repair", baseid: this.baseData.baseID}, true).subscribe((responce) => responce == "success" ? console.log(responce) : console.log(responce));
    }
    setBaseTask(task: string, result: string, finishTime){
        this.baseData.task = {
            action: task,
            result: result,
            endsin: finishTime
        }
    }
    clearBaseTask(){
        this.baseData.task = {
            action: '',
            result: '',
            endsin: 0
        }
    }
    loadOfflineData(){
        this.isDataLoaded = true;
        this.baseData = 
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
        }
    loadOnlineData(){
        function update(is, responce) {
            is.baseData = responce;
            console.log(is.baseData);
            is.baseData.task = {};
            is.isDataLoaded = true;
        }
        this.httpService.postRequest("api/base/RetrieveBaseData", {}, true).subscribe((responce) => update(this, responce));
        //this.httpService.postRequest("api/base/RetrieveBaseStructures", {}, true).subscribe((responce) => console.log(responce));
        //this.isDataLoaded = true;
    }
}