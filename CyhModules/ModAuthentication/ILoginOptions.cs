namespace Cyh.Modules.ModAuthentication
{
    /// <summary>
    /// 與登入有關的選項
    /// </summary>
    public interface ILoginOptions
    {
        /// <summary>
        /// 是否記住此認證
        /// </summary>
        public bool IsPersist { get; }

        /// <summary>
        /// 是否允許F5
        /// </summary>
        public bool AllowRefresh { get; }

        /// <summary>
        /// 認證有效期限
        /// </summary>
        public uint ExpireTime { get; }

        /// <summary>
        /// 登入用的 Model
        /// </summary>
        public ILoginModel LoginModel { get; }
    }
}
