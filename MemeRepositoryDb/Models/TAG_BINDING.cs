﻿namespace MemeRepository.Db.Models
{
    public partial class TAG_BINDING
    {
        public long ID { get; set; }
        public long? IMAGE_ID { get; set; }
        public long? TAG_ID { get; set; }
        public bool? IS_BOUND { get; set; }
    }
}
