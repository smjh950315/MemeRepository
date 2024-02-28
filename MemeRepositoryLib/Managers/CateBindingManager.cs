using Cyh.DataHelper;
using MemeRepository.Lib.DataModels;
using MemeRepository.Lib.Prototype;

namespace MemeRepository.Lib.Managers
{
    public class CateBindingManager<TBinding>
        : BindingManagerBase<TBinding>
        where TBinding : class, ICateBinding, new()
    {
        public CateBindingManager(IDataManagerActivator dataManagerActivator, IDataManagerBuilder dataManagerCreaterBase) : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        public override TBinding CreateNewBinding(long imgId, long selfId, bool bind) {
            return new TBinding
            {
                IMAGE_ID = imgId,
                CATE_ID = selfId,
                IS_BOUND = bind,
            };
        }

        public override TBinding? FindBinding(long imgId, long selfId) {
            return this.GetDataModel(x => x.IMAGE_ID == imgId && x.CATE_ID == selfId);
        }

        public override IEnumerable<long> GetIdsRelateToImageId(long imgId) {
            return this.GetDataModelsAs(x => x.CATE_ID.Value, x => x.CATE_ID != null);
        }
    }
}
