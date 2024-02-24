using Cyh.DataModels;
using Cyh.WebServices.Authentication;
using Microsoft.AspNetCore.Http;

namespace Cyh.WebServices.Prototypes.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public abstract class MyBaseFilter : Attribute
    {
        protected abstract HttpContext? HttpContext { get; set; }

        /// <summary>
        /// 取得僅設定了開始時間與當前執行者ID的空執行結果
        /// </summary>
        /// <returns>執行結果</returns>
        public IDataTransResult EmptyResult => new DataTransResultBase
        {
            BeginTime = DateTime.UtcNow,
            Accesser = this.HttpContext?.User.GetClientId() ?? "UNKNOWN"
        };
    }
}
