using Cyh.DataModels;
using Cyh.WebServices.AppConfigs;

namespace Cyh.WebServices.Controller
{
    public abstract class MyControllerBase : Microsoft.AspNetCore.Mvc.Controller, IController
    {
        public IWebAppConfigurations? _AppConfigurations { get; set; }
        public IController BaseController => this;
        public virtual bool IsLogin => this.IsAuthenticated();
        public IDataTransResult EmptyResult => new DataTransResultBase()
        {
            BeginTime = DateTime.Now,
            Accesser = this.GetClientId() ?? "UNKNOW",
        };
        public IDataTransResult NoLoginResult => new DataTransResultBase()
        {
            BeginTime = DateTime.Now,
            Accesser = this.GetClientId() ?? "UNKNOW",
            Message = "尚未登入"
        };

        public MyControllerBase(IWebAppConfigurations webAppConfigurations) {
            this._AppConfigurations = webAppConfigurations;
        }
    }
}
