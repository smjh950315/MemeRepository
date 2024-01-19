namespace Cyh.Modules.ModIdentity
{
    /// <summary>
    /// 使用者資料的基本模型
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// 使用者的ID
        /// </summary>
        long ID { get; set; }

        /// <summary>
        /// 使用者的名稱
        /// </summary>
        string NAME { get; set; }

        /// <summary>
        /// 使用者的帳號
        /// </summary>
        string ACCOUNT { get; set; }

        /// <summary>
        /// 使用者的密碼
        /// </summary>
        string? PASSWORD { get; set; }

        /// <summary>
        /// 使用者的權限ID
        /// </summary>
        long? ROLE_ID { get; set; }
    }
}
