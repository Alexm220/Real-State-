using RealState.Core.Entities;

namespace RealState.Core.Interfaces;

public interface IOwnerRepository
{
    Task<Owner?> GetOwnerByIdAsync(string id);
    Task<Owner?> GetOwnerByIdOwnerAsync(string idOwner);
    Task<Owner> CreateOwnerAsync(Owner owner);
    Task<Owner?> UpdateOwnerAsync(string id, Owner owner);
    Task<bool> DeleteOwnerAsync(string id);
}
