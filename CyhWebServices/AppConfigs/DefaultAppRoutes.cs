using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyh.WebServices.AppConfigs
{
    /// <summary>
    /// 專案的基本路徑
    /// </summary>
    public partial class DefaultAppRoutes : IRouteSettings
    {
        /// <summary>
        /// 登入路徑
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// 登出路徑
        /// </summary>
        public string Logout { get; set; }

        /// <summary>
        /// 拒絕存取時的路徑
        /// </summary>
        public string AccessDenied { get; set; }

        /// <summary>
        /// 返回網址的參數名稱
        /// </summary>
        public string ReturnUrlParameter { get; set; }

        /// <summary>
        /// 發生錯誤時的路徑
        /// </summary>
        public string Error { get; set; }
    }
}
