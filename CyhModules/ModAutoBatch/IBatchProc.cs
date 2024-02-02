using Cyh.DataModels;

namespace Cyh.Modules.ModAutoBatch
{
    public interface IBatchProc
    {
        /// <summary>
        /// 此排程的名稱
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 執行
        /// </summary>
        /// <returns>執行的結果</returns>
        public IDataTransResult Invoke(bool _forceExecute = false);

        /// <summary>
        /// 是否達到執行要求(時間或自訂條件等)
        /// </summary>
        /// <returns>是否可以執行</returns>
        public bool CanExecute();
    }
}
