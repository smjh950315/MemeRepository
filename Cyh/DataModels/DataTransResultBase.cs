namespace Cyh.DataModels
{
    /// <summary>
    /// 資料交易後的結果
    /// </summary>
    public class DataTransResultBase : IDataTransResult
    {
        List<TransDetails>? _TransDetails;
        DateTime? _EndTime;
        string? _Message;
        string? _Accesser;

        public int TotalTransCount { get; set; }

        public int SucceedTransCount { get; set; }

        public bool IsFinished { get; set; }

        public string Message {
            get => this._Message ?? string.Empty;
            set => this._Message = value;
        }

        public string Accesser {
            get => this._Accesser ?? string.Empty;
            set => this._Accesser = value;
        }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime {
            get => this._EndTime ?? this.BeginTime;
            set => this._EndTime = value;
        }

        public List<TransDetails> TransDetails {
            get {
                if (this._TransDetails == null)
                    this._TransDetails = [];
                return this._TransDetails;
            }
            set { this._TransDetails = value; }
        }

        public DataTransResultBase() {
            this.BeginTime = DateTime.Now;
        }
    }
}
