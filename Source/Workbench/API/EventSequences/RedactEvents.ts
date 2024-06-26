// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from 'Infrastructure';

import { Causation } from '../Auditing/Causation';
import { Identity } from '../Identities/Identity';

export class RedactEvents {

    @field(String)
    eventSourceId!: string;

    @field(String)
    reason!: string;

    @field(String, true)
    eventTypes!: string[];

    @field(Causation, true)
    causation!: Causation[];

    @field(Identity)
    causedBy!: Identity;
}
