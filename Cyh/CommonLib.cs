using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace Cyh
{
    public static partial class CommonLib
    {
        /// <summary>
        /// 對內建 Exception 要處理的方法
        /// </summary>
        public static CommonFuncType.FnHandleException? FuncHandleException;

        /// <summary>
        /// 對內建 Exception 要處理的方法，可以透過 FuncHandleException 設定
        /// </summary>
        /// <param name="ex">Exception 物件</param>
        internal static void HandleException(Exception? ex) {
            if (ex != null && FuncHandleException != null) { FuncHandleException(ex); }
        }

        /// <summary>
        /// 嘗試用輸入的函數取得回傳值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_fn">要執行的函數</param>
        /// <param name="_return_if_exception">發生例外時要回傳的值</param>
        /// <param name="_objs">其餘的參數</param>
        /// <returns>返回的值</returns>
        public static T? TryGetValue<T>(CommonFuncType.FnGetValue<T> _fn, T? _return_if_exception, params object?[] _objs) {
            try {
                return _fn(_objs);
            } catch (Exception ex) {
                HandleException(ex);
                return _return_if_exception;
            }
        }

        /// <summary>
        /// 嘗試執行輸入的函數
        /// </summary>
        /// <param name="_fn">要執行的函數</param>
        /// <param name="_objs">其餘的參數</param>
        /// <returns>是否發生例外</returns>
        public static bool TryExecute(CommonFuncType.FnNoReturn _fn, params object?[] _objs) {
            try {
                _fn(_objs);
                return true;
            } catch (Exception ex) {
                HandleException(ex);
                return false;
            }
        }

        /// <summary>
        /// 驗證輸入是否包含空值
        /// </summary>
        /// <param name="_objs">驗證的資料</param>
        /// <returns>是否包含空值</returns>
        public static bool HasNull([NotNullWhen(false)] params object?[] _objs) {
            if (_objs == null) 
                return true;
            foreach (var obj in _objs) {
                if (obj == null)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 嘗試將輸入的 json 資料轉成指定 model
        /// </summary>
        /// <typeparam name="T">指定的型別</typeparam>
        /// <param name="json">可能是 json 的資料</param>
        /// <param name="caseSenstive">大小寫敏感</param>
        /// <returns>轉換結果，如果失敗返回 null</returns>
        public static T? GetJsonModel<T>(object? json, bool caseSenstive = false) {
            string? jsonStr = json?.ToString();
            if (jsonStr.IsNullOrEmpty())
                return default;
            try {
                return JsonSerializer.Deserialize<T>(jsonStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = !caseSenstive });
            } catch (Exception) {
                return default;
            }
        }

        /// <summary>
        /// 將多個物件字串化加在一起並用第一個輸入作為符號隔離
        /// </summary>
        /// <param name="seperator">隔離符號</param>
        /// <param name="objects">輸入物件</param>
        /// <returns></returns>
        public static string MakeStringWithSeperator(string seperator, params object?[] objects) {
            if (objects.IsNullOrEmpty())
                return String.Empty;

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < objects.Length; ++i) {
                stringBuilder.Append(objects[i]?.ToString());

                if (i != objects.Length - 1)
                    stringBuilder.Append(seperator ?? String.Empty);
            }

            return stringBuilder.ToString();
        }
    }
}
