using System;
using MediatR;

namespace Application.Brands.Commands.Create;

public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandResponse>
{
    public Task<CreatedBrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        CreatedBrandResponse response = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        return Task.FromResult(response);
    }
}