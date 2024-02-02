using ClosedXML.Excel;
using System.Collections;

namespace Cyh.Modules.ModXlsx.Iterator
{
    public class IterSheetCell : IEnumerator<XlsxSheetCell>
    {
        XlsxSheetRow _MySheetRow;
        IEnumerator<IXLCell> _Enumerator;

        public IterSheetCell(XlsxSheetRow _mySheetRow) {
            this._MySheetRow = _mySheetRow;
            this._Enumerator = _mySheetRow._XlsxRow.Cells().GetEnumerator();
        }

        public XlsxSheetCell Current => this._MySheetRow[this._Enumerator.Current.Address.ColumnNumber];

        object IEnumerator.Current => this.Current;

        public void Dispose() { }

        public bool MoveNext() {
            return this._Enumerator.MoveNext();
        }

        public void Reset() {
            this._Enumerator = _MySheetRow._XlsxRow.Cells().GetEnumerator();
        }
    }
}
