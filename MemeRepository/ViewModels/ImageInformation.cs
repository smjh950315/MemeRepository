namespace MemeRepository.ViewModels
{
    public class ImageInformation
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public byte[]? Data { get; set; } = null!;
        public int? Size { get; set; }
        public string? Type { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }

        public IEnumerable<TagInformation> Tags { get; set; } = null!;
    }
}
