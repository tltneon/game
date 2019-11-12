import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { GameVars } from '../gamevars';

@Component({
    selector: 'app-base',
    templateUrl: './base.component.html',
    styleUrls: ['./base.component.css'],
    providers: [HttpService, GameVars]
})
export class BaseComponent implements OnInit {
    isDataLoaded:boolean = false;
    structuresList: string[] = ["lifeComplex", "energyComplex", "aircraftsComplex", "resourceComplex"];
    unitsList: string[] = ["droneUnit", "jetUnit", "lincorUnit", "someGiantShitUnit"];
    baseData:any = {};
    interval;

    constructor(private httpService: HttpService, private gameVars: GameVars){
        this.baseData.structures = [];
        this.baseData.task = {};
    }

    ngOnInit() {
        this.loadOnlineData();
        this.interval = setInterval(() => this.baseData.isActive ? this.updateProdution() : ()=>{}, 1000)
    }

    allowToBuild() {
        return this.structuresList.filter(element => this.baseData.structures.findIndex(o => o.type == element) == -1);
    }
    defenceMultiplier():string {
        return (this.baseData.level * 1.1).toFixed(2);
    }

    updateProdution() {
        for(let i in this.baseData.resources){
            this.baseData.resources[i].count += this.baseData.level * 1;
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

    canMakeUnits(){
        return this.baseData.structures.findIndex(p => p.type == 'aircraftsComplex') + 1;
    }
    makeUnit(unitType: string){
        function update(it, responce){
            let index = it.baseData.units.findIndex(p => p.type == responce);
            if(index)
                it.baseData.units[it.baseData.units.length] = {type: unitType, count: 1};
            else
                it.baseData.units[index].count++;
            console.log(responce);
        }
        this.setBaseTask('makeunit', unitType, 1234567);
        this.httpService.postRequest("api/base/action", {action: "makeunit", result: unitType, baseid: this.baseData.baseID}, true).subscribe((responce) => responce == "success" ? update(this, responce) : console.log(responce));
    }
    buildStructure(structureType: string){
        function update(it, responce){
            it.baseData.structures[it.baseData.structures.length] = {
                type: structureType,
                level: 1,
                task: {
                    action: '',
                    result: '',
                    endsin: 0
                }
            };
            console.log(responce);
        }
        this.setBaseTask('build', structureType, 1234567);
        this.httpService.postRequest("api/base/action", {action: "build", result: structureType, baseid: this.baseData.baseID}, true).subscribe((responce) => responce == "success" ? update(this, responce) : console.log(responce));
    }
    upgradeBase(){
        this.setBaseTask('upgrade', '', 12345678);
        this.httpService.postRequest("api/base/action", {action: "upgrade", baseid: this.baseData.baseID}, true).subscribe((responce) => responce == "success" ? this.baseData.level++ : console.log(responce));
    }
    toggleBaseActiveness(){
        this.baseData.isActive = !this.baseData.isActive;
        this.setBaseTask(this.baseData.isActive ? 'repair' : '', '', 12345678);
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
    loadOnlineData(){
        function update(is, responce) {
            if(responce == null)
            {
                console.log("ошибке");
                is.loadOfflineData();
            }
            else
            {
                is.baseData = responce;
                console.log(is.baseData);
                is.baseData.task = {};
            }
            is.isDataLoaded = true;
        }
        this.httpService.postRequest("api/base/RetrieveBaseData", {}, true).subscribe((responce) => update(this, responce));
    }
    loadOfflineData(){
        this.isDataLoaded = true;
        this.baseData = 
            {
                baseid: 1,
                name: "planet 1",
                owner: "Admin",
                level: 1,
                isActive: true,
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
}