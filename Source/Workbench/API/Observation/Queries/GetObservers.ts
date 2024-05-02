/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from 'Infrastructure/queries';
import { ObserverInformation } from '../../Users/einari/Projects/Cratis/Cratis/Source/Tools/ProxyGenerator/Cratis/Kernel/Contracts/Observation/ObserverInformation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/events/store/{eventStore}/{namespace}/observers');

export interface GetObserversArguments {
    eventStore: string;
    namespace: string;
}
export class GetObservers extends QueryFor<ObserverInformation[], GetObserversArguments> {
    readonly route: string = '/api/events/store/{eventStore}/{namespace}/observers';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ObserverInformation[] = [];

    constructor() {
        super(ObserverInformation, true);
    }

    get requestArguments(): string[] {
        return [
            'eventStore',
            'namespace',
        ];
    }

    static use(args?: GetObserversArguments): [QueryResultWithState<ObserverInformation[]>, PerformQuery<GetObserversArguments>] {
        return useQuery<ObserverInformation[], GetObservers, GetObserversArguments>(GetObservers, args);
    }
}
