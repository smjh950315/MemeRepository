using Cyh.Modules.ModAuthentication;

namespace WebMain.AppConfigs
{
    public class LoginModel : ILoginModel
    {
        public string? Account { get; set; }
        public string? Password { get; set; }
    }

    public class LoginOptions : ILoginOptions
    {
        public bool IsPersist => true;

        public bool AllowRefresh => true;

        public uint ExpireTime => 300;

        public ILoginModel LoginModel => new LoginModel();
    }
}
