namespace MemeRepository.Lib.DataModels
{
    public interface IImageBinding
    {
        long ID { get; set; }
        long? IMAGE_ID { get; set; }
        bool? IS_BOUND { get; set; }
    }
}
