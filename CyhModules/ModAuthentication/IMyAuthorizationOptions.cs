namespace Cyh.Modules.ModAuthentication
{
    /// <summary>
    /// 與登入有關的選項
    /// </summary>
    public interface IMyAuthorizationOptions
    {
        /// <summary>
        /// 是否記住此認證
        /// </summary>
        bool IsPersist { get; }

        /// <summary>
        /// 是否允許F5
        /// </summary>
        bool AllowRefresh { get; }

        /// <summary>
        /// 認證有效期限
        /// </summary>
        uint ExpireTime { get; }

        /// <summary>
        /// 登入用的 Model
        /// </summary>
        ILoginModel LoginModel { get; }
    }
}
