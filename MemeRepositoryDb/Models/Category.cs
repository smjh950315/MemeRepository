using System;
using System.Collections.Generic;

namespace MemeRepository.Db.Models
{
    public partial class Category
    {
        public long CategoryID { get; set; }
        public string CategoryName { get; set; } = null!;
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
