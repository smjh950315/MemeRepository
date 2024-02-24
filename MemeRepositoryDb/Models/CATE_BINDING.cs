namespace MemeRepository.Db.Models
{
    public partial class CATE_BINDING
    {
        public long ID { get; set; }
        public long? IMAGE_ID { get; set; }
        public long? CATE_ID { get; set; }
        public bool? IS_BOUND { get; set; }
    }
}
