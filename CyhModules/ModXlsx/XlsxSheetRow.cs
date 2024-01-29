#pragma warning disable IDE1006
using ClosedXML.Excel;
using Cyh.Modules.ModXlsx.Iterator;
using System.Collections;

namespace Cyh.Modules.ModXlsx
{
    public class XlsxSheetRow : IEnumerable<XlsxSheetCell>
    {
        int _RowIndex;
        internal XlsxWorksheet _MyWorkSheet;
        internal IXLRow _XlsxRow {
            get {
#pragma warning disable CS8602
                return this._MyWorkSheet._Worksheet.Row(this._RowIndex);
#pragma warning restore CS8602
            }
        }

        /// <summary>
        /// Row的索引(從1開始)
        /// </summary>
        public int RowNumber => this._XlsxRow.RowNumber();

        public XlsxSheetCell this[int colNumber] {
            get {
                return new XlsxSheetCell(this, colNumber);
            }
        }

        /// <summary>
        /// [注意] 此處假定輸入的參數必須有效
        /// </summary>
        internal XlsxSheetRow(XlsxWorksheet myWorkSheet, int rowIndex) {
            this._MyWorkSheet = myWorkSheet;
            this._RowIndex = rowIndex;
        }

        public IEnumerator<XlsxSheetCell> GetEnumerator() {
            return new IterSheetCell(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}
