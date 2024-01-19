using Cyh.Modules.ModAuthentication;

namespace Cyh.WebServices.MyDefaults
{
    public partial class DefaultLoginModel : ILoginModel
    {
        public string? Account { get; set; }
        public string? Password { get; set; }
    }
}
