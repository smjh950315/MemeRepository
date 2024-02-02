namespace Cyh.WebServices.AppConfigs
{
    /// <summary>
    /// 專案的基本路徑
    /// </summary>
    public interface IRouteSettings
    {
        /// <summary>
        /// 登入路徑
        /// </summary>
        public string Login => "/Account/Login";

        /// <summary>
        /// 登出路徑
        /// </summary>
        public string Logout => "/Account/Logout";

        /// <summary>
        /// 拒絕存取時的路徑
        /// </summary>
        public string AccessDenied { get; }

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
