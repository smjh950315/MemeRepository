using System;
using System.Collections.Generic;

namespace MemeRepository.Db.Models
{
    public partial class CATE_BINDING
    {
        public long ID { get; set; }
        public long? IMAGE_ID { get; set; }
        public long? TAG_ID { get; set; }
    }
}
