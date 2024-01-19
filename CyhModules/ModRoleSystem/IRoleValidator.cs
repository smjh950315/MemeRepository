namespace Cyh.Modules.ModRoleSystem
{
    /// <summary>
    /// 授權驗證的介面
    /// </summary>
    public interface IRoleValidator
    {
        /// <summary>
        /// 以 代號(或是ID) 尋找相關授權資料
        /// </summary>
        /// <param name="_id">規則ID</param>
        /// <returns>找到的規則</returns>
        public IRole? FindRoleByID(string? _id);

        /// <summary>
        /// 以名稱尋找相關授權資料
        /// </summary>
        /// <param name="_roleName">規則名稱</param>
        /// <returns>找到的規則</returns>
        public IRole? FindRoleByName(string? _roleName);
    }
}
