using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Responses;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Models.Queries.GetListByDynamic;

public class GetListByDynamicQueryHandler : IRequestHandler<GetListByDynamicQuery, GetListResponse<GetListByDynamicModelListItemDto>>
{
    private readonly IModelRepository _modelRepository;
    private readonly IMapper _mapper;

    public GetListByDynamicQueryHandler(IModelRepository modelRepository, IMapper mapper)
    {
        _modelRepository = modelRepository;
        _mapper = mapper;
    }
    public async Task<GetListResponse<GetListByDynamicModelListItemDto>> Handle(GetListByDynamicQuery request, CancellationToken cancellationToken)
    {
        Paginate<Model> models = await _modelRepository.GetListDynamicAsync(
             request.DynamicQuery,
             include: m => m.Include(m => m.Brand).Include(m => m.Fuel).Include(m => m.Transmission),
             index: request.PageRequest.PageIndex,
             size: request.PageRequest.PageSize
             );

        var response = _mapper.Map<GetListResponse<GetListByDynamicModelListItemDto>>(models);

        return response;


    }
}