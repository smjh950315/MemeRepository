﻿using System;
using System.Collections.Generic;

namespace MemeRepository.Db.Models
{
    public partial class TAG
    {
        public long ID { get; set; }
        public string NAME { get; set; } = null!;
        public string? REMIND { get; set; }
        public DateTime? CREATED { get; set; }
    }
}