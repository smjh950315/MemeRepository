using ClosedXML.Excel;

namespace Cyh.Modules.ModXlsx
{
    /// <summary>
    /// Excel資料格
    /// </summary>
    public class XlsxSheetCell
    {
        internal int _ColNumber;
        internal XlsxSheetRow _MySheetRow;
        internal IXLCell _SheetCell {
            get => this._MySheetRow._XlsxRow.Cell(this._ColNumber);
        }
        internal XlsxSheetCell(XlsxSheetRow xlsxSheetRow, int colNumberx) {
            this._MySheetRow = xlsxSheetRow;
            this._ColNumber = colNumberx;
        }

        /// <summary>
        /// Excel資料格在表內的位置
        /// </summary>
        public IXLAddress Address => this._SheetCell.Address;

        /// <summary>
        /// Excel資料格在表內的column索引
        /// </summary>
        public int ColumnNumber => this.Address.ColumnNumber;

        /// <summary>
        /// Excel資料格在表內的row索引
        /// </summary>
        public int RowNumber => this.Address.RowNumber;

        /// <summary>
        /// Excel資料格內儲存的值
        /// </summary>
        public XLCellValue Value {
            get => this._SheetCell.Value;
            set => this._SheetCell.Value = value;
        }
    }
}
