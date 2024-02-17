namespace MemeRepository.ViewModels
{
    public class ImageUploadViewModel
    {
        public string Name { get; set; } = null!;

        public IFormFile ImageFile { get; set; } = null!;
    }
}
