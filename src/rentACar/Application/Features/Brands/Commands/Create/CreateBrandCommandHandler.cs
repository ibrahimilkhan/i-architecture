using Application.Features.Brands.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;

namespace Application.Features.Brands.Commands.Create;

public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandResponse>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;

    public CreateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
    }

    public async Task<CreatedBrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {

        Brand brand = _mapper.Map<Brand>(request);
        brand.Id = Guid.NewGuid();

        var result = await _brandRepository.AddAsync(brand);
        CreatedBrandResponse response = _mapper.Map<CreatedBrandResponse>(result);

        return response;
    }
}