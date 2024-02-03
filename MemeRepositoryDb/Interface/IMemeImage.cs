
using Cyh.DataHelper;

namespace MemeRepository.Db.Interface
{
    public interface IMemeImage
    {
        public string? TITLE { get; set; }

        public DateTime? UPLOADED { get; set; }
    }
}
