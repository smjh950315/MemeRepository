using System;
using System.Collections.Generic;

namespace MemeRepository.Db.Models
{
    public partial class CategoryDetail
    {
        public long CDID { get; set; }
        public string CategoryName { get; set; } = null!;
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
