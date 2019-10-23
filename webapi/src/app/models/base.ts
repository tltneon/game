import { ResourcesJSON } from './resources';
import { StructuresJSON } from './structures';
import { UnitsJSON } from './units';

export class BaseJSON {
    name: string;
    level: number;
	structures: StructuresJSON;
	resources: ResourcesJSON;
	units: UnitsJSON;
}