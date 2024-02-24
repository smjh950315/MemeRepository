using Cyh.DataHelper;
using MemeRepository.Lib.DataModels;
using MemeRepository.Lib.Interface;
using MemeRepository.Lib.Prototype;
using MemeRepository.Lib.ViewModels;
using System.Linq.Expressions;

namespace MemeRepository.Lib.Managers
{
    public class TagViewManager<TTagModel>
        : MemeRepositoryManagerBase<TTagModel, TagViewModel, ITag>
        , ITagManager
        where TTagModel : class, ITag, new()
    {
        public TagViewManager(IDataManagerActivator dataManagerActivator, IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        public override TagViewModel? GetById(long id) {
            return this.GetView(x => x.ID == id);
        }

        public override Expression<Func<TTagModel, bool>> GetExprToFindDataModel(TagViewModel view) {
            return x => x.ID == view.ID;
        }
    }
}
