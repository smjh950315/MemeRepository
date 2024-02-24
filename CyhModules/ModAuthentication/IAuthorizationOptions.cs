namespace Cyh.Modules.ModAuthentication
{
    public interface IAuthorizationOptions
    {
        /// <summary>
        /// 此認證選項的名稱
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 此認證的生命週期
        /// </summary>
        TimeSpan LifeTime { get; }

        /// <summary>
        /// 是否在每次呼叫時重置此認證的生命週期
        /// </summary>
        bool SlidingLiftTime { get; }

        /// <summary>
        /// 此認證是否允許更新
        /// </summary>
        bool AllowRefresh { get; }

        /// <summary>
        /// 此認證是否可以使用多次
        /// </summary>
        bool IsPersistent { get; set; }
    }
}
