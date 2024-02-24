using Cyh.DataModels;
using Cyh.WebServices.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cyh.WebServices.Prototypes.Filters
{
    public abstract class MyAuthorizationBaseFilter : MyBaseFilter, IAuthorizationFilter
    {
        protected override HttpContext? HttpContext { get; set; }

        /// <summary>
        /// 是否不檢查，直接允許所有人使用(主要用來Debug)
        /// </summary>
        protected virtual bool AllowAll { get; set; }

        /// <summary>
        /// 允許的權限名稱集合
        /// </summary>
        protected abstract List<string> AllowedRoles { get; }

        public void OnAuthorization(AuthorizationFilterContext context) {
            if (this.AllowAll) { return; }

            this.HttpContext = context.HttpContext;

            IEnumerable<string> roles = context.HttpContext.User.GetClientRoles();
            foreach (string role in roles) {
                if (this.AllowedRoles.Contains(role)) { return; }
            }

            IDataTransResult result = this.EmptyResult;
            result.TryAppendError_RequireAuthorization();
            result.BatchOnFinish(false);

            context.Result = new JsonResult(result);
        }
    }
}
