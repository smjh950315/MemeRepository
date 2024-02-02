namespace Cyh.Common
{
    /// <summary>
    /// 資料集
    /// </summary>
    public class MyDataSet
    {
        private readonly string? _Name;
        private Dictionary<string, object?>? _KeyValuePairs;

        /// <summary>
        /// 此資料及集的名稱
        /// </summary>
        public string Name {
            get => this._Name ?? "Unnamed";
        }

        /// <summary>
        /// 當前資料集以保存的Key
        /// </summary>
        public IEnumerable<string> Keys {
            get => this._KeyValuePairs?.Keys ?? Enumerable.Empty<string>();
        }

        /// <summary>
        /// 用key值取得儲存的資料
        /// <para>如果是以取得方式呼叫，當該key值不存在時回傳null;</para>
        /// <para>如果是以設定方式呼叫，會更新對應名稱的驗證資料，如果驗證資料不存在，就建立一筆名為key的資料並設定為輸入值</para>
        /// </summary>
        /// <param name="key">驗證資料名稱</param>
        /// <returns>對應名稱的驗證資料</returns>
        public object? this[string key] {
            get {
                if (this._KeyValuePairs == null) return null;
                try {
                    return this._KeyValuePairs[key];
                } catch { return null; }
            }
            set {
                this._KeyValuePairs ??= new();
                try {
                    this._KeyValuePairs[key] = value;
                } catch { }
            }
        }

        public MyDataSet() { }

        public MyDataSet(string name) {
            this._Name = name;
        }
    }
}
