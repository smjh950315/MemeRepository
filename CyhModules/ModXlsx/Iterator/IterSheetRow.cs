using ClosedXML.Excel;
using System.Collections;

namespace Cyh.Modules.ModXlsx.Iterator
{
    public class IterSheetRow : IEnumerator<XlsxSheetRow>
    {
        XlsxWorksheet _MyWorksheet;
        IEnumerator<IXLRow> _Enumerator;

        public IterSheetRow(XlsxWorksheet myWorksheet) {
            this._MyWorksheet = myWorksheet;
            this._Enumerator = myWorksheet._Worksheet.Rows().GetEnumerator();
        }

        public XlsxSheetRow Current => this._MyWorksheet[this._Enumerator.Current.RowNumber()];

        object IEnumerator.Current => this.Current;

        public void Dispose() { }

        public bool MoveNext() {
            return this._Enumerator.MoveNext();
        }

        public void Reset() {
            this._Enumerator = this._MyWorksheet._Worksheet.Rows().GetEnumerator();
        }
    }
}
