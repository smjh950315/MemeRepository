using Cyh.WebServices.AppConfigs;

namespace Cyh.WebServices.Controller
{
    /// <summary>
    /// 基本 Controller 的介面
    /// </summary>
    public partial interface IController : IControllerBase
    {
        public IWebAppConfigurations? _AppConfigurations { get; set; }

        /// <summary>
        /// 是否為登入狀態
        /// </summary>
        public bool IsLogin => this.IsAuthenticated();
    }
}
