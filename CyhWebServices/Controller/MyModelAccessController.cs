using Cyh.DataHelper;
using Cyh.WebServices.AppConfigs;

namespace Cyh.WebServices.Controller
{
    public class MyModelAccessController : MyControllerBase, IMyModelAccessController
    {
        internal IDataManagerCreater _DataManagerCreater;
        internal IDataManagerActivator _DataManagerActivator;

        public IDataManagerActivator DataManagerActivator => this._DataManagerActivator;

        public IDataManagerCreater DataManagerCreater => this._DataManagerCreater;

        public Type? ModelType { get; set; }

        protected MyModelAccessController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerCreater dataManagerCreaterBase)
            : base(webAppConfigurations) {
            this._DataManagerActivator = dataManagerActivator;
            this._DataManagerCreater = dataManagerCreaterBase;
        }
    }
}
