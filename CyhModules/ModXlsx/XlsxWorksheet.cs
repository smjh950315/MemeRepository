#pragma warning disable IDE1006
using ClosedXML.Excel;
using Cyh.Modules.ModXlsx.Iterator;
using System.Collections;

namespace Cyh.Modules.ModXlsx
{
    public class XlsxWorksheet : IEnumerable<XlsxSheetRow>
    {
        string _SheetName;
        internal XlsxWorkbook _MyWorkBook;

        internal IXLWorksheet _Worksheet {
            get {
                IXLWorksheet? workSheet = CommonLib.TryGetValue(fn => this._MyWorkBook?._Workbook?.Worksheet(_SheetName), null);
                if (workSheet == null)
                    workSheet = this.__Unchecked_CreateSheet(this._SheetName);
                return workSheet;
            }
        }

        private IXLWorksheet __Unchecked_CreateSheet(string name) {
#pragma warning disable CS8603
            return this._MyWorkBook._Workbook?.AddWorksheet(name);
#pragma warning restore CS8603
        }

        public XlsxSheetRow this[int rowNumber] {
            get {
                return new XlsxSheetRow(this, rowNumber);
            }
        }

        public bool IsValid => this._Worksheet != null;

        public string SheetName {
            get {
                return this._SheetName;
            }
            set {
                IXLWorksheet? worksheet = this._Worksheet;
                if (worksheet == null)
                    throw new InvalidOperationException("current worksheet is invalid!");
                worksheet.Name = value;
                this._SheetName = value;
            }
        }

        /// <summary>
        /// [注意] 此處輸入的參數必須有效，包含book._Workbook也是，否則後續處理都會有問題
        /// </summary>
        /// <param name="create_if_null">是否發現該名稱不存在的時候新增該名的的試算表</param>
        internal XlsxWorksheet(XlsxWorkbook book, string name) {
            this._MyWorkBook = book;
            this._SheetName = name;
            if (this._Worksheet == null) {
                this.__Unchecked_CreateSheet(name);
            }
        }

        public IEnumerator<XlsxSheetRow> GetEnumerator() {
            return new IterSheetRow(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

    }
}
