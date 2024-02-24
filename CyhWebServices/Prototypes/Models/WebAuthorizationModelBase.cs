using Cyh.Common;
using Cyh.Modules.ModAuthentication;

namespace Cyh.WebServices.Prototypes.Models
{
    public class WebAuthorizationModelBase : IAuthorizationModel
    {
        public string? Account { get; set; }
        public string? Password { get; set; }

        /// <summary>
        /// 取得準備認證用的資料集
        /// </summary>
        /// <returns>準備認證用的資料集</returns>
        public MyDataSet GetValidationDataSet() {
            MyDataSet dataSet = new MyDataSet();
            dataSet["Account"] = this.Account;
            dataSet["Password"] = this.Password;
            return dataSet;
        }
    }
}
