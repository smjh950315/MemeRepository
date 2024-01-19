namespace Cyh.Modules.ModAuthentication
{
    /// <summary>
    /// 登入用的基本介面
    /// </summary>
    public interface ILoginModel
    {
        /// <summary>
        /// 帳號或是使用者名稱
        /// </summary>
        public string? Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string? Password { get; set; }
    }
}
