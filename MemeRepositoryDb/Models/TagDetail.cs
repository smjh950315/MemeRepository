using System;
using System.Collections.Generic;

namespace MemeRepository.Db.Models
{
    public partial class TagDetail
    {
        public long TDID { get; set; }
        public string TagName { get; set; } = null!;
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
