using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyh.Modules.ModXlsx.Iterator
{
    public class IterSheetRow : IEnumerator<XlsxSheetRow>
    {
        XlsxWorksheet _MyWorksheet;
        IEnumerator<IXLRow> _Enumerator;

        public IterSheetRow(XlsxWorksheet myWorksheet) {
            _MyWorksheet = myWorksheet;
            _Enumerator = myWorksheet._Worksheet.Rows().GetEnumerator();
        }

        public XlsxSheetRow Current => _MyWorksheet[_Enumerator.Current.RowNumber()];

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext() {
            return _Enumerator.MoveNext();
        }

        public void Reset() {
            _Enumerator = _MyWorksheet._Worksheet.Rows().GetEnumerator();
        }
    }
}
