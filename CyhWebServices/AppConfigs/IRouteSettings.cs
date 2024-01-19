namespace Cyh.WebServices.AppConfigs
{
    /// <summary>
    /// 專案的基本路徑
    /// </summary>
    public interface IRouteSettings
    {
        public string Login => "Account/Login";
        public string Logout => "Account/Logout";
        public string AccessDenied { get; }
        public string ReturnUrlParameter { get; }
        public string Error { get; }
    }
}
