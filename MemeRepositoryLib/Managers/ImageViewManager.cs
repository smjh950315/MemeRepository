using Cyh.DataHelper;
using MemeRepository.Lib.DataModels;
using MemeRepository.Lib.Interface;
using MemeRepository.Lib.Prototype;
using MemeRepository.Lib.ViewModels;
using System.Linq.Expressions;

namespace MemeRepository.Lib.Managers
{
    public class ImageViewManager<TImage>
        : MemeRepositoryManagerBase<TImage, ImageViewModel, IImage>
        , IImageManager
        where TImage : class, IImage, new()
    {
        public ImageViewManager(IDataManagerActivator dataManagerActivator, IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        public override ImageViewModel? GetById(long id) {
            return this.GetView(x => x.ID == id);
        }

        public override Expression<Func<TImage, bool>> GetExprToFindDataModel(ImageViewModel view) {
            return x => x.ID == view.ID;
        }
    }
}
