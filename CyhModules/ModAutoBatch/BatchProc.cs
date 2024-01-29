#pragma warning disable IDE1006 // 命名樣式
#pragma warning disable IDE0049 // 命名樣式
using Cyh.DataHelper;
using System.Diagnostics.CodeAnalysis;

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
    /// 排程執行規則
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

    public struct BatchInvokeRule
    {
        bool _RepeatOnFixedTime = false;
        readonly DateTime _RepeatFixedTime;
        readonly TimeSpan _RepeatTimeSpan;
        BATCH_INVOKE_ROLE _Role = BATCH_INVOKE_ROLE.UNDEFINED;


        private bool _CheckCanExecuteByTime(DateTime lastInvoked) {
            if (this._RepeatOnFixedTime) { // 定時執行 
                return this._RepeatFixedTime.Today() > lastInvoked;
            } else {
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
                if (this._RepeatOnFixedTime) {
                    return this._RepeatFixedTime.Today() < DateTime.Now;
                }
            }

            switch (this._Role) {
                case BATCH_INVOKE_ROLE.UNDEFINED:
                    throw new NotImplementedException("Undifined batch invoke role!");
                case BATCH_INVOKE_ROLE.INVOKE_ONCE:
                    return !isInvoked;
                case BATCH_INVOKE_ROLE.INVOKE_BY_LOOP:
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
        private List<IDataTransResult> _ExecLogs = new List<IDataTransResult>();

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
            if (this.ForceExecuteOnce)
                return true;

            if (!this.ExecuteStatus.IsInvoked())
                return true;

            if (!this.AllowRepeat)
                return false;

            if (this.IsExecuting)
                return false;

            if (this.RepeatOnlyFailed)
                return !this.ExecuteStatus.IsSucceed();

            return true;
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
        /// 排程被呼叫的時間
        /// </summary>
        public DateTime InvokedTime { get; private set; }

        /// <summary>
        /// 排程執行完成的時間
        /// </summary>
        public DateTime FinishedTime { get; private set; }

        /// <summary>
        /// 此排程的狀態
        /// </summary>
        public BATCH_STATUS ExecuteStatus { get; private set; }

        /// <summary>
        /// 此排程是否正在執行
        /// </summary>
        public bool IsExecuting { get; private set; }

        /// <summary>
        /// 是否只有在失敗才允許重複執行
        /// </summary>
        public bool RepeatOnlyFailed { get; set; }

        /// <summary>
        /// 是否允許重複執行
        /// </summary>
        public bool AllowRepeat { get; set; }

        /// <summary>
        /// 此排程的名稱
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 此排程的描述
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// 執行
        /// </summary>
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

        public BatchProc(
            String name,
            bool _allowRepeat = false,
            bool _repeatOnlyFailed = true,
            string? description = null
            ) {
            this.Name = name;
            this.Description = description;
            this.AllowRepeat = _allowRepeat;
            this.RepeatOnlyFailed = _repeatOnlyFailed;
        }
    }
}
