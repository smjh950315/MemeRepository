using System.Text;

namespace Cyh.MyException
{
    /// <summary>
    /// 參數訊息
    /// </summary>
    public class ArgumentDetail
    {
        /// <summary>
        /// 格式化字串
        /// </summary>
        static public string DefaultFormatString = "Name: {0}, Type: {1}, IsNull: {2}\n";

        /// <summary>
        /// 參數類型
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 參數名稱
        /// </summary>
        public string ArgumentName { get; set; }

        /// <summary>
        /// 是否為 null
        /// </summary>
        public bool IsNull { get; set; }

        /// <summary>
        /// 參數訊息
        /// </summary>
        public ArgumentDetail() {
            this.TypeName = "UNKNOWN";
            this.ArgumentName = "UNKNOWN";
        }

        /// <summary>
        /// 參數訊息
        /// </summary>
        /// <param name="typeName">參數類型</param>
        /// <param name="argumentName">參數名稱</param>
        public ArgumentDetail(string typeName, string argumentName) {
            this.TypeName = typeName;
            this.ArgumentName = argumentName;
            this.IsNull = false;
        }

        /// <summary>
        /// 取得字串化的參數資料
        /// </summary>
        /// <returns>字串化的參數資料</returns>
        public override string ToString() {
            return String.Format(
                DefaultFormatString,
                this.ArgumentName,
                this.TypeName,
                this.IsNull.ToString());
        }

        /// <summary>
        /// 建立新的參數訊息
        /// </summary>
        /// <typeparam name="T">參數類型</typeparam>
        /// <param name="para_name">參數名稱</param>
        /// <param name="isNull">是否為 null</param>
        /// <returns>參數資訊</returns>
        static public ArgumentDetail NewDetails<T>(string para_name, bool isNull = true) {
            ArgumentDetail res = new()
            {
                TypeName = typeof(T).Name,
                ArgumentName = para_name,
                IsNull = isNull
            };
            return res;
        }
    }

    /// <summary>
    /// 參數相關的例外狀況
    /// </summary>
    public class MyArgumentException : ArgumentNullException
    {
        List<ArgumentDetail>? _ArgumentDetails;

        /// <summary>
        /// 參數資訊的清單
        /// </summary>
        public List<ArgumentDetail> ArgumentDetails {
            get {
                this._ArgumentDetails ??= new();
                return this._ArgumentDetails;
            }
        }

        /// <summary>
        /// 取得字串化的參數資料
        /// </summary>
        /// <returns>字串化的參數資料</returns>
        public string GetDetailString() {
            if (_ArgumentDetails.IsNullOrEmpty())
                return "Empty!";

            StringBuilder sb = new StringBuilder();

            foreach (var argDetails in ArgumentDetails) {
                sb.Append(argDetails.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// 參數相關的例外狀況
        /// </summary>
        public MyArgumentException() { }

        /// <summary>
        /// 參數相關的例外狀況
        /// </summary>
        /// <param name="args">參數資訊集合</param>
        public MyArgumentException(params ArgumentDetail[] args) {
            if (args.IsNullOrEmpty()) return;
            ArgumentDetails.AddRange(args);
            Console.WriteLine("ArgumentException: " + GetDetailString() + DateTime.Now.ToString());
        }
    }
}
