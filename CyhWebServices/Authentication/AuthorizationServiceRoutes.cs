namespace Cyh.WebServices.Authentication
{
    /// <summary>
    /// 與授權服務有關的路徑
    /// </summary>
    public class AuthorizationServiceRoutes
    {
        private string? _Login;
        private string? _Logout;
        private string? _AccessDenied;

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
    }
}
