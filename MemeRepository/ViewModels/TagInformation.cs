namespace MemeRepository.ViewModels
{
    public class TagInformation
    {
        public long TagId { get; set; }
        public string? TagName { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
