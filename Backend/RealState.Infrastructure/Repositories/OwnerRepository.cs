using MongoDB.Driver;
using RealState.Core.Entities;
using RealState.Core.Interfaces;
using RealState.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace RealState.Infrastructure.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly IMongoCollection<Owner> _owners;

    public OwnerRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _owners = mongoDatabase.GetCollection<Owner>(mongoDbSettings.Value.OwnersCollectionName);
    }

    public async Task<Owner?> GetOwnerByIdAsync(string id)
    {
        return await _owners.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Owner?> GetOwnerByIdOwnerAsync(string idOwner)
    {
        return await _owners.Find(x => x.IdOwner == idOwner).FirstOrDefaultAsync();
    }

    public async Task<Owner> CreateOwnerAsync(Owner owner)
    {
        owner.CreatedAt = DateTime.UtcNow;
        owner.UpdatedAt = DateTime.UtcNow;
        await _owners.InsertOneAsync(owner);
        return owner;
    }

    public async Task<Owner?> UpdateOwnerAsync(string id, Owner owner)
    {
        owner.UpdatedAt = DateTime.UtcNow;
        var result = await _owners.ReplaceOneAsync(x => x.Id == id, owner);
        return result.MatchedCount > 0 ? owner : null;
    }

    public async Task<bool> DeleteOwnerAsync(string id)
    {
        var result = await _owners.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}
