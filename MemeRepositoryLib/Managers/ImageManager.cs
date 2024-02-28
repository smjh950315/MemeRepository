using Cyh.DataHelper;
using Cyh.DataModels;
using DocumentFormat.OpenXml.Wordprocessing;
using MemeRepository.Lib.DataModels;
using MemeRepository.Lib.Interface;
using MemeRepository.Lib.Prototype;
using MemeRepository.Lib.ViewModels;
using System.Linq.Expressions;

namespace MemeRepository.Lib.Managers
{
    public class ImageManager<TImage, TTag, TCate, TTagBinding, TCateBinding>
        : MemeRepositoryManagerBase<TImage, ImageViewModel, TTag, TagViewModel, TCate, CateViewModel>
        , IImageManager
        where TImage : class, IImage, new()
        where TTag : class, ITag, new()
        where TCate : class, ICategory, new()
        where TTagBinding : class, ITagBinding, new()
        where TCateBinding : class, ICateBinding, new()
    {
        IMyDataAccesser<ImageDetailViewModel>? IDataManager<ImageDetailViewModel>.MainDataSource { get; set; }

        TagBindingManager<TTagBinding> TagBindingManager { get; set; }
        CateBindingManager<TCateBinding> CateBindingManager { get; set; }

        public ImageManager(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
            this.TagBindingManager = new(dataManagerActivator, dataManagerCreaterBase);
            this.CateBindingManager = new(dataManagerActivator, dataManagerCreaterBase);
        }
        public override Expression<Func<TImage, bool>> GetExprToFindDataModel(ImageViewModel view) {
            return x => x.ID == view.ID;
        }

        public class ImageMinInfo
        {
            public long ID { get; set; }
        }

        public override Expression<Func<TCate, bool>> GetExprToFindDataModel(CateViewModel view) {
            return x => x.ID == view.ID;
        }

        public override void UpdateModelFromViewModel(TCate data, CateViewModel view) {
            view.UpdateFromTo(view, data);
        }

        public override Expression<Func<CateViewModel, TCate>> GetExprToSub2DataModel() {
            return new CateViewModel().GetSelectorFromTo<CateViewModel, TCate>();
        }

        public override Expression<Func<TCate, CateViewModel>> GetExprToSub2ViewModel() {
            return new CateViewModel().GetSelectorFromTo<TCate, CateViewModel>();
        }

        public override Expression<Func<TCate, bool>> GetExprRelateToMainData2(TImage data) {
            var binds = this.CateBindingManager.GetDataModelsAs(x => x.CATE_ID, x => x.IMAGE_ID == data.ID);
            return x => binds.Contains(x.ID);
        }

        public override Expression<Func<TTag, bool>> GetExprToFindDataModel(TagViewModel view) {
            return x => x.ID == view.ID;
        }

        public override void UpdateModelFromViewModel(TTag data, TagViewModel view) {
            view.UpdateFromTo(view, data);
        }

        public override Expression<Func<TTag, bool>> GetExprRelateToMainData(TImage data) {
            var binds = this.TagBindingManager.GetDataModelsAs(x => x.TAG_ID, x => x.IMAGE_ID == data.ID);
            return x => binds.Contains(x.ID);
        }

        public override Expression<Func<TagViewModel, TTag>> GetExprToSubDataModel() {
            return new TagViewModel().GetSelectorFromTo<TagViewModel, TTag>();
        }

        public override Expression<Func<TTag, TagViewModel>> GetExprToSubViewModel() {
            return new TagViewModel().GetSelectorFromTo<TTag, TagViewModel>();
        }

        public override Expression<Func<ImageViewModel, TImage>> GetExprToDataModel() {
            return new ImageViewModel().GetSelectorFromTo<ImageViewModel, TImage>();
        }

        public override Expression<Func<TImage, ImageViewModel>> GetExprToViewModel() {
            return new ImageViewModel().GetSelectorFromTo<TImage, ImageViewModel>();
        }

        public override void UpdateModelFromViewModel(TImage data, ImageViewModel view) {
            view.UpdateFromTo(view, data);
        }

        public ImageDetailViewModel? GetById(long id) {
            return this.GetViewGroup<ImageDetailViewModel>(x => x.ID == id);
        }

        public IEnumerable<ImageDetailViewModel> GetAll(int begin, int count) {
            var imageIds = this.MainModelHelper.GetDataModelsAs(x => x.ID, begin, count, null);
            List<ImageDetailViewModel> imageDetailViews = new List<ImageDetailViewModel>();
            foreach (var id in imageIds) {
                var imageDetailView = this.GetById(id);
                if (imageDetailView != null) {
                    imageDetailViews.Add(imageDetailView);
                }
            }
            return imageDetailViews;
        }

        public IDataTransResult Save(ImageDetailViewModel viewModel, IDataTransResult? result, bool execNow) {
            return this.Update(viewModel, result, execNow);
        }

        public IDataTransResult Save(IEnumerable<ImageDetailViewModel> viewModels, IDataTransResult? result, bool execNow) {
            return this.Update(viewModels, result, execNow);
        }
    }
}
