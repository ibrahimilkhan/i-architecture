using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Dynamic;
using MediatR;

namespace Application.Features.Models.Queries.GetListByDynamic;

public class GetListByDynamicQuery : IRequest<GetListResponse<GetListByDynamicModelListItemDto>>
{
    public PageRequest PageRequest { get; set; } = new();
    public DynamicQuery DynamicQuery { get; set; }
}