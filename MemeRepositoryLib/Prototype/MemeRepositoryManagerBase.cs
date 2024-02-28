using Cyh.DataHelper;
using Cyh.DataModels;
using Cyh.Modules.ModViewData;

namespace MemeRepository.Lib.Prototype
{
    public abstract class MemeRepositoryManagerBase<TDataModel, TViewModel, IDataModel>
        : MyViewModelHelperBase<TDataModel, TViewModel, IDataModel>
        , IMemeRepositoryManager<TViewModel>
        where TDataModel : class, IDataModel, new()
        where TViewModel : class, IDataModel, ISelectableModel<TViewModel, IDataModel>, new()
    {
        public MemeRepositoryManagerBase(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        public IMyDataAccesser<TViewModel>? MainDataSource { get; set; }

        public IEnumerable<TViewModel> GetAll(int begin, int count) {
            return this.GetMultiViews(begin, count, null);
        }

        public abstract TViewModel? GetById(long id);

        public IDataTransResult Save(TViewModel viewModel, IDataTransResult? result, bool execNow) {
            return this.SaveFromView(viewModel, result, execNow);
        }

        public IDataTransResult Save(IEnumerable<TViewModel> viewModels, IDataTransResult? result, bool execNow) {
            return this.SaveFromView(viewModels, result, execNow);
        }
    }

    public abstract class MemeRepositoryManagerBase<TDataModel1, TViewModel1, TDataModel2, TViewModel2>
        : MyRelatedViewModelHelperBase<TDataModel1, TViewModel1, TDataModel2, TViewModel2>
        where TDataModel1 : class
        where TDataModel2 : class
        where TViewModel1 : class
        where TViewModel2 : class
    {
        public MemeRepositoryManagerBase(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }
    }

    public abstract class MemeRepositoryManagerBase<TDataModel1, TViewModel1, TDataModel2, TViewModel2, TDataModel3, TViewModel3>
        : MyRelatedViewModelHelperBase<TDataModel1, TViewModel1, TDataModel2, TViewModel2, TDataModel3, TViewModel3>
        where TDataModel1 : class
        where TDataModel2 : class
        where TDataModel3 : class
        where TViewModel1 : class
        where TViewModel2 : class
        where TViewModel3 : class
    {
        public MemeRepositoryManagerBase(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }
    }
}
