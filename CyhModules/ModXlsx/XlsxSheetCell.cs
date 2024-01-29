using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Cyh.Modules.ModXlsx
{
    public class XlsxSheetCell
    {
        internal int _ColNumber;
        internal XlsxSheetRow _MySheetRow;


        internal IXLCell _SheetCell {
            get => this._MySheetRow._XlsxRow.Cell(this._ColNumber);
        }
        public IXLAddress Address => this._SheetCell.Address;


        public int ColumnNumber => this.Address.ColumnNumber;
        public int RowNumber => this.Address.RowNumber;

        public XLCellValue Value {
            get => this._SheetCell.Value;
            set => this._SheetCell.Value = value;
        }

        internal XlsxSheetCell(XlsxSheetRow xlsxSheetRow, int colNumberx) {
            this._MySheetRow = xlsxSheetRow;
            this._ColNumber = colNumberx;
        }
    }
}
