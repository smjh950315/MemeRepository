namespace Cyh
{
    public static partial class CommonFuncType
    {
        public delegate T FnGetValue<T>(params object?[] objs);
        public delegate void FnNoReturn(params object?[] objs);
        public delegate void FnNoReturn<T>(ref T? _obj, params object?[] objs);
        public delegate void FnHandleException(Exception? ex, params object?[] args);
        public delegate TDst? FnConvert<TDst, TSrc>(TSrc? src);
        public delegate TDst? FnConvert<TDst>(object? src);
        public delegate bool FnIsValid(object? src);
    }
}
