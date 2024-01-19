using Cyh.Modules.ModAuthentication;
using Cyh.WebServices.AppConfigs;

namespace Cyh.WebServices.MyDefaults
{
    public class DefaultCookieAuthenticOptions : ICookieAuthenticOptions
    {
        public int AgeMinutes => 30;
        public string Name => "AppCookie";
        public string Path => "/";
        public bool HttpOnly => true;
    }

    public class DefaultLoginOptions : ILoginOptions
    {
        public bool IsPersist => true;
        public bool AllowRefresh => true;
        public uint ExpireTime => 30;

        public ILoginModel LoginModel => new DefaultLoginModel();
    }

    public class DefaultRouteSettings : IRouteSettings
    {
        public string Login => "/Account/Login";
        public string Logout => "/Account/Logout";
        public string AccessDenied => "/Account/AccessDenied";
        public string ReturnUrlParameter => "returnUrl";
        public string Error => "/Home/Error";
    }

    public class DefaultViewSettings : IViewSettings
    {
        public string Title_Default => "Untitle";
        public bool Display_LoginState => true;
        public bool Display_AppTitle => true;
    }
}
