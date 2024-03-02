namespace Cyh.Modules.ModForm
{
    /// <summary>
    /// 一對多表單組合
    /// </summary>
    /// <typeparam name="TMainForm">主資料</typeparam>
    /// <typeparam name="TSubForm">關聯的副資料</typeparam>
    public interface IFormGroup<TMainForm, TSubForm>
    {
        /// <summary>
        /// 表單的單頭
        /// </summary>
        TMainForm MainForm { get; set; }

        /// <summary>
        /// 表單的單身
        /// </summary>
        IEnumerable<TSubForm> SubForms { get; set; }
    }

    /// <summary>
    /// 一對多表單組合
    /// </summary>
    /// <typeparam name="TMainForm">主資料</typeparam>
    /// <typeparam name="TSubForm">關聯的副資料</typeparam>
    /// <typeparam name="TSub2Form">關聯的副資料</typeparam>
    public interface IFormGroup<TMainForm, TSubForm, TSub2Form> : IFormGroup<TMainForm, TSubForm>
    {
        IEnumerable<TSub2Form> SubForms2 { get; set; }
    }
}
