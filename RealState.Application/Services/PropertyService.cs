using AutoMapper;
using RealState.Application.Interfaces;
using RealState.Core.DTOs;
using RealState.Core.Entities;
using RealState.Core.Interfaces;

namespace RealState.Application.Services;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;

    public PropertyService(
        IPropertyRepository propertyRepository,
        IOwnerRepository ownerRepository,
        IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _ownerRepository = ownerRepository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<PropertyDto>> GetPropertiesAsync(PropertyFilterDto filter)
    {
        var propertiesResult = await _propertyRepository.GetPropertiesAsync(filter);
        
        var propertyDtos = new List<PropertyDto>();
        
        foreach (var property in propertiesResult.Items)
        {
            // Load images for each property
            var images = await _propertyRepository.GetPropertyImagesAsync(property.IdProperty);
            property.Images = images;
            
            // Load owner information
            var owner = await _ownerRepository.GetOwnerByIdOwnerAsync(property.IdOwner);
            property.Owner = owner;
            
            var propertyDto = _mapper.Map<PropertyDto>(property);
            propertyDtos.Add(propertyDto);
        }

        return new PagedResultDto<PropertyDto>
        {
            Items = propertyDtos,
            TotalCount = propertiesResult.TotalCount,
            Page = propertiesResult.Page,
            PageSize = propertiesResult.PageSize
        };
    }

    public async Task<PropertyDetailDto?> GetPropertyByIdAsync(string id)
    {
        var property = await _propertyRepository.GetPropertyByIdAsync(id);
        if (property == null)
            return null;

        // Load related data
        var images = await _propertyRepository.GetPropertyImagesAsync(property.IdProperty);
        var traces = await _propertyRepository.GetPropertyTracesAsync(property.IdProperty);
        var owner = await _ownerRepository.GetOwnerByIdOwnerAsync(property.IdOwner);

        property.Images = images;
        property.Traces = traces;
        property.Owner = owner;

        return _mapper.Map<PropertyDetailDto>(property);
    }

    public async Task<PropertyDto> CreatePropertyAsync(PropertyDto propertyDto)
    {
        var property = _mapper.Map<Property>(propertyDto);
        property.IdProperty = Guid.NewGuid().ToString();
        
        var createdProperty = await _propertyRepository.CreatePropertyAsync(property);
        return _mapper.Map<PropertyDto>(createdProperty);
    }

    public async Task<PropertyDto?> UpdatePropertyAsync(string id, PropertyDto propertyDto)
    {
        var existingProperty = await _propertyRepository.GetPropertyByIdAsync(id);
        if (existingProperty == null)
            return null;

        var property = _mapper.Map<Property>(propertyDto);
        property.Id = id;
        property.CreatedAt = existingProperty.CreatedAt;

        var updatedProperty = await _propertyRepository.UpdatePropertyAsync(id, property);
        return updatedProperty != null ? _mapper.Map<PropertyDto>(updatedProperty) : null;
    }

    public async Task<bool> DeletePropertyAsync(string id)
    {
        return await _propertyRepository.DeletePropertyAsync(id);
    }
}
