namespace MemeRepository.Db.Models
{
    public partial class TAG
    {
        public long ID { get; set; }
        public string? NAME { get; set; }
        public DateTime? CREATED { get; set; }
        public DateTime? UPDATED { get; set; }
    }
}
