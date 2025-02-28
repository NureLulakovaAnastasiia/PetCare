using Syncfusion.Blazor;

namespace WebPetCare.Resources
{
    public class SyncfusionLocalizer : ISyncfusionStringLocalizer
    {
        public string GetText(string key)
        {
            return this.ResourceManager.GetString(key);
        }

        public System.Resources.ResourceManager ResourceManager
        {
            get
            {
                return WebPetCare.Resources.SfResources.ResourceManager;

            }
        }
    }
}
