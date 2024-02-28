using Cyh.DataHelper;
using MemeRepository.Lib.DataModels;

namespace MemeRepository.Lib.Prototype
{
    public abstract class BindingManagerBase<TBinding>
        : MyModelHelperBase<TBinding> where TBinding : class, IImageBinding, new()
    {
        protected BindingManagerBase(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        public abstract TBinding? FindBinding(long imgId, long selfId);

        public abstract IEnumerable<long> GetIdsRelateToImageId(long imgId);

        public abstract TBinding CreateNewBinding(long imgId, long selfId, bool bind);

        public void SetBinding(long imgId, long selfId, bool bind) {
            var target = this.FindBinding(imgId, selfId);
            if (bind) {
                if (target == null) {
                    this.SaveDataModel(this.CreateNewBinding(imgId, selfId, bind), null, true);
                } else {
                    if (target.IS_BOUND != bind) {
                        target.IS_BOUND = bind;
                        this.SaveDataModel(target, null, true);
                    }
                }
            } else {
                if (target == null) {
                    return;
                } else {
                    target.IS_BOUND = bind;
                    this.SaveDataModel(target, null, true);
                }
            }
        }
    }
}
