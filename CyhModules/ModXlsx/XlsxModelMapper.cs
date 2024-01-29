using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Reflection;
using static Cyh.ObjectHelper;

namespace Cyh.Modules.ModXlsx
{
    /// <summary>
    /// 將 Excel 的欄位與 Model 的成員對應
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class XlsxModelMapper<TModel> where TModel : class, new()
    {
        int _ReferenceRowNumber;
        XlsxWorksheet _MyWorksheet;
        XlsxSheetRow _MyReferenceRow {
            get {
                return this._MyWorksheet[this._ReferenceRowNumber];
            }
        }
        List<string>? _ModelMemberList;
        List<string>? _CommonOfColumnAndModel;
        Dictionary<int, string>? _XlsxColumnIndexMapping;
        List<MemberInfo>? _ModelMemberInfo;

        Dictionary<int, string> XlsxColumnIndexMapping {
            get {
                if (this._XlsxColumnIndexMapping == null) {
                    this._XlsxColumnIndexMapping = new Dictionary<int, string>();
                    XlsxSheetRow row = this._MyReferenceRow;
                    foreach (XlsxSheetCell cell in row) {
                        this._XlsxColumnIndexMapping.Add(cell.ColumnNumber, cell.Value.GetText());
                    }
                    this._XlsxColumnIndexMapping.DistinctBy(x => x.Value);
                }
                return this._XlsxColumnIndexMapping;
            }
        }
        List<string> ModelMembers {
            get {
                if (this._ModelMemberList == null) {
                    this._ModelMemberList = ObjectHelper.GetInstanceMemberList<TModel>();
                }
                return this._ModelMemberList;
            }
        }
        List<string> CommonOfColumnAndModel {
            get {
                if (this._CommonOfColumnAndModel == null) {
                    IEnumerable<string> modelMembers = this.ModelMembers;
                    IEnumerable<string> xlsxColumns = this.XlsxColumnIndexMapping.Values;
                    this._CommonOfColumnAndModel = new List<string>();
                    foreach (string member in modelMembers) {
                        if (xlsxColumns.Contains(member)) {
                            this._CommonOfColumnAndModel.Add(member);
                        }
                    }
                }
                return this._CommonOfColumnAndModel;
            }
        }

        List<MemberInfo> ModelMemberInfos {
            get {
                if (this._ModelMemberInfo == null) {
                    this._ModelMemberInfo = GetMemberInfosOf<TModel>(ComplexBindingFlags.Inst_ObjectType_Member).ToList();
                }
                return this._ModelMemberInfo;
            }
        }

        bool _SetModelInnerValue(ref TModel model, string memberName, object? value) {
            MemberInfo? memberInfo = this.ModelMemberInfos.FirstOrDefault(x => x.Name == memberName);
            if (memberInfo == null) return false;
            return TrySetValue(model, memberInfo, value);
        }
        string? _GetModelInnerValue(object model, string memberName) {
            MemberInfo? memberInfo = this.ModelMemberInfos.FirstOrDefault(x => x.Name == memberName);
            if (memberInfo == null) return default;
            return TryGetValue(model, memberInfo)?.ToString();
        }

        void _ReadFromRow(XlsxSheetRow row, out TModel? model) {
            model = new TModel();
            IEnumerable<string> modelMembers = this.ModelMembers;
            foreach (string member in this.CommonOfColumnAndModel) {
                int colIndex = XlsxColumnIndexMapping.FirstOrDefault(x => x.Value == member).Key;
                this._SetModelInnerValue(ref model, member, row[colIndex].Value.GetText());
            }
        }

        void _WriteToRow(TModel? model, ref XlsxSheetRow? xlsxSheetCells) {
            //throw new NotImplementedException("尚未完成的功能");
            if (model == null || xlsxSheetCells == null) return;
            foreach (string member in this.CommonOfColumnAndModel) {
                int colIndex = XlsxColumnIndexMapping.FirstOrDefault(x => x.Value == member).Key;
                xlsxSheetCells[colIndex].Value = this._GetModelInnerValue(model, member);
            }
        }

        public XlsxModelMapper(XlsxWorksheet sheet, int refRowNumber) {
            this._MyWorksheet = sheet;
            this._ReferenceRowNumber = refRowNumber;
        }

        public List<TModel> GetModels() {
            List<TModel> result = new List<TModel>();
            bool refIsPassed = false;
            foreach (var row in this._MyWorksheet) {
                if (!refIsPassed) {
                    if (row.RowNumber == this._ReferenceRowNumber) {
                        refIsPassed = true;
                        continue;
                    }
                }
                TModel? model;
                this._ReadFromRow(row, out model);
                if (model != null) {
                    result.Add(model);
                }
            }
            return result;
        }
        public void WriteModels(IEnumerable<TModel> models) {
            int currentRowNum = this._ReferenceRowNumber + 1;
            foreach (var model in models) {
                var row = this._MyWorksheet[currentRowNum];
                this._WriteToRow(model, ref row);
                currentRowNum++;
            }
        }

    }
}
