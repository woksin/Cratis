// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Sample.Accounts.Debit
{
    public record AccountsOverview(Guid Id, IEnumerable<DebitAccountSummary> DebitAccounts);
}
