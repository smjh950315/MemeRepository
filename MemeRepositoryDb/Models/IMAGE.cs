using System;
using System.Collections.Generic;

namespace MemeRepository.Db.Models
{
    public partial class Image
    {
        public long ImageID { get; set; }
        public long? CategoryID { get; set; }
        public long? TagID { get; set; }
        public byte[] ImageData { get; set; } = null!;
        public int? ImageSize { get; set; }
        public string? ImageType { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
