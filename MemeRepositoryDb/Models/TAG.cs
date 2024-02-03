using System;
using System.Collections.Generic;

namespace MemeRepository.Db.Models
{
    public partial class Tag
    {
        public long TagID { get; set; }
        public string? TagName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
