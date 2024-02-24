namespace Cyh.Modules.ModForm
{
    /// <summary>
    /// 用來表示單頭與單身的關聯
    /// </summary>
    /// <typeparam name="TMainForm"></typeparam>
    /// <typeparam name="TTargetForm"></typeparam>
    public interface IFormGroup<TMainForm, TSubForm>
    {
        /// <summary>
        /// 表單的單頭
        /// </summary>
        TMainForm? MainForm { get; set; }

        /// <summary>
        /// 表單的單身
        /// </summary>
        IEnumerable<TSubForm>? SubForms { get; set; }
    }

    public interface IFormGroup<TMainForm, TSubForm, TSub2Form> : IFormGroup<TMainForm, TSubForm>
    {
        IEnumerable<TSub2Form>? SubForm2 { get; set; }
    }

    /// <summary>
    /// 用來表示單頭與單身的關聯
    /// </summary>
    /// <typeparam name="TMainForm"></typeparam>
    /// <typeparam name="TTargetForm"></typeparam>
    public class FormGroup<TMainForm, TSubForm> : IFormGroup<TMainForm, TSubForm>
    {
        public TMainForm? MainForm { get; set; }

        public IEnumerable<TSubForm>? SubForms { get; set; }
    }
}
