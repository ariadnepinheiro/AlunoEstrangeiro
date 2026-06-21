namespace Techne.Web
{
    public class HadesSiteMap
    {
        public static void RefreshSiteMap()
        {
            HadesSiteMapProvider.ClearAllCaches();
        }
    }
}