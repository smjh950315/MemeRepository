using Cyh.Modules.ModForm;

namespace MemeRepository.Lib.ViewModels
{
    public class ImageBindingViewModel : IFormGroup<ImageViewModel, TagBindingViewModel, CateBindingViewModel>
    {
        public IEnumerable<CateBindingViewModel>? SubForms2 { get; set; }
        public ImageViewModel? MainForm { get; set; }
        public IEnumerable<TagBindingViewModel>? SubForms { get; set; }
    }
}
