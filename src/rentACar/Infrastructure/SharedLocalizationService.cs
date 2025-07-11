using Core.CrossCuttingConcerns.Localization;
using Microsoft.Extensions.Localization;

namespace Infrastucture
{
    public class SharedLocalizationService : ISharedLocalizationService
    {
        private readonly IStringLocalizer _localizer;

        public SharedLocalizationService(IStringLocalizerFactory factory)
        {
            var type = typeof(ResourceAnchor);
            _localizer = factory.Create("Messages", type.Assembly.GetName().Name);
        }

        public string this[string key] => _localizer[key];
    }
}