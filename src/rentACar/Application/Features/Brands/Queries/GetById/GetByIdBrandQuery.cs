﻿using MediatR;

namespace Application.Features.Brands.Queries.GetById;

public class GetByIdBrandQuery : IRequest<GetByIdBrandResponse>
{
    public Guid Id { get; set; }
    public GetByIdBrandQuery(Guid id)
    {
        Id = id;
    }
}