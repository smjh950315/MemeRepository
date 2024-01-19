using System.Text;

namespace Cyh.MyException
{
    public class ArgumentDetail
    {
        static public string DefaultFormatString = "Name: {}, Type: {}, IsNull: {}";
        public string TypeName { get; set; }
        public string ArgumentName { get; set; }
        public bool IsNull { get; set; }
        public ArgumentDetail() {
            this.TypeName = "UNKNOWN";
            this.ArgumentName = "UNKNOWN";
        }
        public ArgumentDetail(string typeName, string argumentName) {
            this.TypeName = typeName;
            this.ArgumentName = argumentName;
            this.IsNull = false;
        }

        public override string ToString() {
            return String.Format(
                DefaultFormatString,
                this.ArgumentName,
                this.TypeName, 
                this.IsNull.ToString());
        }

        static public ArgumentDetail NewDetails<T>(string para_name, bool isNull=true) {
            ArgumentDetail res = new() {
                TypeName = typeof(T).Name,
                ArgumentName = para_name,
                IsNull = isNull
            };
            return res;
        }
    }
    public class MyArgumentException : ArgumentNullException
    {
        List<ArgumentDetail>? _ArgumentDetails;
        public List<ArgumentDetail> ArgumentDetails {
            get {
                this._ArgumentDetails ??= new();
                return this._ArgumentDetails;
            }
        }

        public string GetDetailList() {
            if (_ArgumentDetails.IsNullOrEmpty())
                return "Empty!";

            StringBuilder sb = new StringBuilder();
            
            foreach(var argDetails in ArgumentDetails) {
                sb.Append(argDetails.ToString());
            }

            return sb.ToString();
        }

        public MyArgumentException() { }

        public MyArgumentException(params ArgumentDetail[] args) {
            if (args.IsNullOrEmpty()) return;
            ArgumentDetails.AddRange(args);
        }
    }
}
