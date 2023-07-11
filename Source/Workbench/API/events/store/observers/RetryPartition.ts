/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/applications/commands';
import { Validator } from '@aksio/applications/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/events/store/{{microserviceId}}/observers/{{observerId}}/failed-partitions/{{tenantId}}/retry/{{partitionId}}');

export interface IRetryPartition {
    microserviceId?: string;
    tenantId?: string;
    observerId?: string;
    partitionId?: string;
}

export class RetryPartitionValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        microserviceId: new Validator(),
        tenantId: new Validator(),
        observerId: new Validator(),
        partitionId: new Validator(),
    };
}

export class RetryPartition extends Command<IRetryPartition> implements IRetryPartition {
    readonly route: string = '/api/events/store/{{microserviceId}}/observers/{{observerId}}/failed-partitions/{{tenantId}}/retry/{{partitionId}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new RetryPartitionValidator();

    private _microserviceId!: string;
    private _tenantId!: string;
    private _observerId!: string;
    private _partitionId!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
            'microserviceId',
            'tenantId',
            'observerId',
            'partitionId',
        ];
    }

    get properties(): string[] {
        return [
            'microserviceId',
            'tenantId',
            'observerId',
            'partitionId',
        ];
    }

    get microserviceId(): string {
        return this._microserviceId;
    }

    set microserviceId(value: string) {
        this._microserviceId = value;
        this.propertyChanged('microserviceId');
    }
    get tenantId(): string {
        return this._tenantId;
    }

    set tenantId(value: string) {
        this._tenantId = value;
        this.propertyChanged('tenantId');
    }
    get observerId(): string {
        return this._observerId;
    }

    set observerId(value: string) {
        this._observerId = value;
        this.propertyChanged('observerId');
    }
    get partitionId(): string {
        return this._partitionId;
    }

    set partitionId(value: string) {
        this._partitionId = value;
        this.propertyChanged('partitionId');
    }

    static use(initialValues?: IRetryPartition): [RetryPartition, SetCommandValues<IRetryPartition>, ClearCommandValues] {
        return useCommand<RetryPartition, IRetryPartition>(RetryPartition, initialValues);
    }
}