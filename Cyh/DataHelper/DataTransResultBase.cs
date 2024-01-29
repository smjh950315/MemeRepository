using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料交易後的結果
    /// </summary>
    public class DataTransResultBase : IDataTransResult
    {
        TransDetails[]? _TransDetails;
        DateTime? _EndTime;
        string? _Message;
        string? _Accesser;

        public int TotalTransCount { get; set; }

        public int SucceedTransCount { get; set; }

        public string Message {
            get => _Message ?? String.Empty;
            set => _Message = value;
        }

        public string Accesser {
            get => _Accesser ?? String.Empty;
            set => _Accesser = value;
        }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime {
            get => this._EndTime ?? this.BeginTime;
            set => this._EndTime = value;
        }

        public TransDetails[] TransDetails {
            get {
                if (this._TransDetails == null)
                    this._TransDetails = new TransDetails[this.TotalTransCount];
                return this._TransDetails;
            }
            set { this._TransDetails = value; }
        }

        public DataTransResultBase() {
            this.BeginTime = DateTime.Now;
        }
    }
}
