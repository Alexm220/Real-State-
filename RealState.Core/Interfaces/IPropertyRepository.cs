using RealState.Core.DTOs;
using RealState.Core.Entities;

namespace RealState.Core.Interfaces;

public interface IPropertyRepository
{
    Task<PagedResultDto<Property>> GetPropertiesAsync(PropertyFilterDto filter);
    Task<Property?> GetPropertyByIdAsync(string id);
    Task<Property> CreatePropertyAsync(Property property);
    Task<Property?> UpdatePropertyAsync(string id, Property property);
    Task<bool> DeletePropertyAsync(string id);
    Task<List<PropertyImage>> GetPropertyImagesAsync(string propertyId);
    Task<List<PropertyTrace>> GetPropertyTracesAsync(string propertyId);
}
