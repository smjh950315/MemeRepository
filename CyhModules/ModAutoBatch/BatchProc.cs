using Cyh.DataModels;

namespace Cyh.Modules.ModAutoBatch
{
    /// <summary>
    /// 排程狀態
    /// </summary>
    public enum BATCH_STATUS : uint
    {
        UNKNOWN = 0,
        IS_INVOKED = 1,
        IS_SUCCEED = 1 << 1,
    }

    /// <summary>
    /// 排程執行規則的FLAG
    /// </summary>
    public enum BATCH_INVOKE_ROLE
    {
        /// <summary>
        /// 未定義
        /// </summary>
        UNDEFINED,

        /// <summary>
        /// 只執行一次
        /// </summary>
        INVOKE_ONCE,

        /// <summary>
        /// 循環執行
        /// </summary>
        INVOKE_BY_LOOP,
    }

    public static class ExecStatusExtends
    {
        private static bool Has(this BATCH_STATUS status, BATCH_STATUS valid_status) {
            return (status & valid_status) != 0;
        }

        /// <summary>
        /// 排程是否已被發起
        /// </summary>
        /// <param name="status">排程狀態</param>
        /// <returns>排程是否已被發起</returns>
        public static bool IsInvoked(this BATCH_STATUS status) {
            return status.Has(BATCH_STATUS.IS_INVOKED);
        }

        /// <summary>
        /// 排程是否已執行成功
        /// </summary>
        /// <param name="status">排程狀態</param>
        /// <returns>排程是否已執行成功</returns>
        public static bool IsSucceed(this BATCH_STATUS status) {
            return status.Has(BATCH_STATUS.IS_SUCCEED);
        }
    }

    /// <summary>
    /// 排程執行規則
    /// </summary>
    public struct BatchInvokeRule
    {
        bool _RepeatOnFixedTime = false;
        readonly DateTime _RepeatFixedTime;
        readonly TimeSpan _RepeatTimeSpan;
        BATCH_INVOKE_ROLE _Role = BATCH_INVOKE_ROLE.UNDEFINED;

        /// <summary>
        /// 在有允許多次執行的情況下檢查是否該立即執行
        /// </summary>
        /// <param name="lastInvoked">最後執行的時間</param>
        /// <returns>是否該立即執行</returns>
        private bool _CheckCanExecuteByTime(DateTime lastInvoked) {
            if (this._RepeatOnFixedTime) {
                // 定時執行，如果今天應該要實行的時間晚於最後執行的時間就立即執行
                return this._RepeatFixedTime.Today() > lastInvoked;
            } else {
                // 定期執行，如果上次到現在的時間間隔大於設定的間隔就立即執行
                return DateTime.Now - lastInvoked > _RepeatTimeSpan;
            }
        }

        /// <summary>
        /// 排程規則的建構子
        /// </summary>
        /// <param name="role">排程執行規則</param>
        /// <param name="repeatFixedTime">要定時執行的時間</param>
        public BatchInvokeRule(BATCH_INVOKE_ROLE role, DateTime repeatFixedTime) {
            this._Role = role;
            this._RepeatFixedTime = repeatFixedTime;
            this._RepeatOnFixedTime = true;
            this._RepeatTimeSpan = TimeSpan.Zero;
        }

        /// <summary>
        /// 排程規則的建構子
        /// </summary>
        /// <param name="role">排程執行規則</param>
        /// <param name="repeatTimeSpan">要循環執行的間隔</param>
        public BatchInvokeRule(BATCH_INVOKE_ROLE role, TimeSpan repeatTimeSpan) {
            this._Role = role;
            this._RepeatFixedTime = DateTime.MinValue;
            this._RepeatTimeSpan = repeatTimeSpan;
            this._RepeatOnFixedTime = false;
        }

        /// <summary>
        /// 檢查輸入條件判斷是否可以執行，注意! 如果回傳True，則需要更新該任務的時間及狀態
        /// </summary>
        /// <param name="isInvoked">是否已經執行</param>
        /// <param name="isSucceed">是否成功</param>
        /// <param name="lastInvoked">上次執行的時間</param>
        /// <returns>可否立刻執行</returns>
        public bool CanExecute(bool isInvoked, bool isSucceed, DateTime lastInvoked) {
            if (this._Role == BATCH_INVOKE_ROLE.INVOKE_ONCE && !isInvoked) {
                // 只執行一次且還沒被執行
                if (this._RepeatOnFixedTime) {
                    // 定時執行，如果當前時間早於設定時間，就立刻執行
                    return this._RepeatFixedTime.Today() > DateTime.Now;
                }
            }

            switch (this._Role) {
                case BATCH_INVOKE_ROLE.UNDEFINED:
                    throw new NotImplementedException("Undifined batch invoke role!");
                case BATCH_INVOKE_ROLE.INVOKE_ONCE:
                    // 只執行一次，如果還沒執行就立即執行
                    return !isInvoked;
                case BATCH_INVOKE_ROLE.INVOKE_BY_LOOP:
                    // 多次執行，額外判斷是否該立即執行
                    return this._CheckCanExecuteByTime(lastInvoked);
                default:
                    throw new NotImplementedException("Undifined batch invoke role!");
            }
        }
    }

