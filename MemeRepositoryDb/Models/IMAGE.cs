using MemeRepository.Db.Interface;
using System;
using System.Collections.Generic;

namespace MemeRepository.Db.Models
{
    public partial class IMAGE : IMemeImage
    {
        public long ID { get; set; }
        public string? TITLE { get; set; }
        public string? REMIND { get; set; }
        public DateTime? UPLOADED { get; set; }
        public string? DATA { get; set; }
    }
}
