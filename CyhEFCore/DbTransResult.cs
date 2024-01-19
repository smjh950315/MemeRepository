using Cyh.DataHelper;

namespace Cyh.EFCore
{
    public class DbTransResult : IDataTransResult
    {
        public int TotalTransCount {  get; set; }
        public int SucceedTransCount {  get; set; }
    }
}
