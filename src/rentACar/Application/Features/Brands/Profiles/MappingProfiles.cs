using Application.Features.Brands.Commands.Create;
using Domain.Entities;

namespace Application.Features.Brands.Profiles
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
        {
            CreateMap<Brand, CreateBrandCommand>()
                .ReverseMap();

            CreateMap<Brand, CreatedBrandResponse>()
                .ReverseMap();
        }
    }
}