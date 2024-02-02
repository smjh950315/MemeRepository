using Cyh.Common;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using static Cyh.ObjectHelper;

namespace Cyh
{
    /// <summary>
    /// 常用擴充方法
    /// </summary>
    public static partial class CommonExtends
    {
        private class _MinMaxTuple<T> where T : IComparable
        {
            internal readonly T? Min;
            internal readonly T? Max;
            internal readonly bool IsEqual;
            internal _MinMaxTuple(bool isEqual) {
                this.Min = default;
                this.Max = default;
                this.IsEqual = isEqual;
            }
            internal _MinMaxTuple(T? min, T? max) {
                this.Min = min;
                this.Max = max;
                this.IsEqual = false;
            }
        }
        private static _MinMaxTuple<T> _Get_MinMax<T>(T? lhs, T? rhs) where T : IComparable {
            if (lhs == null && rhs == null)
                throw new Exception("both inputs are null !");

            if (lhs == null)
                return new _MinMaxTuple<T>(lhs, rhs);

            else if (rhs == null)
                return new _MinMaxTuple<T>(rhs, lhs);

            int res = lhs.CompareTo(rhs);

            if (res == 0)
                return new _MinMaxTuple<T>(true);

            else if (res == -1)
                return new _MinMaxTuple<T>(lhs, rhs);

            return new _MinMaxTuple<T>(rhs, lhs);
        }

        /// <summary>
        /// 判斷是否為 null 或是空值
        /// </summary>
        /// <param name="values"></param>
        /// <returns>是否為 null 或是空值</returns> 
        public static bool IsNullOrEmpty([NotNullWhen(false)] this IEnumerable? values) {
            return values == null || !values.GetEnumerator().MoveNext();
        }

        /// <summary>
        /// 判斷是否為 null 或是空值
        /// </summary>
        /// <param name="values"></param>
        /// <returns>是否為 null 或是空值</returns> 
        public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? values) {
            return (values as IEnumerable).IsNullOrEmpty();
        }

        /// <summary>
        /// 驗證此物件是否是輸入集合中的一員
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">輸入集合</param>
        /// <returns>此物件是否是輸入集合中的一員</returns>
        public static bool IsAnyOf<T>(this T? obj, IEnumerable<T>? values) {
            return obj != null && !values.IsNullOrEmpty() && values.Contains(obj);
        }

        /// <summary>
        /// 驗證此物件是否是輸入集合中的一員
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">輸入集合</param>
        public static bool IsAnyOf<T>(this T? obj, params T[] values) {
            return obj.IsAnyOf(values as IEnumerable<T>);
        }

        /// <summary>
        /// 此物件是否在兩個輸入值的範圍中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static bool IsInRangeOf<T>(this T? obj, T? val1, T? val2) where T : IComparable {
            if (obj == null)
                return false;

            if (val1 == null && val2 == null)
                return false;

            _MinMaxTuple<T> minMaxVal = _Get_MinMax(val1, val2);

            if (minMaxVal.IsEqual)
                return obj.CompareTo(val1) == 0;

            return !(obj.CompareTo(minMaxVal.Min) < 0 || obj.CompareTo(minMaxVal.Max) > 0);
        }

        /// <summary>
        /// 此類型是否可以從來源類型賦值
        /// </summary>
        /// <param name="other">來源類型</param>
        public static bool CanAccept(this Type? type, Type? other) {
            return type != null && other != null && type.IsAssignableFrom(other);
        }

        /// <summary>
        /// 此類型是否可以從 <typeparamref name="T"/> 賦值
        /// </summary>
        /// <typeparam name="T">來源型別</typeparam>
        public static bool CanAccept<T>(this Type? type) {
            return CanAccept(type, typeof(T));
        }

        /// <summary>
        /// 當 bool 是空值，回傳特定結果
        /// </summary>
        /// <param name="val_if_null">null 時要回傳的結果</param>
        /// <returns>最終結果</returns>
        public static bool NullOr(this bool? val, bool val_if_null) {
            return val == null ? val_if_null : (bool)val;
        }

        /// <summary>
        /// 取得例外狀況的詳細資訊
        /// </summary>
        /// <param name="exception">內建的例外狀況</param>
        /// <returns>詳細資訊</returns>
        public static ExceptionDetails GetDetails(this Exception? exception) {
            return new(exception);
        }

        /// <summary>
        /// 比較時間在小時與分的部分是否相同
        /// </summary>
        /// <param name="other"></param>
        /// <returns>是否相同</returns>
        public static bool EqualInHourAndMinute(this DateTime self, DateTime other) {
            return self.Hour == other.Hour && self.Minute == other.Minute;
        }

        /// <summary>
        /// 取得與當前實體的小時以及分鐘相同但日期是今天的時間
        /// </summary>
        /// <returns>時間</returns>
        public static DateTime Today(this DateTime self) {
            DateTime now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, self.Hour, self.Minute, self.Millisecond, self.Kind);
        }

        /// <summary>
        /// 將物件內的成員以類似字典的形式儲存成資料集
        /// </summary>
        /// <param name="obj">要被擷取的物件</param>
        /// <returns>擷取的資料集</returns>
        public static MyDataSet GetMyDataSet(this object? obj, Expression<Func<string, bool>>? expression = null) {
            if (obj == null) return new();
            IEnumerable<MemberInfo> memberInfos;
            if (expression != null) {
                IEnumerable<MemberInfo> _memberInfos = GetMemberInfosOf(obj.GetType(), ComplexBindingFlags.Inst_ObjectType_Member);
                IEnumerable<string> memberNames = _memberInfos.Select(m => m.Name).AsQueryable().Where(expression);
                memberInfos = _memberInfos.Where(x => memberNames.Contains(x.Name));
            } else {
                memberInfos = GetMemberInfosOf(obj.GetType(), ComplexBindingFlags.Inst_ObjectType_Member);
            }
            if (!memberInfos.Any()) { return new(); }
            MyDataSet myDataSet = new();
            foreach (MemberInfo memberInfo in memberInfos) {
                try {
                    myDataSet[memberInfo.Name] = TryGetValue(obj, memberInfo);
                } catch { }
            }
            return myDataSet;
        }
    }
}
