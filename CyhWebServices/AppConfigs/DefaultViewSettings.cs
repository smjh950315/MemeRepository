namespace Cyh.WebServices.AppConfigs
{
    public class DefaultViewSettings : IViewSettings
    {
        private string? _Title_CurrentUser;
        private string? _Title_Default;

        public string Title_CurrentUser {
            get => this._Title_CurrentUser ?? "Current:";
            set => this._Title_CurrentUser = value;
        }

        public string Title_Default {
            get => this._Title_Default ?? "Untitled";
            set => this._Title_Default = value;
        }

        public bool Display_LoginState { get; set; }

        public bool Display_AppTitle { get; set; }

        public bool Display_ActionName { get; set; }
    }
}
