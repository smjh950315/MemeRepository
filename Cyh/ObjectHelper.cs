using System.Reflection;

namespace Cyh
{
    /// <summary>
    /// 物件操作的 Helper
    /// </summary>
    public static partial class ObjectHelper
    {
        delegate T? FnForceCast<T>(object? obj);

        class MemberInfoPair
        {
            public MemberInfo SrcMemberInfo { get; set; }
            public MemberInfo DstMemberInfo { get; set; }
            public MemberInfoPair(MemberInfo dstMemberInfo, MemberInfo srcMemberInfo) {
                this.SrcMemberInfo = srcMemberInfo;
                this.DstMemberInfo = dstMemberInfo;
            }
        }

        private static T? _Impl_Force_Cast<T>(object? obj) {
            try {
                return (T?)obj;
            } catch {
                return default;
            }
        }

        private static IEnumerable<MemberInfo> _Remove_Reflection_MemberInfos(MemberInfo[] infos) {
            return infos.IsNullOrEmpty()
                ? Enumerable.Empty<MemberInfo>()
                : infos.Where(m => m.MemberType != MemberTypes.Method && !m.Name.StartsWith(".c")).AsEnumerable();
        }

        internal delegate MemberInfo[] FnGetMemberInfo(object? _Obj);

        private static IEnumerable<MemberInfo> _Get_ObjectType_MemberInfos(Type? type, BindingFlags _flag, string? name = null) {
            if (type == null)
                return Enumerable.Empty<MemberInfo>();

            MemberInfo[] members = name.IsNullOrEmpty() ? type.GetMembers(_flag) : type.GetMember(name, _flag);

            return _Remove_Reflection_MemberInfos(members);
        }

        private static IEnumerable<MethodInfo> _Get_MethodType_MemberInfos(Type? type, BindingFlags _flag, string? name = null) {
            if (type == null)
                return Enumerable.Empty<MethodInfo>();

            if (name.IsNullOrEmpty())
                return type.GetMethods(_flag);

            MethodInfo? method = type.GetMethod(name, _flag);

            return method == null ? Enumerable.Empty<MethodInfo>() : new MethodInfo[] { method };
        }

        private static object? _GetValue_FromMemberInfo(object? obj, MemberInfo? info) {
            if (obj == null || info == null)
                return null;

            try {
                if (info is PropertyInfo propertyInfo)
                    return propertyInfo.GetValue(obj);
                else if (info is FieldInfo fieldInfo)
                    return fieldInfo.GetValue(obj);
                else
                    return null;
            } catch {
                return null;
            }
        }

        private static bool _SetValue_ToMemberInfo(object? obj, MemberInfo? info, object? value) {
            if (obj == null || info == null)
                return false;

            try {
                if (info is PropertyInfo propertyInfo)
                    propertyInfo.SetValue(obj, value);
                else if (info is FieldInfo fieldInfo)
                    fieldInfo.SetValue(obj, value);
                else
                    return false;
                return true;
            } catch {
                return false;
            }
        }

        private static IEnumerable<string> _GetMemberNames(Type? tp, BindingFlags _flag) {
            if (tp == null)
                return Enumerable.Empty<string>();
            return _Get_ObjectType_MemberInfos(tp, _flag).Select(x => x.Name);
        }

        private static MethodInfo? _GetConvertMethod_FormDotNetBuiltIn(Type? dstType, Type? srcType) {
            if (dstType == null || srcType == null)
                return null;

            IEnumerable<MethodInfo> methodInfos = _Get_MethodType_MemberInfos(typeof(Convert), ComplexBindingFlags.Type_MethodType_Member | BindingFlags.NonPublic);

            IEnumerable<MethodInfo> method_Sort1 = methodInfos
                .Where(
                x => x.Name.StartsWith("To")
                && x.ReturnType == dstType
                && x.GetParameters().Length == 1);

            IEnumerable<MethodInfo> method_Sort2 = method_Sort1
                .Where(
                x => x.GetParameters().First().ParameterType == srcType
                && x.GetParameters().First().ParameterType == typeof(object));

            return method_Sort2.Any() ? method_Sort2.First() : null;
        }

        private static MethodInfo _GetConvertMethod_ForceCast<T>() {
            return ((FnForceCast<T>)_Impl_Force_Cast<T>).GetMethodInfo();
        }

        /// <summary>
        /// 複合的BindingFlags
        /// </summary>
        public class ComplexBindingFlags
        {
            // 不是函數 的成員
            public const BindingFlags ObjectType_Member = BindingFlags.Public;
            // 函數類型 的成員
            public const BindingFlags MethodType_Member = BindingFlags.Public | BindingFlags.InvokeMethod;

