using Cyh.DataHelper;

namespace Cyh.WebServices.Controller
{
    public static partial class MyControllerExtends
    {
        public static TManager? GetDataManager<TManager, TDataModel>(
            this IMyCastableModelController castableModelController,
            IDataManagerCreater dataManagerCreaterBase,
            ref IDataManager<TDataModel>? manager)
            where TManager : class, IDataManager<TDataModel>
            where TDataModel : class {
            if (manager == null) {
                manager = dataManagerCreaterBase.GetDefault<TDataModel>() as TManager;
                castableModelController.DataManagerActivator.Activate(manager);
            }
            return manager as TManager;
        }
    }
}
