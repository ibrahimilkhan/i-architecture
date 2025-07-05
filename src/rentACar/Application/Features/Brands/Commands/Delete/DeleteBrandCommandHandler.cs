using System;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.Brands.Commands.Delete;

public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, DeletedBrandResponse>
{
    private readonly IBrandRepository _repository;
    private readonly IMapper _mapper;

    public DeleteBrandCommandHandler(IBrandRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DeletedBrandResponse> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await _repository.GetAsync(x => x.Id == request.Id);
        await _repository.DeleteAsync(brand);

        var response = _mapper.Map<DeletedBrandResponse>(brand);
        return response;
    }
}