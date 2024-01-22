using System.Text;

namespace Cyh.Common
{
    /// <summary>
    /// 例外的詳細資訊
    /// </summary>
    public class ExceptionDetails
    {
        string? _Message;
        string? _Source;
        string? _StackTrace;
        List<string>? _Stacks;

        /// <summary>
        /// 例外狀態產生的訊息
        /// </summary>
        public string Message {
            get => this._Message ?? String.Empty;
            set => this._Message = value;
        }

        /// <summary>
        /// 例外發生的來源
        /// </summary>
        public string Source {
            get => this._Source ?? String.Empty;
            set => this._Source = value;
        }

        /// <summary>
        /// 例外狀況的堆疊追蹤
        /// </summary>
        public string StackTrace {
            get => this._StackTrace ?? String.Empty;
            set => this._StackTrace = value;
        }

        /// <summary>
        /// 例外狀況的堆疊追蹤(LIST形式)
        /// </summary>
        public List<string> Stacks {
            get {
                this._Stacks ??= new();
                if (!this.StackTrace.IsNullOrEmpty()) {
                    var array = this.StackTrace.Split("\n   at ");
                    this._Stacks.AddRange(array);
                }
                return this._Stacks;
            }
        }

        public ExceptionDetails() { }

        public ExceptionDetails(Exception? exception) {
            this._Message = exception?.Message;
            this._Source = exception?.Source;
            this._StackTrace = exception?.StackTrace;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"[Message] {this.Message}");
            sb.AppendLine($"[Source] {this.Source}");
            sb.AppendLine($"[StackTrace]\n{this.StackTrace}");
            return sb.ToString();
        }
    }
}
