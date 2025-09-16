using MongoDB.Driver;
using RealState.Core.DTOs;
using RealState.Core.Entities;
using RealState.Core.Interfaces;
using RealState.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace RealState.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<Property> _properties;
    private readonly IMongoCollection<PropertyImage> _propertyImages;
    private readonly IMongoCollection<PropertyTrace> _propertyTraces;

    public PropertyRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);

        _properties = mongoDatabase.GetCollection<Property>(mongoDbSettings.Value.PropertiesCollectionName);
        _propertyImages = mongoDatabase.GetCollection<PropertyImage>(mongoDbSettings.Value.PropertyImagesCollectionName);
        _propertyTraces = mongoDatabase.GetCollection<PropertyTrace>(mongoDbSettings.Value.PropertyTracesCollectionName);
    }

    public async Task<PagedResultDto<Property>> GetPropertiesAsync(PropertyFilterDto filter)
    {
        var filterBuilder = Builders<Property>.Filter.Empty;

        if (!string.IsNullOrEmpty(filter.Name))
        {
            filterBuilder &= Builders<Property>.Filter.Regex(x => x.Name, 
                new MongoDB.Bson.BsonRegularExpression(filter.Name, "i"));
        }

        if (!string.IsNullOrEmpty(filter.Address))
        {
            filterBuilder &= Builders<Property>.Filter.Regex(x => x.Address, 
                new MongoDB.Bson.BsonRegularExpression(filter.Address, "i"));
        }

        if (filter.MinPrice.HasValue)
        {
            filterBuilder &= Builders<Property>.Filter.Gte(x => x.Price, filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            filterBuilder &= Builders<Property>.Filter.Lte(x => x.Price, filter.MaxPrice.Value);
        }

        var totalCount = await _properties.CountDocumentsAsync(filterBuilder);

        var properties = await _properties
            .Find(filterBuilder)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Limit(filter.PageSize)
            .ToListAsync();

        return new PagedResultDto<Property>
        {
            Items = properties,
            TotalCount = (int)totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<Property?> GetPropertyByIdAsync(string id)
    {
        return await _properties.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Property> CreatePropertyAsync(Property property)
    {
        property.CreatedAt = DateTime.UtcNow;
        property.UpdatedAt = DateTime.UtcNow;
        await _properties.InsertOneAsync(property);
        return property;
    }

    public async Task<Property?> UpdatePropertyAsync(string id, Property property)
    {
        property.UpdatedAt = DateTime.UtcNow;
        var result = await _properties.ReplaceOneAsync(x => x.Id == id, property);
        return result.MatchedCount > 0 ? property : null;
    }

    public async Task<bool> DeletePropertyAsync(string id)
    {
        var result = await _properties.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<List<PropertyImage>> GetPropertyImagesAsync(string propertyId)
    {
        return await _propertyImages
            .Find(x => x.IdProperty == propertyId && x.Enabled)
            .ToListAsync();
    }

    public async Task<List<PropertyTrace>> GetPropertyTracesAsync(string propertyId)
    {
        return await _propertyTraces
            .Find(x => x.IdProperty == propertyId)
            .SortByDescending(x => x.DateSale)
            .ToListAsync();
    }
}
