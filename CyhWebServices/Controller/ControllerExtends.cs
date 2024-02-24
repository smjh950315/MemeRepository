using Cyh.DataModels;
using Cyh.WebServices.Authentication;
using System.Security.Claims;
using System.Text.Json;

namespace Cyh.WebServices.Controller
{
    static partial class ControllerExtends
    {
        internal static ClaimsPrincipal? _User(this IControllerBase? controller) {
            return controller == null ? null : controller.User;
        }

        /// <summary>
        /// 取得Claim集合
        /// </summary>
        /// <returns>Claim集合</returns>
        public static IReadOnlyClaimList GetClaims(this IControllerBase? controller) {
            return new ClaimList(controller._User()?.Claims);
        }

        /// <summary>
        /// 取得當前客戶端的識別ID
        /// </summary>
        /// 當前客戶端的識別ID，如果未設定，回傳null
        public static string? GetClientId(this IControllerBase? controller) {
            return controller._User().GetClientId();
        }

        /// <summary>
        /// 取得當前客戶端的權限名稱集合
        /// </summary>
        /// <returns>當前客戶端的權限名稱集合</returns>
        public static IEnumerable<string> GetClientRoles(this IControllerBase? controller) {
            return controller._User().GetClientRoles();
        }

        /// <summary>
        /// 當前的客戶端是否已授權
        /// </summary>
        public static bool IsAuthenticated(this IControllerBase? controller) {
            if (controller == null) { return false; }
            if (controller.User == null) { return false; }
            if (controller.User.Identity == null) { return false; }
            return controller.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// 嘗試將輸入可能為JSON的物件轉成某個MODEL
        /// </summary>
        /// <typeparam name="TModel">要轉的MODEL型別</typeparam>
        /// <param name="json">可能為JSON的物件</param>
        /// <returns>取得的MODEL，如果失敗回傳NULL</returns>
        public static TModel? JsonDeserialize<TModel>(this IControllerBase? controller, object? json) where TModel : class {
            if (json == null) { return null; }
            if (json is TModel model) { return model; }
            string? rawJsonString;
            if (json is string str) {
                rawJsonString = str;
            } else {
                rawJsonString = json.ToString() ?? String.Empty;
            }
            return TryGetValue(fn => JsonSerializer.Deserialize<TModel>(rawJsonString), null);
        }

        /// <summary>
        /// 嘗試將MODEL字串化
        /// </summary>
        /// <typeparam name="TModel">來源的MODEL型別</typeparam>
        /// <param name="model">來源的MODEL</param>
        /// <returns>字串化的JSON物件</returns>
        public static string JsonSerialize<TModel>(this IControllerBase? controller, TModel model) where TModel : class {
            if (model == null) { return String.Empty; }
            return TryGetValue(fn => JsonSerializer.Serialize(model), null) ?? String.Empty;
        }

        /// <summary>
        /// 取得僅設定了開始時間與當前執行者ID的空執行結果
        /// </summary>
        /// <returns>執行結果</returns>
        public static IDataTransResult GetEmptyResult(this IControllerBase? controller) {
            return new DataTransResultBase
            {
                BeginTime = DateTime.UtcNow,
                Accesser = controller.GetClientId() ?? "UNKNOWN"
            };
        }

        /// <summary>
        /// 取得因為"需要認證"而失敗的交易結果
        /// </summary>
        /// <returns>失敗的交易結果</returns>
        public static IDataTransResult GetLoginRequiredResult(this IControllerBase? controller, string? message = null) {
            IDataTransResult result = controller.GetEmptyResult();

            result.TryAppendError_RequireAuthorization();
            if (!message.IsNullOrEmpty()) { result.Message += message; }
            result.BatchOnFinish(false);

            return result;
        }
    }
}
