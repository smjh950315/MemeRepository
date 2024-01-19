using Cyh.DataHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Cyh.Modules.ModForm
{
    /// <summary>
    /// 表單管理器的實體
    /// </summary>
    public abstract class FormManagerBase : IFormManager
    {
        public virtual IFormManager? GetDefault() {
            throw new NotImplementedException();
        }
        public virtual IFormManager<T>? GetDefault<T>() {
            throw new NotImplementedException();
        }
        public virtual IFormManager<T, U>? GetDefault<T, U>() {
            throw new NotImplementedException();
        }
        public virtual IFormManager<T, U, V>? GetDefault<T, U, V>() {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 表單管理器的實體
    /// </summary>
    /// <typeparam name="MFEntity">表單的模型</typeparam>
    public abstract class FormManagerBase<MFEntity> : FormManagerBase, IFormManager<MFEntity>
    {
        public IMyDataAccesser<MFEntity>? MainFormSource { get; set; }       
    }

    /// <summary>
    /// 表單管理器的實體
    /// </summary>
    /// <typeparam name="MFEntity">表頭的模型</typeparam>
    /// <typeparam name="TFEntity">表身的模型</typeparam>
    public abstract class FormManagerBase<MFEntity, TFEntity> : FormManagerBase<MFEntity>, IFormManager<MFEntity, TFEntity>
    {
        public IMyDataAccesser<TFEntity>? TargetFormSource { get; set; }
    }

    /// <summary>
    /// 表單管理器的實體
    /// </summary>
    /// <typeparam name="MFEntity">表頭的模型</typeparam>
    /// <typeparam name="TFEntity1">表身的模型1</typeparam>
    /// <typeparam name="TFEntity2">表身的模型2</typeparam>
    public abstract class FormManagerBase<MFEntity, TFEntity1, TFEntity2> : FormManagerBase<MFEntity, TFEntity1>, IFormManager<MFEntity, TFEntity1, TFEntity2>
    {
        public IMyDataAccesser<TFEntity2>? TargetFormSource2 { get; set; }
    }

}
