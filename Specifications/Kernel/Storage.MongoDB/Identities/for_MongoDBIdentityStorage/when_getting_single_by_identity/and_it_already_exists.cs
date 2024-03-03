// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Identities;

namespace Cratis.Kernel.Storage.MongoDB.Identities.for_MongoDBIdentityStorage.when_getting_single_by_identity;

public class and_it_already_exists : given.two_identities_registered
{
    Identity identity;
    IdentityId identityId;

    void Establish() => identity = new Identity(first_identity_from_database.Subject, first_identity_from_database.Name, first_identity_from_database.UserName);

    async Task Because() => identityId = await store.GetSingleFor(identity);

    [Fact] void should_return_an_id() => identityId.ShouldNotBeNull();
    [Fact] void should_not_insert_the_identity() => inserted_identities.Count.ShouldEqual(0);
    [Fact] void should_return_the_correct_identity() => identityId.ShouldEqual(first_identity);
}