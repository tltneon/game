export class GameVars {
    readonly data = {
        droneUnit: {
            name: "Drone",
            desctiption: "John Doe",
            power: 10,
            credits: 100,
            energy: 25
        },
        jetUnit: {
            name: "Jet",
            desctiption: "John Doe",
            power: 110,
            credits: 1000,
            energy: 250
        },
        lincorUnit: {
            name: "Lincor",
            desctiption: "John Doe",
            power: 1500,
            credits: 10000,
            energy: 2500
        },
        someGiantShitUnit: {
            name: "Death Star",
            desctiption: "John Doe",
            power: 200000,
            credits: 1000000,
            energy: 250000,
            neutrino: 1
        },
        /* structs */
        lifeComplex: {
            name: "Living Complex",
            desctiption: "Increasing population",
            credits: 300,
            energy: 25,
            basePopulationProduction: 7
        },
        resourceComplex: {
            name: "Resource Complex",
            desctiption: "Producing credits",
            credits: 100,
            energy: 25,
            baseCreditsProduction: 10
        },
        energyComplex: {
            name: "Takomak",
            desctiption: "Producing energy",
            credits: 100,
            energy: 25,
            baseEnergyProduction: 10
        },
        aircraftsComplex: {
            name: "Aircrafts Factory",
            desctiption: "Allows to make units",
            credits: 1000,
            energy: 250
            
        },
        researchStation: {
            name: "Research Station",
            desctiption: "Producing neutrino",
            credits: 100000,
            energy: 250000,
            baseNeutrinoProduction: 0.000001
        },
        /* special */
        base: {
            repair: {
                credits: 2000,
                energy: 2000
            },
            upgrade: {
                credits: 5000,
                energy: 5000
            },
        }
    }
    text = {
        "notanowner": "You're not an owner",
        "notenoughresources": "You have no enough resources to make your dreams come true",
        "alreadyexists": "That structure already made",
        "wronginput": "Wrong input",
        "wrongbaseid": "Wrong base data",
        "success": "Successfully perform an act",
        "youwin": "You win that battle!",
        "youlose": "You lose that battle",
        "cannotuseatyourself": "You can't perform that action on yourself!",
        "baseisinactive": "You can't attack innactive base!",
        "populationLimit": "Not enough population to create new unit",
        "noLifeComplex": "Life Complex is not exists on the base",
        "noAircrafts": "Aircrafts Factory is not exists on the base",
        "wrongdatareceived": "Wrong data received",
        "wrongpassword": "Wrong password",
    }

    getInfo(item: string) {
        return this.data[item] || {};
    }
    getText(string: string): string {
        return this.text[string] || string;
    }

    registerError(msg: string) {
        document.body.querySelector("#errorSign").classList.add("error")
        let myContainer = <HTMLElement> document.body.querySelector("#errorSign").children[1].children[1];
        myContainer.innerHTML += '<li>' + msg + '</li>';
    }
}