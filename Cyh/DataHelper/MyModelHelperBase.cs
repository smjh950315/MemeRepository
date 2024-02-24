using Cyh.DataModels;

namespace Cyh.DataHelper
{
    public class MyModelHelperBase : IModelHelper
    {
        internal IDataManagerBuilder _DataManagerCreater;
        internal IDataManagerActivator _DataManagerActivator;
        public IDataManagerActivator DataManagerActivator => this._DataManagerActivator;
        public IDataManagerBuilder DataManagerCreater => this._DataManagerCreater;
        public IDataTransResult EmptyResult => new DataTransResultBase();
        public Type? ModelType { get; set; }

        protected MyModelHelperBase(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase) {
            this._DataManagerActivator = dataManagerActivator;
            this._DataManagerCreater = dataManagerCreaterBase;
        }
    }
    public class MyModelHelperBase<T> : MyModelHelperBase, IModelHelper<T> where T : class
    {
        public IDataManager<T>? DefaultDataManager { get; set; }

        protected MyModelHelperBase(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
            this.ModelType = typeof(T);
        }
    }
    public class MyModelHelperBase<TMainDataModel, TSubDataModel>
        : MyModelHelperBase<TMainDataModel>, IModelHelper<TSubDataModel>
        where TMainDataModel : class
        where TSubDataModel : class
    {
        protected MyModelHelperBase(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        IDataManager<TSubDataModel>? IModelHelper<TSubDataModel>.DefaultDataManager { get; set; }

        public IModelHelper<TMainDataModel> MainModelHelper => this;
        public IModelHelper<TSubDataModel> SubModelHelper => this;
    }
}
