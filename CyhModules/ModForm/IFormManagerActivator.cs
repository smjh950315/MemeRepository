using Cyh.DataHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyh.Modules.ModForm
{
    /// <summary>
    /// 表單管理活性化工具的介面
    /// </summary>
    public interface IFormManagerActivator
    {
        /// <summary>
        /// 取得資料存取器
        /// </summary>
        /// <typeparam name="T">要存取的資料型別</typeparam>
        /// <returns>失敗時回傳 null</returns>
        IMyDataAccesser<T>? TryGetDataAccesser<T>() where T : class;
    }

    /// <summary>
    /// 表單管理活性化工具的介面
    /// </summary>
    public interface IFormManagerCreater
    {
        TFormManager CreateManager<TFormManager>() where TFormManager : IFormManager;
    }
}
