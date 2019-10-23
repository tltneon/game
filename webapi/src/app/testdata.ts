export class UserJSON {
    name: string;
    level: number;
    energy: number;
    credits: number;
}
export class BaseJSON {
    name: string;
    level: number;
}
export class StructuresJSON {
    type: string;
    level: number;
}
export class ResourcesJSON {
    energy: number;
    credits: number;
    population: number;
}

export class BattlesJSON {
    action: string;
    from: string;
    to: string;
    arrival: number;
}

export class StatsJSON {
    name: string;
    basename: string;
    level: number;
    wins: number;
    loses: number;
    resources: string;
}

export class UnitsJSON {
    name: string;
    level: number;
}