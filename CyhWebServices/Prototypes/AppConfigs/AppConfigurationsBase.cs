using Cyh.WebServices.AppConfigs;

namespace Cyh.WebServices.Prototypes.AppConfigs
{
    public abstract class AppConfigurationsBase : IWebAppConfigurations
    {
        private string? _ReturnUrlParameter;
        private string? _Error;

        public abstract string ApplicationName { get; }

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
