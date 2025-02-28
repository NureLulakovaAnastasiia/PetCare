using Microsoft.Extensions.Localization;

namespace WebPetCare.Resources
{
    public static class EnumLocalizationHelper
    {
        public static string GetLocalizedEnum<T>(this T enumValue, IStringLocalizer localizer) where T : Enum
        {
            string key = $"{enumValue}";
            string localizedValue = localizer[key];

            return string.IsNullOrEmpty(localizedValue) ? enumValue.ToString() : localizedValue;
        }
    }
}
