namespace Cyh.Modules.ModForm
{
    /// <summary>
    /// 一對多表單組合
    /// </summary>
    /// <typeparam name="TMainForm">主資料</typeparam>
    /// <typeparam name="TSubForm">關聯的副資料</typeparam>
    public class MyFormGroupBase<TMainForm, TSubForm> : IFormGroup<TMainForm, TSubForm>
    {
        private IEnumerable<TSubForm>? _SubForms;

        public TMainForm MainForm { get; set; } = default!;

        public IEnumerable<TSubForm> SubForms {
            get => this._SubForms ?? Enumerable.Empty<TSubForm>();
            set => this._SubForms = value;
        }
    }

    /// <summary>
    /// 一對多表單組合
    /// </summary>
    /// <typeparam name="TMainForm">主資料</typeparam>
    /// <typeparam name="TSubForm">關聯的副資料</typeparam>
    /// <typeparam name="TSub2Form">關聯的副資料</typeparam>
    public class MyFormGroupBase<TMainForm, TSubForm, TSub2Form>
        : MyFormGroupBase<TMainForm, TSubForm>, IFormGroup<TMainForm, TSubForm, TSub2Form>
    {
        private IEnumerable<TSub2Form>? _SubForms2;

        public IEnumerable<TSub2Form> SubForms2 {
            get => this._SubForms2 ?? Enumerable.Empty<TSub2Form>();
            set => this._SubForms2 = value;
        }
    }
}
