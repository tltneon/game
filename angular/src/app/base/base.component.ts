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
    isDataLoaded: boolean = false;
    resourceProduction: {
        credits: number;
        energy: number;
        neutrino: number;
        population: number;
    } = {
        credits: 0,
        energy: 0,
        neutrino: 0,
        population: 0
    };
    structuresList: string[] = ["lifeComplex", "energyComplex", "aircraftsComplex", "resourceComplex", "researchStation"];
    unitsList: string[] = ["droneUnit", "jetUnit", "lincorUnit", "someGiantShitUnit"];
    baseData: any = {};
    interval;

    requestedCreditsToUnit: number = 0;
    requestedEnergyToUnit: number = 0;
    requestedCreditsToStructure: number = 0;
    requestedEnergyToStructure: number = 0;

    constructor(private httpService: HttpService, private gameVars: GameVars){
        this.baseData.structures = [];
        this.baseData.resources = {};
        this.baseData.task = {};
    }

    ngOnInit() {
        this.loadOnlineData();
        this.interval = setInterval(() => this.baseData.isActive && this.updateProdution(), 999)
    }
    // проверяет какие здания можно построить
    allowToBuild(): string[] {
        return this.structuresList.filter(element => this.baseData.structures.findIndex(o => o.type == element) == -1);
    }
    // выводит число с точностью до второго знака
    formatNumber(num: number): string {
        num = num || 0;
        return num.toFixed(2);
    }
    // высчитывает мультипликатор защиты базы
    defenceMultiplier(): string {
        return this.formatNumber(this.baseData.level * 0.16 + 1);
    }
    // обновляет данные формы покупки юнитов и строений
    updateInput(isStructureInput: boolean, unitInput: string, count: number = 1): void {
        if(isStructureInput) {
            this.requestedCreditsToStructure = this.gameVars.getInfo(unitInput).credits;
            this.requestedEnergyToStructure = this.gameVars.getInfo(unitInput).energy;
        }
        else {
            this.requestedCreditsToUnit = this.gameVars.getInfo(unitInput).credits * count;
            this.requestedEnergyToUnit = this.gameVars.getInfo(unitInput).energy * count;
        }
    }
    // вычитает ресурсы
    reduceCounters(credits: number = 0, energy: number = 0, neutrino: number = 0): void {
        this.baseData.resources.credits -= credits;
        this.baseData.resources.energy -= energy;
        this.baseData.resources.neutrino -= neutrino;
    }
    // пересчитывает размер начисляемых ресурсов
    recalculateProduction(): void {
        this.resourceProduction.credits = 0;
        this.resourceProduction.energy = 0;
        this.resourceProduction.neutrino = 0;
        this.resourceProduction.population = 0;
        this.baseData.structures.forEach(element => {
            this.resourceProduction.credits += this.gameVars.getInfo(element.type).baseCreditsProduction * element.level || 0;
            this.resourceProduction.energy += this.gameVars.getInfo(element.type).baseEnergyProduction * element.level || 0;
            this.resourceProduction.neutrino += this.gameVars.getInfo(element.type).baseNeutrinoProduction * element.level || 0;
            this.resourceProduction.population += this.gameVars.getInfo(element.type).basePopulationProduction * element.level || 0;
        });
        this.resourceProduction.credits /= 60;
        this.resourceProduction.energy /= 60;
        this.resourceProduction.neutrino /= 60;
    }
    // обновляет индикатор с ресурсами (бар в верхней части экрана)
    updateProdution(): void {
        this.baseData.resources.credits += this.resourceProduction.credits;
        this.baseData.resources.energy += this.resourceProduction.energy;
        this.baseData.resources.neutrino += this.resourceProduction.neutrino;
    }
    // грузит оффлайновые данные
    private getStructID(structure: any): number {
        if(this.baseData.structures)
            return this.baseData.structures.findIndex((element) => element.type == structure.type);
        else
            return null;
    }
    // улучшает постройку
    upgradeStructure(structure: any): void {
        this.setStructureTask(structure, 'upgrade', '', 123456789);
        
        this.httpService.postRequest(
            "api/base/action", 
            {action: "upgradestructure", baseid: this.baseData.baseID, result: structure.type})
                .subscribe(
                    (responce: string) => {
                        if(responce == "success") {
                            this.baseData.structures[this.getStructID(structure)].level++;
                            this.reduceCounters(
                                this.gameVars.getInfo(structure.type).credits * structure.level,
                                this.gameVars.getInfo(structure.type).energy * structure.level, 
                                this.gameVars.getInfo(structure.type).neutrino * structure.level);
                        }
                        alert(this.gameVars.getText(responce));
                    });
    }
    // убивает строение (только UI)
    destroyStructure(structure: object): void {
        this.baseData.structures.splice(this.getStructID(structure), 1);
    }
    // назначает таску строения
    setStructureTask(structure: object, task: string, result: string = '', finishTime: number = 0):void {
        this.baseData.structures[this.getStructID(structure)].task = {
            action: task,
            result: result,
            endsin: finishTime                
        }
    }
    // проверяет, построен ли строительный комплекс
    canMakeUnits():boolean {
        return this.baseData.structures.findIndex(p => p.type == 'aircraftsComplex') + 1;
    }
    // создаёт юнит
    makeUnit(unitType: string): void {
        this.setBaseTask('makeunit', unitType, 1234567);
        this.httpService.postRequest(
            "api/base/action", 
            {action: "makeunit", result: unitType, baseid: this.baseData.baseID})
                .subscribe(
                    (responce: string) => {
                        if(responce == "success") {
                            let index = this.baseData.units.findIndex(p => p.type == unitType);
                            if(index == -1)
                                this.baseData.units[this.baseData.units.length] = {type: unitType, count: 1};
                            else
                                this.baseData.units[index].count++;
                                this.reduceCounters(
                                    this.gameVars.getInfo(unitType).credits,
                                    this.gameVars.getInfo(unitType).energy, 
                                    this.gameVars.getInfo(unitType).neutrino);
                        }
                        alert(this.gameVars.getText(responce));
                    },
                    error => console.log(error));
    }
    // создаёт постройку
    buildStructure(structureType: string): void {
        if(structureType != undefined)
        {
            this.setBaseTask('build', structureType, 1234567);
            this.httpService.postRequest(
                "api/base/action", 
                {action: "build", result: structureType, baseid: this.baseData.baseID})
                    .subscribe(
                        (responce) => {
                            if(responce == "success") {
                                this.baseData.structures[this.baseData.structures.length] = {
                                    type: structureType,
                                    level: 1,
                                    task: {
                                        action: '',
                                        result: '',
                                        endsin: 0
                                    }
                                };
                                this.reduceCounters(
                                    this.gameVars.getInfo(structureType).credits, 
                                    this.gameVars.getInfo(structureType).energy, 
                                    this.gameVars.getInfo(structureType).neutrino);
                            }
                            alert(this.gameVars.getText(responce.toString()));
                            this.recalculateProduction();
                        },
                        error => console.log(error));
        }
    }
    // улучшает базу
    upgradeBase(): void {
        this.setBaseTask('upgrade', '', 12345678);
        this.httpService.postRequest("api/base/action", {action: "upgrade", baseid: this.baseData.baseID}).subscribe(
            (responce) => {
                if(responce == "success"){
                    this.baseData.level++;
                    this.reduceCounters(
                        this.gameVars.getInfo("base").upgrade.credits * this.baseData.level,
                        this.gameVars.getInfo("base").upgrade.energy * this.baseData.level, 
                        this.gameVars.getInfo("base").upgrade.neutrino * this.baseData.level);
                }
                alert(this.gameVars.getText(responce.toString()));
            },
            error => this.gameVars.registerError(error.message));
    }
    // переключает активность базы ака ремонтирует
    toggleBaseActiveness(): void {
        this.setBaseTask(this.baseData.isActive ? 'repair' : '', '', 12345678);
        this.httpService.postRequest("api/base/action", {action: "repair", baseid: this.baseData.baseID}).subscribe(
            (responce) => {
                responce == "success" 
                    ? this.baseData.isActive = !this.baseData.isActive 
                    : console.log(responce)
                alert(this.gameVars.getText(responce.toString()));
            },
            error => this.gameVars.registerError(error.message));
    }
    // назначает таску базе
    setBaseTask(task: string, result: string, finishTime: number): void {
        this.baseData.task = {
            action: task,
            result: result,
            endsin: finishTime
        }
    }
    // грузит онлайновые данные
    loadOnlineData(): void {
        this.httpService.postRequest("api/base/RetrieveBaseData", {}, true).subscribe(
            (responce) => {
                if(responce == null)
                {
                    this.loadOfflineData();
                    this.gameVars.registerError("api/base/RetrieveBaseData => null");
                }
                else
                {
                    this.baseData = responce;
                    this.baseData.task = {};
                }
                this.isDataLoaded = true;
                this.recalculateProduction();
            },
            error => this.gameVars.registerError(error.message));
    }
    // грузит оффлайновые данные
    loadOfflineData(): void {
        this.isDataLoaded = true;
        this.baseData = {
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
            resources: {
                credits: 330,
                energy: 120,
                neutrino: 0.0,
            },
            units: [
                {
                    type: "droneUnit",
                    count: 1
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
        this.recalculateProduction();
    }
}