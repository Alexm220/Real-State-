using AutoMapper;
using RealState.Application.Interfaces;
using RealState.Core.DTOs;
using RealState.Core.Entities;
using RealState.Core.Interfaces;

namespace RealState.Application.Services;

public class OwnerService : IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;

    public OwnerService(IOwnerRepository ownerRepository, IMapper mapper)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
    }

    public async Task<OwnerDto?> GetOwnerByIdAsync(string id)
    {
        var owner = await _ownerRepository.GetOwnerByIdAsync(id);
        return owner != null ? _mapper.Map<OwnerDto>(owner) : null;
    }

    public async Task<OwnerDto?> GetOwnerByIdOwnerAsync(string idOwner)
    {
        var owner = await _ownerRepository.GetOwnerByIdOwnerAsync(idOwner);
        return owner != null ? _mapper.Map<OwnerDto>(owner) : null;
    }

    public async Task<OwnerDto> CreateOwnerAsync(OwnerDto ownerDto)
    {
        var owner = _mapper.Map<Owner>(ownerDto);
        owner.IdOwner = Guid.NewGuid().ToString();
        
        var createdOwner = await _ownerRepository.CreateOwnerAsync(owner);
        return _mapper.Map<OwnerDto>(createdOwner);
    }

    public async Task<OwnerDto?> UpdateOwnerAsync(string id, OwnerDto ownerDto)
    {
        var existingOwner = await _ownerRepository.GetOwnerByIdAsync(id);
        if (existingOwner == null)
            return null;

        var owner = _mapper.Map<Owner>(ownerDto);
        owner.Id = id;
        owner.CreatedAt = existingOwner.CreatedAt;

        var updatedOwner = await _ownerRepository.UpdateOwnerAsync(id, owner);
        return updatedOwner != null ? _mapper.Map<OwnerDto>(updatedOwner) : null;
    }

    public async Task<bool> DeleteOwnerAsync(string id)
    {
        return await _ownerRepository.DeleteOwnerAsync(id);
    }
}
