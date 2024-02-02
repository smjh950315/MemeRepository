namespace Cyh.WebServices.AppConfigs
{
    /// <summary>
    /// 專案的基本路徑
    /// </summary>
    public partial class DefaultAppRoutes : IRouteSettings
    {
        private string? _Login;
        private string? _Logout;
        private string? _AccessDenied;
        private string? _ReturnUrlParameter;
        private string? _Error;

        /// <summary>
        /// 登入路徑
        /// </summary>
        public string Login {
            get => this._Login ?? "/Account/Login";
            set => this._Login = value;
        }

        /// <summary>
        /// 登出路徑
        /// </summary>
        public string Logout {
            get => this._Logout ?? "/Account/Logout";
            set => this._Logout = value;
        }

        /// <summary>
        /// 拒絕存取時的路徑
        /// </summary>
        public string AccessDenied {
            get => this._AccessDenied ?? "/Account/AccessDenied";
            set => this._AccessDenied = value;
        }

        /// <summary>
        /// 返回網址的參數名稱
        /// </summary>
        public string ReturnUrlParameter {
            get => this._ReturnUrlParameter ?? "returnUrl";
            set => this._ReturnUrlParameter = value;
        }

        /// <summary>
        /// 發生錯誤時的路徑
        /// </summary>
        public string Error {
            get => this._Error ?? "/Home/Error";
            set => this._Error = value;
        }
    }
}
