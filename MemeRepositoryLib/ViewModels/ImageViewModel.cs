using Cyh.DataModels;
using MemeRepository.Lib.DataModels;
using System.Linq.Expressions;

namespace MemeRepository.Lib.ViewModels
{
    /// <summary>
    /// 圖像資料檢視
    /// </summary>
    public class ImageViewModel : IImage, ISelectableModel<ImageViewModel, IImage>
    {
        public long ID { get; set; }
        public string? NAME { get; set; }
        public byte[]? DATA { get; set; }
        public int? SIZE { get; set; }
        public string? TYPE { get; set; }
        public DateTime? CREATED { get; set; }
        public DateTime? UPDATED { get; set; }

        public Expression<Func<From, To>> GetSelectorFromTo<From, To>()
            where From : IImage
            where To : class, IImage, new() {
            return x => new To
            {
                ID = x.ID,
                NAME = x.NAME,
                DATA = x.DATA,
                SIZE = x.SIZE,
                TYPE = x.TYPE,
                CREATED = x.CREATED,
                UPDATED = x.UPDATED,
            };
        }

        public ImageViewModel Self() => this;

        public void UpdateFromTo<From, To>(From from, To to)
            where From : IImage
            where To : IImage {
            to.ID = from.ID;
            to.NAME = from.NAME;
            to.DATA = from.DATA;
            to.SIZE = from.SIZE;
            to.TYPE = from.TYPE;
            to.CREATED = from.CREATED;
            to.UPDATED = from.UPDATED;
        }
    }
}
