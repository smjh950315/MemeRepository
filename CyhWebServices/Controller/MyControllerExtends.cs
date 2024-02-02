using Cyh.DataHelper;

namespace Cyh.WebServices.Controller
{
    public static partial class MyControllerExtends
    {
        public static TManager? GetDataManager<TManager, TDataModel>(
            this IMyCastableModelController castableModelController,
            IDataManager theBaseManager,
            ref IDataManager<TDataModel>? manager)
            where TManager : class, IDataManager<TDataModel>
            where TDataModel : class {
            if (manager == null) {
                manager = theBaseManager.GetDefault<TDataModel>() as TManager;
                castableModelController.DataManagerActivator.Activate(manager);
            }
            return manager as TManager;
        }
    }
}
