namespace Cyh.WebServices.AppConfigs
{
    /// <summary>
    /// 顯示在前端的資訊選項
    /// </summary>
    public interface IViewSettings
    {
        public string Title_CurrentUser => "目前使用者: ";
        public string Title_Default { get; }
        public bool Display_LoginState { get; }
        public bool Display_AppTitle { get; }
        public bool Display_ActionName => false;
    }
}
