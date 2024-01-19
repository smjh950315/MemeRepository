using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyh.Modules.ModForm
{
    /// <summary>
    /// 用來表示單頭與單身的關聯
    /// </summary>
    /// <typeparam name="TMainForm"></typeparam>
    /// <typeparam name="TTargetForm"></typeparam>
    public interface IFormGroup<TMainForm, TTargetForm>
    {
        /// <summary>
        /// 表單的單頭
        /// </summary>
        TMainForm? MainForm { get; set; }

        /// <summary>
        /// 表單的單身
        /// </summary>
        IEnumerable<TTargetForm>? TargetForms { get; set; }
    }

    /// <summary>
    /// 用來表示單頭與單身的關聯
    /// </summary>
    /// <typeparam name="TMainForm"></typeparam>
    /// <typeparam name="TTargetForm"></typeparam>
    public class FormGroup<TMainForm, TTargetForm> : IFormGroup<TMainForm, TTargetForm>
    {
        public TMainForm? MainForm { get; set; }

        public IEnumerable<TTargetForm>? TargetForms { get; set; }
    }
}
