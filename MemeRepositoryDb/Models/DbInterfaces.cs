using MemeRepository.Db.Interface;
using MemeRepository.Lib.DataModels;

namespace MemeRepository.Db.Models
{
    public partial class IMAGE : IImage
    {
    }
    public partial class TAG : ITag
    {
    }
    public partial class CATE : ICategory
    {
    }
    public partial class TAG_BINDING : IImageAttributeBindingModel { }
    public partial class CATE_BINDING : IImageAttributeBindingModel { }
}
