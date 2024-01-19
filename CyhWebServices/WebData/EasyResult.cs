
namespace Cyh.WebServices.WebData
{
    public class EasyResult
    {
        public bool Succeed { get; set; }

        public string Result => Succeed ? "成功" : "失敗";

        public string Message { get; set; }

        public List<object> Details { get; set; }

        public EasyResult() {
            Details = new List<object>();
            Message = "null";
        }

        /// <summary>
        /// 加入其他資訊
        /// </summary>
        /// <param name="details">要加入的額外資訊</param>
        public void AddDetails(object details) {
            Details.Add(details);
        }

        public static implicit operator EasyResult(string message) {
            return new EasyResult { Message = message };
        }

        public static implicit operator EasyResult(bool? suceed) {
            if (suceed == null)
                return new EasyResult { Succeed = false };
            return new EasyResult { Succeed = (bool)suceed };
        }

        public static implicit operator bool(EasyResult res) {
            return res.Succeed;
        }

    }
}
