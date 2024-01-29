using Cyh.DataHelper;

namespace Cyh.Modules.ModAutoBatch
{
    /// <summary>
    /// 排程批次管理器
    /// </summary>
    public class BatchProcHandler : IBatchProcHandler
    {
        List<IBatchProc>? _Processes;

        public IEnumerable<IDataTransResult> Execute(string batchName) {
            if (this._Processes.IsNullOrEmpty())
                return Enumerable.Empty<IDataTransResult>();

            IEnumerable<IBatchProc>? processHandlers = null;

            if (batchName.IsNullOrEmpty()) {
                processHandlers = this._Processes;
            } else {
                processHandlers = this._Processes.Where(p => p.Name == batchName);
            }

            if (processHandlers.Any()) {
                List<IDataTransResult> results = new List<IDataTransResult>();
                foreach (IBatchProc batch in processHandlers) {
                    results.Add(batch.Invoke());
                }
                return results;
            }
            return Enumerable.Empty<IDataTransResult>();
        }

        public void Pushback(IBatchProc? batch) {
            if (batch == null) return;
            this._Processes ??= new();
            this._Processes.Add(batch);
        }
    }
}
