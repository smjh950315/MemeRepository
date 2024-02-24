using Cyh.DataHelper;
using MemeRepository.Lib.DataModels;
using MemeRepository.Lib.Interface;
using MemeRepository.Lib.Prototype;
using MemeRepository.Lib.ViewModels;
using System.Linq.Expressions;

namespace MemeRepository.Lib.Managers
{
    public class CategoryViewManager<TCategoryModel>
        : MemeRepositoryManagerBase<TCategoryModel, CategoryViewModel, ICategory>
        , ICateManager
        where TCategoryModel : class, ICategory, new()
    {
        public CategoryViewManager(IDataManagerActivator dataManagerActivator, IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        public override CategoryViewModel? GetById(long id) {
            return this.GetView(x => x.ID == id);
        }

        public override Expression<Func<TCategoryModel, bool>> GetExprToFindDataModel(CategoryViewModel view) {
            return x => x.ID == view.ID;
        }
    }
}
