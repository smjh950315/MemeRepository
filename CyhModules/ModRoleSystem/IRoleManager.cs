using Cyh.Modules.ModViewData;

namespace Cyh.Modules.ModRoleSystem
{
    /// <summary>
    /// 權限驗證器，通常透過DB存取類別庫繼承
    /// </summary>
    public interface IRoleManager
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

    /// <summary>
    /// 權限驗證器，通常透過DB存取類別庫繼承
    /// </summary>
    public interface IRoleManager<TRoleData, TRoleView>
        : IRoleManager, IViewModelHelper<TRoleData, TRoleView>
        where TRoleData : class
        where TRoleView : class, IRole
    {
    }
}
