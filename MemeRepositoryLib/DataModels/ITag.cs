namespace MemeRepository.Lib.DataModels
{
    public interface ITag
    {
        public long ID { get; set; }
        public string? NAME { get; set; }
        public DateTime? CREATED { get; set; }
        public DateTime? UPDATED { get; set; }
    }
}
