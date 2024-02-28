using Cyh.DataModels;

namespace Cyh.DataHelper
{
    public class MyModelHelperBase : IModelHelper
    {
        internal IDataManagerBuilder _DataManagerBuilder;
        internal IDataManagerActivator _DataManagerActivator;
        public IDataManagerActivator DataManagerActivator => this._DataManagerActivator;
        public IDataManagerBuilder DataManagerBuilder => this._DataManagerBuilder;
        public IDataTransResult EmptyResult => new DataTransResultBase();
        public Type? ModelType { get; set; }

        protected MyModelHelperBase(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase) {
            this._DataManagerActivator = dataManagerActivator;
            this._DataManagerBuilder = dataManagerCreaterBase;
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
        public IModelHelper<T> MainModelHelper => this;
    }
    public class MyModelHelperBase<TMainDataModel, TSubDataModel>
        : MyModelHelperBase<TMainDataModel>, IModelHelper<TSubDataModel>
        where TMainDataModel : class
        where TSubDataModel : class
    {
        IDataManager<TSubDataModel>? IModelHelper<TSubDataModel>.DefaultDataManager { get; set; }
        protected MyModelHelperBase(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }
        public IModelHelper<TSubDataModel> SubModelHelper => this;
    }
}
