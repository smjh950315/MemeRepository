using ClosedXML.Excel;
using System.Collections;

namespace Cyh.Modules.ModXlsx.Iterator
{
    public class IterWorkSheet : IEnumerator<XlsxWorksheet>
    {
        XlsxWorkbook _MyWorkbook;
        IEnumerator<IXLWorksheet>? _Enumerator;
        public IterWorkSheet(XlsxWorkbook myWorkbook) {
            this._MyWorkbook = myWorkbook;
            if (!myWorkbook.IsValid) {
                this._Enumerator = null;
            } else {
                this._Enumerator = this._MyWorkbook._Workbook?.Worksheets.GetEnumerator();
            }
        }

        public XlsxWorksheet Current {
            get {
                if (this._Enumerator == null)
                    throw new InvalidOperationException("how do you get here ?!");
                return this._MyWorkbook[this._Enumerator.Current.Name];
            }
        }

        object IEnumerator.Current => this.Current;

        public void Dispose() { }

        public bool MoveNext() {
            if (this._Enumerator != null)
                return this._Enumerator.MoveNext();
            return false;
        }

        public void Reset() {
            if (this._MyWorkbook.IsValid) {
                this._Enumerator = this._MyWorkbook._Workbook?.Worksheets.GetEnumerator();
            } else {
                this._Enumerator = null;
            }
        }
    }
}
