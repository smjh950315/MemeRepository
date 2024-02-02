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
        /// [注意] 此處假定輸入的參數必須有效
        /// </summary>
        internal XlsxSheetRow(XlsxWorksheet myWorkSheet, int rowIndex) {
            this._MyWorkSheet = myWorkSheet;
            this._RowIndex = rowIndex;
        }

        /// <summary>
        /// Row的索引(從1開始)
        /// </summary>
        public int RowNumber => this._XlsxRow.RowNumber();

        /// <summary>
        /// 用列號取得資料格
        /// </summary>
        /// <param name="colNumber">要取得的列號</param>
        /// <returns>資料格</returns>
        public XlsxSheetCell this[int colNumber] {
            get {
                return new XlsxSheetCell(this, colNumber);
            }
        }


        public IEnumerator<XlsxSheetCell> GetEnumerator() {
            return new IterSheetCell(this);
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}
