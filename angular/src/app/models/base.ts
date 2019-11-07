import { ResourcesJSON } from './resources';
import { StructuresJSON } from './structures';
import { UnitsJSON } from './units';
import { TaskJSON } from './task';

export class BaseJSON {
	baseid: number;
    name: string;
	owner: string;
	level: number;
	isactive: boolean;
	structures: StructuresJSON[];
	resources: ResourcesJSON[];
	units: UnitsJSON[];
	task: TaskJSON;
}