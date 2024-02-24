namespace MemeRepository.Lib.DataModels
{
    public interface IImage
    {
        public long ID { get; set; }
        public string? NAME { get; set; }
        public byte[]? DATA { get; set; }
        public int? SIZE { get; set; }
        public string? TYPE { get; set; }
        public DateTime? CREATED { get; set; }
        public DateTime? UPDATED { get; set; }
    }
}
