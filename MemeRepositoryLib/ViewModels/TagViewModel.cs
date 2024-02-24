using Cyh.DataModels;
using MemeRepository.Lib.DataModels;
using System.Linq.Expressions;

namespace MemeRepository.Lib.ViewModels
{
    public class TagViewModel : ITag, ISelectableModel<TagViewModel, ITag>
    {
        public long ID { get; set; }
        public string? NAME { get; set; }
        public DateTime? CREATED { get; set; }
        public DateTime? UPDATED { get; set; }

        public Expression<Func<From, To>> GetSelectorFromTo<From, To>()
            where From : ITag
            where To : class, ITag, new() {
            return x => new To
            {
                ID = x.ID,
                NAME = x.NAME,
                CREATED = x.CREATED,
                UPDATED = x.UPDATED,
            };
        }

        public TagViewModel Self() => this;

        public void UpdateFromTo<From, To>(From from, To to)
            where From : ITag
            where To : ITag {
            to.ID = from.ID;
            to.NAME = from.NAME;
            to.CREATED = from.CREATED;
            to.UPDATED = from.UPDATED;
        }
    }
}
