namespace Cyh.WebServices.AppConfigs
{
    /// <summary>
    /// 顯示在前端的資訊選項
    /// </summary>
    public interface IViewSettings
    {
        string Title_CurrentUser { get; }
        string Title_Default { get; }
        bool Display_LoginState { get; }
        bool Display_AppTitle { get; }
        bool Display_ActionName { get; }
    }
}
