using Cyh.Modules.ModAuthentication;

namespace Cyh.WebServices.AppConfigs
{
    /// <summary>
    /// 基本的 App 相關設定
    /// </summary>
    public interface IWebAppConfigurations
    {
        /// <summary>
        /// 應用程式的名稱
        /// </summary>
        public string ApplicationName { get; }

        /// <summary>
        /// 基本的 Cookie 認證選項
        /// </summary>
        public ICookieAuthenticOptions CookieAuthenticOptions { get; }

        /// <summary>
        /// 與登入有關的選項
        /// </summary>
        public IMyAuthorizationOptions LoginOptions { get; }

        /// <summary>
        /// 專案的基本路徑
        /// </summary>
        public IRouteSettings RouteSettings { get; }

        /// <summary>
        /// 顯示在前端的資訊選項
        /// </summary>
        public IViewSettings ViewSettings { get; }
    }
}