    /// <summary>
    /// 排程批次
    /// </summary>
    public abstract class BatchProc : IBatchProc
    {
        private bool _ForceExecuteOnce = false;
        private BatchInvokeRule _BatchInvokeRule;
        private List<IDataTransResult> _ExecLogs = new List<IDataTransResult>();

        /// <summary>
        /// 是否已經設定要強迫執行一次(透過 Invoke (bool) 設定)，如果設定過 TRUE，回傳一次 TRUE後，下次呼叫會變回 FALSE
        /// </summary>
        private bool ForceExecuteOnce {
            get {
                if (this._ForceExecuteOnce) {
                    this._ForceExecuteOnce = false;
                    return true;
                } else { return false; }
            }
        }
        private void _OnExecute() {
            this.InvokedTime = DateTime.Now;
            this.ExecuteStatus |= BATCH_STATUS.IS_INVOKED;
            this.IsExecuting = true;
        }
        private void _OnSucceed() {
            this.FinishedTime = DateTime.Now;
            this.ExecuteStatus |= BATCH_STATUS.IS_SUCCEED;
            this.IsExecuting = false;
        }
        private void _OnFailed() {
            this.FinishedTime = DateTime.Now;
            this.ExecuteStatus &= ~BATCH_STATUS.IS_SUCCEED;
            this.IsExecuting = false;
        }
        private bool _CanExecute() {
            return this.ForceExecuteOnce
            || this._BatchInvokeRule.CanExecute(this.IsInvoked, this.IsSucceed, this.InvokedTime);
        }
        private IDataTransResult _NoStatusCheck_RunProgram() {
            this._OnExecute();
            try {
                IDataTransResult result = this.Execute();
                this._OnSucceed();
                this._ExecLogs.Add(result);
                return result;
            } catch (Exception ex) {
                CommonLib.HandleException(ex);
                this._OnFailed();
                var result = new DataTransResultBase
                {
                    BeginTime = this.InvokedTime,
                    EndTime = this.FinishedTime
                };
                this._ExecLogs.Add(result);
                return result;
            }
        }

        /// <summary>
        /// 未初始化的執行結果
        /// </summary>
        public static IDataTransResult EmptyResult => new DataTransResultBase();

        /// <summary>
        /// 此排程的狀態
        /// </summary>
        public BATCH_STATUS ExecuteStatus { get; private set; }

        /// <summary>
        /// 排程被呼叫的時間
        /// </summary>
        public DateTime InvokedTime { get; private set; }

        /// <summary>
        /// 排程執行完成的時間
        /// </summary>
        public DateTime FinishedTime { get; private set; }

        /// <summary>
        /// 是否執行過
        /// </summary>
        public bool IsInvoked => this.ExecuteStatus.IsInvoked();

        /// <summary>
        /// 是否成功執行
        /// </summary>
        public bool IsSucceed => this.ExecuteStatus.IsSucceed();

        /// <summary>
        /// 此排程是否正在執行
        /// </summary>
        public bool IsExecuting { get; private set; }

        /// <summary>
        /// 是否在失敗時重複執行
        /// </summary>
        public bool RepeatOnFailed { get; private set; }

        /// <summary>
        /// 此排程的名稱
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 此排程的描述
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// 執行
        /// </summary>
        /// <param name="_forceExecute">是否強迫執行</param>
        /// <returns>執行的結果</returns>
        public IDataTransResult Invoke(bool _forceExecute = false) {
            this._ForceExecuteOnce = _forceExecute;
            return this._CanExecute()
                ? this._NoStatusCheck_RunProgram()
                : EmptyResult;
        }

        /// <summary>
        /// 是否達到執行要求(時間或自訂條件等)
        /// </summary>
        /// <returns>是否可以執行</returns>
        public virtual bool CanExecute() {
            // 若開啟"失敗時重新執行"且已經執行但是尚未成功的情況下，設定強迫執行
            if (this.RepeatOnFailed && this.IsInvoked && !this.IsSucceed)
                this._ForceExecuteOnce = true;
            return this._CanExecute();
        }

        /// <summary>
        /// 要執行的函數(複寫)
        /// </summary>
        /// <returns>執行的結果</returns>
        public abstract IDataTransResult Execute();

        /// <summary>
        /// 取得執行LOG
        /// </summary>
        /// <returns>執行LOG</returns>
        public IEnumerable<IDataTransResult> GetInvokedLog() {
            return this._ExecLogs;
        }

        /// <summary>
        /// 排程程式的建構子
        /// </summary>
        /// <param name="name">排程的名稱</param>
        /// <param name="rule">排程執行規則</param>
        /// <param name="_repeatOnFailed">失敗時重新執行</param>
        /// <param name="description">描述</param>
        public BatchProc(
            String name,
            BatchInvokeRule rule,
            bool _repeatOnFailed = true,
            string? description = null
            ) {
            this._BatchInvokeRule = rule;
            this.Name = name;
            this.Description = description;
            this.RepeatOnFailed = _repeatOnFailed;
        }
    }
}
