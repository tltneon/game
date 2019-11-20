export class GameVars {
    readonly data = {
        droneUnit: {
            name: "Drone",
            desctiption: "John Doe",
            power: 10,
            credits: 15,
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
            credits: 45,
            energy: 25,
            basePopulationProduction: 7
        },
        resourceComplex: {
            name: "Resource Complex",
            desctiption: "Producing credits",
            credits: 23,
            energy: 5,
            baseCreditsProduction: 10
        },
        energyComplex: {
            name: "Takomak",
            desctiption: "Producing energy",
            credits: 17,
            energy: 10,
            baseEnergyProduction: 10
        },
        aircraftsComplex: {
            name: "Aircrafts Factory",
            desctiption: "Allows to make units",
            credits: 75,
            energy: 40,
            baseAttackProduction: 0.13
            
        },
        researchStation: {
            name: "Research Station",
            desctiption: "Producing neutrino",
            credits: 10000,
            energy: 25000,
            baseNeutrinoProduction: 0.000001
        },
        /* special */
        base: {
            repair: {
                credits: 20,
                energy: 12
            },
            upgrade: {
                credits: 45,
                energy: 70
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
        "wrongtoken": "Wrong user data",
        "nounits": "You have no units to start attacking someone!",
    }

    getInfo(item: string) {
        return this.data[item] || {};
    }
    getText(string: string): string {
        return this.text[string] || string;
    }

    registerError(msg: string): void {
        document.body.querySelector("#errorSign").classList.add("error")
        let myContainer = <HTMLElement> document.body.querySelector("#errorSign").children[1].children[1];
        myContainer.innerHTML += '<li>' + msg + '</li>';
    }
}