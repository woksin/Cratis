// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from 'Infrastructure/queries';
import { RecommendationInformation } from '../Cratis/Recommendations/RecommendationInformation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/events/store/{eventStore}/{namespace}/recommendations/observe/observe');

export interface AllRecommendationsArguments {
    eventStore: string;
    namespace: string;
}
export class AllRecommendations extends ObservableQueryFor<RecommendationInformation[], AllRecommendationsArguments> {
    readonly route: string = '/api/events/store/{eventStore}/{namespace}/recommendations/observe/observe';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: RecommendationInformation[] = [];

    constructor() {
        super(RecommendationInformation, true);
    }

    get requestArguments(): string[] {
        return [
            'eventStore',
            'namespace',
        ];
    }

    static use(args?: AllRecommendationsArguments): [QueryResultWithState<RecommendationInformation[]>] {
        return useObservableQuery<RecommendationInformation[], AllRecommendations, AllRecommendationsArguments>(AllRecommendations, args);
    }
}
