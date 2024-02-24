namespace MemeRepository.ViewModels
{
    public class ImageInformationViewModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public byte[]? Data { get; set; } = null!;
        public int? Size { get; set; }
        public string? Type { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }

        public IEnumerable<TagInformationViewModel> Tags { get; set; } = null!;
    }
}
