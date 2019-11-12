export class GameVars {
    data = {
        droneUnit: {
            name: "Drone",
            desctiption: "John Doe",
            power: 10,
            cost: {
                credit: 100,
                energy: 25
            }
        },
        jetUnit: {
            name: "Jet",
            desctiption: "John Doe",
            power: 110,
            cost: {
                credit: 1000,
                energy: 250
            }
        },
        lincorUnit: {
            name: "Lincor",
            desctiption: "John Doe",
            power: 1500,
            cost: {
                credit: 10000,
                energy: 2500
            }
        },
        someGiantShitUnit: {
            name: "Death Star",
            desctiption: "John Doe",
            power: 1000000,
            cost: {
                credit: 1000000,
                energy: 250000
            }
        },
        /* structs */
        lifeComplex: {
            name: "Living Complex",
            desctiption: "John Doe",
            power: 1000000,
            cost: {
                credit: 1000000,
                energy: 250000
            }
        },
        resourceComplex: {
            name: "Resource Complex",
            desctiption: "John Doe",
            power: 1000000,
            cost: {
                credit: 1000000,
                energy: 250000
            }
        },
        energyComplex: {
            name: "Takomak",
            desctiption: "John Doe",
            power: 1000000,
            cost: {
                credit: 1000000,
                energy: 250000
            }
        },
        aircraftsComplex: {
            name: "Aircrafts Factory",
            desctiption: "John Doe",
            power: 1000000,
            cost: {
                credit: 1000000,
                energy: 250000
            }
        },
    }

    constructor(){}

    getInfo(item:string) {
        return this.data[item];
    }
}