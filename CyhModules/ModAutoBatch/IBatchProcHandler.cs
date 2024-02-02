using Cyh.DataModels;

namespace Cyh.Modules.ModAutoBatch
{
    /// <summary>
    /// 排程批次管理介面
    /// </summary>
    public interface IBatchProcHandler
    {
        /// <summary>
        /// 執行指定名稱的排程
        /// </summary>
        /// <param name="batchName">排程名稱，如果空白則執行所有排程</param>
        /// <returns>排程執行的結果</returns>
        IEnumerable<IDataTransResult> Execute(string? batchName, bool _forceRun = false);

        /// <summary>
        /// 加入排程
        /// </summary>
        /// <param name="batch">要加入的排程</param>
        void Pushback(IBatchProc? batch);
    }
}
