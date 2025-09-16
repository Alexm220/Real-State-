using AutoMapper;
using RealState.Core.DTOs;
using RealState.Core.Entities;

namespace RealState.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Property mappings
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => 
                src.Images.FirstOrDefault(i => i.Enabled)!.File ?? string.Empty));

        CreateMap<Property, PropertyDetailDto>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => 
                src.Images.FirstOrDefault(i => i.Enabled)!.File ?? string.Empty));

        CreateMap<PropertyDto, Property>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Owner, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Traces, opt => opt.Ignore());

        // Owner mappings
        CreateMap<Owner, OwnerDto>();
        CreateMap<OwnerDto, Owner>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Property Image mappings
        CreateMap<PropertyImage, PropertyImageDto>();

        // Property Trace mappings
        CreateMap<PropertyTrace, PropertyTraceDto>();
    }
}
