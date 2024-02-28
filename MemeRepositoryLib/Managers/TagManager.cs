using Cyh.DataHelper;
using MemeRepository.Lib.DataModels;
using MemeRepository.Lib.Interface;
using MemeRepository.Lib.Prototype;
using MemeRepository.Lib.ViewModels;
using System.Linq.Expressions;

namespace MemeRepository.Lib.Managers
{
    public class TagManager<TTagModel>
        : MemeRepositoryManagerBase<TTagModel, TagViewModel, ITag>
        , ITagManager
        where TTagModel : class, ITag, new()
    {
        public TagManager(IDataManagerActivator dataManagerActivator, IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        public override TagViewModel? GetById(long id) {
            return this.GetView(x => x.ID == id);
        }

        public override Expression<Func<TTagModel, bool>> GetExprToFindDataModel(TagViewModel view) {
            return x => x.ID == view.ID;
        }

        public IEnumerable<TagViewModel> GetDetails(IEnumerable<long> ids) {
            List<TagViewModel> details = new List<TagViewModel>();
            foreach (long id in ids) {
                TagViewModel? detail = this.GetById(id);
                if (detail != null) {
                    details.Add(detail);
                }
            }
            return details;
        }
    }
}