            // 實體中 不是函數 的成員
            public const BindingFlags Inst_ObjectType_Member = ObjectType_Member | BindingFlags.Instance;
            // 實體中 函數類型 的成員
            public const BindingFlags Inst_MethodType_Member = MethodType_Member | BindingFlags.Instance;

            // 靜態 不是函數 的成員
            public const BindingFlags Type_ObjectType_Member = ObjectType_Member | BindingFlags.Static;
            // 靜態 函數類型 的成員
            public const BindingFlags Type_MethodType_Member = MethodType_Member | BindingFlags.Static;
        }

        /// <summary>
        /// 取得符合特定 .NET 運行時連結狀態 物件類型的成員資訊
        /// </summary>
        /// <param name="tp">要分析的型別</param>
        /// <param name="flags">.NET 運行時連結狀態</param>
        /// <param name="name">尋找的成員名稱</param>
        /// <returns>符合條件的成員資訊</returns>
        public static IEnumerable<MemberInfo> GetMemberInfosOf(Type? tp, BindingFlags flags, string? name = null) {
            return _Get_ObjectType_MemberInfos(tp, flags, name);
        }

        /// <summary>
        /// 取得符合特定 .NET 運行時連結狀態 方法類型的成員資訊
        /// </summary>
        /// <param name="tp">要分析的型別</param>
        /// <param name="flag">.NET 運行時連結狀態</param>
        /// <param name="name">尋找的成員名稱</param>
        /// <returns>符合條件的成員資訊</returns>
        public static IEnumerable<MethodInfo> GetMethodInfosOf(Type? tp, BindingFlags flag, string? name = null) {
            return _Get_MethodType_MemberInfos(tp, flag, name);
        }

        /// <summary>
        /// 取得符合特定 .NET 運行時連結狀態 物件類型的成員資訊
        /// </summary>
        /// <typeparam name="T">要分析的型別</typeparam>
        /// <param name="flags">.NET 運行時連結狀態</param>
        /// <param name="name">尋找的成員名稱</param>
        /// <returns>符合條件的成員資訊</returns>
        public static IEnumerable<MemberInfo> GetMemberInfosOf<T>(BindingFlags flags, string? name = null) {
            return _Get_ObjectType_MemberInfos(typeof(T), flags, name);
        }

        /// <summary>
        /// 取得符合特定 .NET 運行時連結狀態 方法類型的成員資訊
        /// </summary>
        /// <typeparam name="T">要分析的型別</typeparam>
        /// <param name="flag">.NET 運行時連結狀態</param>
        /// <param name="name">尋找的成員名稱</param>
        /// <returns>符合條件的成員資訊</returns>
        public static IEnumerable<MethodInfo> GetMethodInfosOf<T>(BindingFlags flag, string? name = null) {
            return _Get_MethodType_MemberInfos(typeof(T), flag, name);
        }

        /// <summary>
        /// 取得某物件實體化後，特定內部成員名稱的值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="memberName"></param>
        /// <returns>符合條件的成員的值</returns>
        public static object? GetInstanceMemberValue(object? instance, string? memberName) {
            if (instance == null)
                return null;

            IEnumerable<MemberInfo> memberInfos = GetMemberInfosOf(instance.GetType(), ComplexBindingFlags.Inst_ObjectType_Member, memberName);

            MemberInfo? memberInfo = memberInfos.FirstOrDefault();

            return CommonLib.TryGetValue(fn => _GetValue_FromMemberInfo(instance, memberInfo), null);
        }

        /// <summary>
        /// 設定某物件實體化後，內部成員名稱與輸入相同的值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="memberName"></param>
        /// <param name="value"></param>
        /// <returns>是否成功設定</returns>
        public static bool SetInstanceMemberValue(object? instance, string? memberName, object? value) {
            if (instance == null || memberName.IsNullOrEmpty())
                return false;

            IEnumerable<MemberInfo> memberInfos = GetMemberInfosOf(instance.GetType(), ComplexBindingFlags.Inst_ObjectType_Member, memberName);

            MemberInfo? memberInfo = memberInfos.FirstOrDefault();

            return _SetValue_ToMemberInfo(instance, memberInfo, value);
        }

        /// <summary>
        /// 嘗試用型別 T 取得物件 obj 內成員名稱為 name 的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="_name"></param>
        /// <returns>取得的物件</returns>
        public static T? TryGetValueAs<T>(object? obj, string? name) {
            return (T?)GetInstanceMemberValue(obj, name);
        }

