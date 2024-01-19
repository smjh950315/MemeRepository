using Cyh.DataHelper;
using Cyh.Modules.ModForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cyh.Modules.ModViewData
{
    public interface IViewModelGetter<TViewModel, TDataModel, IViewModel>
        where TViewModel : IViewModel
        where TDataModel : IViewModel
    {
        IMyDataAccesser<TDataModel> DataAccesser { get; set; }
    }
}
