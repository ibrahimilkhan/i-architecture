namespace Core.CrossCuttingConcerns.Localization;

public interface ISharedLocalizationService
{
    string this[string key] { get; }
}