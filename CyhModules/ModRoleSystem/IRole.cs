namespace Cyh.Modules.ModRoleSystem
{
    /// <summary>
    /// 與角色有關的規則
    /// </summary>
    public interface IRole
    {
        /// <summary>
        /// 規則的ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 規則的名稱
        /// </summary>
        public string NAME { get; set; }
    }
}
