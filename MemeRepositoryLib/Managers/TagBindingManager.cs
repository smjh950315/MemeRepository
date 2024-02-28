using Cyh.DataHelper;
using MemeRepository.Lib.DataModels;
using MemeRepository.Lib.Prototype;

namespace MemeRepository.Lib.Managers
{
    public class TagBindingManager<TBinding>
        : BindingManagerBase<TBinding>
        where TBinding : class, ITagBinding, new()
    {
        public TagBindingManager(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        public override TBinding CreateNewBinding(long imgId, long selfId, bool bind) {
            return new TBinding
            {
                IMAGE_ID = imgId,
                TAG_ID = selfId,
                IS_BOUND = bind,
            };
        }

        public override TBinding? FindBinding(long imgId, long selfId) {
            return this.GetDataModel(x => x.IMAGE_ID == imgId && x.TAG_ID == selfId);
        }

        public override IEnumerable<long> GetIdsRelateToImageId(long imgId) {
            return this.GetDataModelsAs(x => x.TAG_ID.Value, x => x.IMAGE_ID == imgId && x.TAG_ID != null);
        }
    }
}
