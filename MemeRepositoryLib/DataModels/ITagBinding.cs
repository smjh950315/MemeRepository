namespace MemeRepository.Lib.DataModels
{
    public interface ITagBinding : IImageBinding
    {
        public long? TAG_ID { get; set; }
    }
}
