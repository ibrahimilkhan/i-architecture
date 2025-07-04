using Core.Application.Requests;
using Core.Application.Responses;
using MediatR;

namespace Application.Features.Brands.Queries.GetList;

public class GetListBrandQuery : IRequest <GetListResponse<GetListBrandListItemDto>>
{
    public PageRequest PageRequest { get; set; } = new();

}