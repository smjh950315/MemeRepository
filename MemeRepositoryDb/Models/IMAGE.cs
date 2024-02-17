using System;
using System.Collections.Generic;

namespace MemeRepository.Db.Models
{
    public partial class IMAGE
    {
        public long ID { get; set; }
        public string? NAME { get; set; }
        public byte[]? DATA { get; set; }
        public int? SIZE { get; set; }
        public string? TYPE { get; set; }
        public DateTime? CREATED { get; set; }
        public DateTime? UPDATED { get; set; }
    }
}
