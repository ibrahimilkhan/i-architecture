using Application.Features.Brands.Constants;
using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Localization;
using Domain.Entities;

namespace Application.Features.Brands.Rules;

public class BrandBusinessRules
{
    private readonly IBrandRepository _brandRepository;
    private readonly ISharedLocalizationService _sharedLocalizationService;

    public BrandBusinessRules(IBrandRepository brandRepository, ISharedLocalizationService sharedLocalizationService)
    {
        _brandRepository = brandRepository;
        _sharedLocalizationService = sharedLocalizationService;
    }

    public async Task BrandNameCannotBeDuplicatedWhenInserted(string name)
    {
        Brand? existingBrand = await _brandRepository.GetAsync(b => b.Name.ToLower() == name.ToLower());

        if (existingBrand != null)
        {
            var x = _sharedLocalizationService["BrandNameExists"];

        }
    }
}