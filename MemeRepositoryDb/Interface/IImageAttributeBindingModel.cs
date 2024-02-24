namespace MemeRepository.Db.Interface
{
    public interface IImageAttributeBindingModel
    {
        public long ID { get; set; }
        public long? IMAGE_ID { get; set; }
        public bool? IS_BOUND { get; set; }
    }
}
