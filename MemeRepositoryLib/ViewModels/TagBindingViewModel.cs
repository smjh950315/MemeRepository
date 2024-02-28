using Cyh.DataModels;
using MemeRepository.Lib.DataModels;
using System.Linq.Expressions;

namespace MemeRepository.Lib.ViewModels
{
    public class TagBindingViewModel : ITagBinding, ISelectableModel<TagBindingViewModel, ITagBinding>
    {
        public long ID { get; set; }
        public long? IMAGE_ID { get; set; }
        public long? TAG_ID { get; set; }
        public bool? IS_BOUND { get; set; }

        public TagBindingViewModel Self() => this;

        public void UpdateFromTo<From, To>(From from, To to)
            where From : ITagBinding
            where To : ITagBinding {
            to.ID = from.ID;
            to.IMAGE_ID = from.IMAGE_ID;
            to.TAG_ID = from.TAG_ID;
            to.IS_BOUND = from.IS_BOUND;
        }

        public Expression<Func<From, To>> GetSelectorFromTo<From, To>()
            where From : ITagBinding
            where To : class, ITagBinding, new() {
            return x => new To
            {
                ID = x.ID,
                IMAGE_ID = x.IMAGE_ID,
                TAG_ID = x.TAG_ID,
                IS_BOUND = x.IS_BOUND,
            };
        }
    }
}
