using Cyh.Modules.ModForm;

namespace MemeRepository.Lib.ViewModels
{
    public class ImageDetailViewModel : IFormGroup<ImageViewModel, TagViewModel, CateViewModel>
    {
        public IEnumerable<CateViewModel>? SubForms2 { get; set; }
        public ImageViewModel? MainForm { get; set; }
        public IEnumerable<TagViewModel>? SubForms { get; set; }
    }
}
