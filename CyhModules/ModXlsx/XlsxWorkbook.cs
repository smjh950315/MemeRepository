using ClosedXML;
using ClosedXML.Excel;
using Cyh.Modules.ModXlsx.Iterator;
using DocumentFormat.OpenXml.EMMA;
using System.Collections;
using System.IO;

namespace Cyh.Modules.ModXlsx
{
    /// <summary>
    /// Excel結構存取器
    /// </summary>
    public class XlsxWorkbook : IEnumerable<XlsxWorksheet>
    {
        internal IXLWorkbook? _Workbook;
        private XlsxWorksheet __Unchecked_CallSheet(string name) {
            return new(this, name);
        }
        private XlsxWorkbook() { }

        /// <summary>
        /// 當前Excel結構是否有效
        /// </summary>
        public bool IsValid {
            get => _Workbook != null;
        }

        /// <summary>
        /// 用名稱取得試算表
        /// </summary>
        /// <param name="name">要取得的試算表名稱</param>
        /// <returns>試算表存取器</returns>
        /// <exception cref="ArgumentNullException">名稱為空或是當前試算文件無效</exception>
        public XlsxWorksheet this[string? name, bool create_if_null = false] {
            get {
                if (!this.IsValid)
                    throw new ArgumentNullException("current workbook is invalid!");

                if (name.IsNullOrEmpty())
                    throw new ArgumentNullException("name is empty!");

                return this.__Unchecked_CallSheet(name);
            }
        }

        /// <summary>
        /// 不從其他地方載入，直接建立新的Excel結構
        /// </summary>
        /// <returns>新的Excel結構</returns>
        public static XlsxWorkbook Create() {
            XlsxWorkbook wb = new XlsxWorkbook();
            wb._Workbook = new XLWorkbook();
            return wb;
        }

        /// <summary>
        /// 從資料流讀取Excel結構
        /// </summary>
        /// <param name="stream">資料流</param>
        public XlsxWorkbook(Stream stream) {
            try {
                this._Workbook = new XLWorkbook(stream);
            } catch (Exception ex) {
                CommonLib.HandleException(ex);
            }
        }

        /// <summary>
        /// 從檔案路徑讀取Excel結構
        /// </summary>
        /// <param name="path">檔案路徑</param>
        public XlsxWorkbook(string path) {
            try {
                this._Workbook = new XLWorkbook(path);
            } catch (Exception ex) {
                CommonLib.HandleException(ex);
            }
        }

        /// <summary>
        /// 儲存當前Excel結構
        /// </summary>
        /// <returns>是否成功儲存</returns>
        public bool Save() {
            if (this.IsValid)
                return false;
#pragma warning disable CS8602
            return CommonLib.TryExecute(fn => this._Workbook.Save());
#pragma warning restore CS8602            
        }

        /// <summary>
        /// 儲存當前Excel結構到資料流
        /// </summary>
        /// <returns>是否成功儲存</returns>
        public bool Save(Stream stream) {
            if (!this.IsValid)
                return false;
            return CommonLib.TryExecute(fn => this._Workbook?.SaveAs(stream));
        }

        /// <summary>
        /// 儲存當前Excel結構到檔案路徑
        /// </summary>
        /// <param name="path">目標檔案路徑</param>
        /// <returns>是否成功儲存</returns>
        public bool SaveToFile(string path) {
            FileStream? fs = CommonLib.TryGetValue(fn => new FileStream(path, FileMode.OpenOrCreate), null);
            if (fs == null) return false;
            bool res = this.Save(fs);
            fs.Close();
            return res;
        }


        public IEnumerator<XlsxWorksheet> GetEnumerator() {
            return new IterWorkSheet(this);
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}
