// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.MongoDB;
using MongoDB.Driver;

namespace Aksio.Cratis.Compliance.MongoDB
{
    /// <summary>
    /// Represents an implementation of <see cref="IEncryptionKeyStore"/> for MongoDB.
    /// </summary>
    public class MongoDBEncryptionKeyStore : IEncryptionKeyStore
    {
        readonly IMongoCollection<EncryptionKeyForIdentifier> _encryptionKeysCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBEncryptionKeyStore"/> class.
        /// </summary>
        /// <param name="database"><see cref="ISharedDatabase"/> to use for accessing database.</param>
        public MongoDBEncryptionKeyStore(ISharedDatabase database)
        {
            _encryptionKeysCollection = database.GetCollection<EncryptionKeyForIdentifier>("encryption-keys");
        }

        /// <inheritdoc/>
        public async Task SaveFor(EncryptionKeyIdentifier identifier, EncryptionKey key)
        {
            await _encryptionKeysCollection.ReplaceOneAsync(
                _ => _.Identifier == identifier,
                new EncryptionKeyForIdentifier(identifier, key.Public, key.Private),
                new ReplaceOptions() { IsUpsert = true });
        }

        /// <inheritdoc/>
        public async Task<bool> HasFor(EncryptionKeyIdentifier identifier)
        {
            var result = await _encryptionKeysCollection.CountDocumentsAsync(_ => _.Identifier == identifier);
            return result == 1;
        }

        /// <inheritdoc/>
        public async Task<EncryptionKey> GetFor(EncryptionKeyIdentifier identifier)
        {
            var result = await _encryptionKeysCollection.FindAsync(_ => _.Identifier == identifier);
            var key = result.SingleOrDefault();
            ThrowIfMissingEncryptionKey(identifier, key);
            return new(key.PublicKey, key.PrivateKey);
        }

        /// <inheritdoc/>
        public async Task DeleteFor(EncryptionKeyIdentifier identifier) => await _encryptionKeysCollection.DeleteOneAsync(_ => _.Identifier == identifier);

        void ThrowIfMissingEncryptionKey(EncryptionKeyIdentifier identifier, EncryptionKeyForIdentifier key)
        {
            if (key == default)
            {
                throw new MissingEncryptionKey(identifier);
            }
        }
    }
}
