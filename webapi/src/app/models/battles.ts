import { UnitsJSON } from './units';

export class BattlesJSON {
    key: string;
    action: string;
    from: string;
    to: string;
    departure: number;
    arrival: number;
	units: UnitsJSON[];
}