using Cyh.Modules.ModForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyh.WebServices.Controller
{
    public interface IViewModelController<TViewModel, TDataModel, IViewModel>
        where TViewModel : IViewModel
        where TDataModel : IViewModel
    {
        IFormManager<TDataModel> FormManager { get; set; }

    }
}
