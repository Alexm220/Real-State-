using RealState.Core.DTOs;

namespace RealState.Application.Interfaces;

public interface IOwnerService
{
    Task<OwnerDto?> GetOwnerByIdAsync(string id);
    Task<OwnerDto?> GetOwnerByIdOwnerAsync(string idOwner);
    Task<OwnerDto> CreateOwnerAsync(OwnerDto ownerDto);
    Task<OwnerDto?> UpdateOwnerAsync(string id, OwnerDto ownerDto);
    Task<bool> DeleteOwnerAsync(string id);
}
