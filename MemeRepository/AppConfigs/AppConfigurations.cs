using Cyh.Modules.ModAuthentication;
using Cyh.WebServices.AppConfigs;

namespace MemeRepository.AppConfigs
{
    public class AppConfigurations : AppConfigurationBase
    {
        public override string ApplicationName => "ApiTest";
        public override ILoginOptions LoginOptions => new LoginOptions();
    }
}