using Cyh.Modules.ModAuthentication;

namespace Cyh.WebServices.AppConfigs
{
    /// <summary>
    /// 基本的 App 相關設定
    /// </summary>
    public interface IWebAppConfigurations
    {
        public string ApplicationName { get; }
        public ICookieAuthenticOptions CookieAuthenticOptions => new MyDefaults.DefaultCookieAuthenticOptions();
        public ILoginOptions LoginOptions => new MyDefaults.DefaultLoginOptions();
        public IRouteSettings RouteSettings => new MyDefaults.DefaultRouteSettings();
        public IViewSettings ViewSettings => new MyDefaults.DefaultViewSettings();
    }
}
