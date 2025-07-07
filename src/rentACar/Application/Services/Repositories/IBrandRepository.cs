using Core.Persistence.Repositories;
using Domain.Entities;
using System;

namespace Application.Services.Repositories;

public interface IBrandRepository : IAsyncRepository<Brand, Guid>
{

}