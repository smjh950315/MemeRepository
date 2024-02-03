using Cyh.DataModels;
using MemeRepository.Db.Interface;
using System.Linq.Expressions;

namespace MemeRepository.Models
{
    public class ImageViewModel
        : IMemeImage
        , ISelectableModel<ImageViewModel, IMemeImage>
    {
        public string? TITLE { get; set; }

        public DateTime? UPLOADED { get; set; }

        public Expression<Func<From, To>> GetSelectorFromTo<From, To>()
            where From : IMemeImage
            where To : class, IMemeImage, new() {

            return x => new To
            {
                TITLE = x.TITLE,
                UPLOADED = x.UPLOADED,
            };
        }
    }
}
