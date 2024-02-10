using Cyh.DataHelper;
using Cyh.DataModels;

namespace Cyh.WebServices.Controller
{
    public interface IMyModelAccessController : IModelHelper
    {
    }

    public interface IMyModelAccessController<DataModel> : IMyModelAccessController, IModelHelper<DataModel> where DataModel : class
    {
    }
}
