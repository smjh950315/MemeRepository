
using Cyh.DataHelper;

namespace MemeRepository.Db.Interface
{
    public interface IMemeTag
    {
        public string? TagName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
