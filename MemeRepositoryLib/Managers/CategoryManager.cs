using Cyh.DataHelper;
using MemeRepository.Lib.DataModels;
using MemeRepository.Lib.Interface;
using MemeRepository.Lib.Prototype;
using MemeRepository.Lib.ViewModels;
using System.Linq.Expressions;

namespace MemeRepository.Lib.Managers
{
    public class CategoryManager<TCategoryModel>
        : MemeRepositoryManagerBase<TCategoryModel, CateViewModel, ICategory>
        , ICateManager
        where TCategoryModel : class, ICategory, new()
    {
        public CategoryManager(IDataManagerActivator dataManagerActivator, IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        public override CateViewModel? GetById(long id) {
            return this.GetView(x => x.ID == id);
        }

        public override Expression<Func<TCategoryModel, bool>> GetExprToFindDataModel(CateViewModel view) {
            return x => x.ID == view.ID;
        }
    }
}