        /// <summary>
        /// 嘗試將 obj 內名稱為 name 的成員的值設定為 value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns>是否成功設定</returns>
        public static bool TrySetValue(object? obj, string? name, object? value) {
            return SetInstanceMemberValue(obj, name, value);
        }

        /// <summary>
        /// 嘗試轉換物件
        /// </summary>
        /// <typeparam name="T">目標型別</typeparam>
        /// <param name="value">輸入物件</param>
        /// <returns>轉換後的物件</returns>
        public static T? ConvertTo<T>(object? value) {
            if (value == null)
                return default;

            if (typeof(T).GetType().IsAssignableFrom(value.GetType()))
                return (T?)value;

            try {
                MethodInfo? builtInMethod = _GetConvertMethod_FormDotNetBuiltIn(typeof(T), value.GetType());
                if (builtInMethod != null)
                    return (T?)builtInMethod.Invoke(null, new object[] { value });

                MethodInfo? forceCastMethod = _GetConvertMethod_ForceCast<T>();
                if (forceCastMethod != null)
                    return (T?)forceCastMethod.Invoke(null, new object[] { value });

                return default;
            } catch {
                return default;
            }
        }

        /// <summary>
        /// 將 <paramref name="src"/> 轉型為 <typeparamref name="TDst"/> 並將 <paramref name="dst"/> 設定為該值
        /// </summary>
        /// <typeparam name="TDst"></typeparam>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        /// <returns>是否成功設定</returns>
        public static bool TryConvert<TDst, TSrc>(ref TDst? dst, TSrc? src) {
            if (src == null)
                return false;

            dst = ConvertTo<TDst>(src);

            return dst != null;
        }

        /// <summary>
        /// 嘗試轉換成目標型別，但是以成員對成員的方式一一設定內容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? CastTo<T>(object? value) where T : class, new() {
            if (value == null)
                return default;

            if (typeof(T).GetType().IsAssignableFrom(value.GetType()))
                return (T?)value;

            try {
                BindingFlags memberFlags = ComplexBindingFlags.Inst_ObjectType_Member | BindingFlags.NonPublic | BindingFlags.Public;
                T res = new T();
                Type valType = value.GetType();

                var srcMemberInfos = _Get_ObjectType_MemberInfos(valType, memberFlags);
                var dstMemberInfos = _Get_ObjectType_MemberInfos(typeof(T), memberFlags);
                var srcueMembers = srcMemberInfos.Select(x => x.Name);
                var resMembers = dstMemberInfos.Select(x => x.Name);

                var memberInfoPairs = new List<MemberInfoPair>();

                foreach (var dstMemberInfo in dstMemberInfos) {
                    foreach (var srcMemberInfo in srcMemberInfos) {
                        if (dstMemberInfo.Name == srcMemberInfo.Name) {
                            memberInfoPairs.Add(new(dstMemberInfo, srcMemberInfo));
                            break;
                        }
                    }
                }

                foreach (var memberInfoPair in memberInfoPairs) {
                    _SetValue_ToMemberInfo(
                        res,
                        memberInfoPair.DstMemberInfo,
                        _GetValue_FromMemberInfo(value, memberInfoPair.SrcMemberInfo));
                }

                return res;
            } catch {
                return default;
            }
        }

        /// <summary>
        /// 取得物件內的成員名稱清單
        /// </summary>
        /// <typeparam name="T">要分析的型別</typeparam>
        /// <returns>成員名稱清單</returns>
        public static List<string> GetMemberList<T>() {
            return _GetMemberNames(typeof(T), ComplexBindingFlags.Inst_ObjectType_Member | BindingFlags.Static).ToList();
        }

        /// <summary>
        /// 取得實體化物件內的成員名稱清單
        /// </summary>
        /// <typeparam name="T">要分析的型別</typeparam>
        /// <returns>成員名稱清單</returns>
        public static List<string> GetInstanceMemberList<T>() {
            return _GetMemberNames(typeof(T), ComplexBindingFlags.Inst_ObjectType_Member).ToList();
        }

        /// <summary>
        /// 取得物件內的靜態成員名稱清單
        /// </summary>
        /// <typeparam name="T">要分析的型別</typeparam>
        /// <returns>成員名稱清單</returns>
        public static List<string> GetStaticMemberList<T>() {
            return _GetMemberNames(typeof(T), ComplexBindingFlags.Type_ObjectType_Member).ToList();
        }
    }

}
