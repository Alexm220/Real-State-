using RealState.Core.DTOs;

namespace RealState.Application.Interfaces;

public interface IPropertyService
{
    Task<PagedResultDto<PropertyDto>> GetPropertiesAsync(PropertyFilterDto filter);
    Task<PropertyDetailDto?> GetPropertyByIdAsync(string id);
    Task<PropertyDto> CreatePropertyAsync(PropertyDto propertyDto);
    Task<PropertyDto?> UpdatePropertyAsync(string id, PropertyDto propertyDto);
    Task<bool> DeletePropertyAsync(string id);
}
