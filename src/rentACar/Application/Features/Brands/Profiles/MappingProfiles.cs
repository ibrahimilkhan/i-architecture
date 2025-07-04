using Application.Features.Brands.Commands.Create;
using Application.Features.Brands.Queries.GetList;
using Core.Application.Responses;
using Core.Persistence.Paging;
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

            CreateMap<Brand, GetListBrandListItemDto>()
                .ReverseMap();

            CreateMap<Paginate<Brand>, GetListResponse<GetListBrandListItemDto>>()
                .ReverseMap();
        }
    }
}