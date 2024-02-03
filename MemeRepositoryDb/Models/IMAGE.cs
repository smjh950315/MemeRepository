using System;
using System.Collections.Generic;

namespace MemeRepository.Db.Models
{
    public partial class Image
    {
        /// <summary>
        /// 圖片ID
        /// </summary>
        public long ImageID { get; set; }
        /// <summary>
        /// 圖片分類ID
        /// </summary>
        public long? CategoryID { get; set; }
        public long? TagID { get; set; }
        /// <summary>
        /// 圖片資料
        /// </summary>
        public byte[] ImageData { get; set; } = null!;
        /// <summary>
        /// 圖片大小
        /// </summary>
        public int? ImageSize { get; set; }
        /// <summary>
        /// 圖片類型
        /// </summary>
        public string? ImageType { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
