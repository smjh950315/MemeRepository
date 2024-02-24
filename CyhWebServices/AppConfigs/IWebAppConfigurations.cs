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
        /// 返回網址的參數名稱
        /// </summary>
        public string ReturnUrlParameter { get; }

        /// <summary>
        /// 發生錯誤時的路徑
        /// </summary>
        public string Error { get; }
    }
}
