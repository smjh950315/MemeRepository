using ClosedXML.Excel;
using System.Collections;

namespace Cyh.Modules.ModXlsx.Iterator
{
    public class IterSheetCell : IEnumerator<XlsxSheetCell>
    {
        XlsxSheetRow _MySheetRow;
        IEnumerator<IXLCell> _Enumerator;

        public IterSheetCell(XlsxSheetRow _mySheetRow) {
            _MySheetRow = _mySheetRow;
            _Enumerator = _mySheetRow._XlsxRow.Cells().GetEnumerator();
        }

        public XlsxSheetCell Current => _MySheetRow[_Enumerator.Current.Address.ColumnNumber];

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext() {
            return _Enumerator.MoveNext();
        }

        public void Reset() {
            _Enumerator = _MySheetRow._XlsxRow.Cells().GetEnumerator();
        }
    }
}
