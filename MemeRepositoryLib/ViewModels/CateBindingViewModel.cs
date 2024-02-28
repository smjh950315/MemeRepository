using Cyh.DataModels;
using MemeRepository.Lib.DataModels;
using System.Linq.Expressions;

namespace MemeRepository.Lib.ViewModels
{
    public class CateBindingViewModel : ICateBinding, ISelectableModel<CateBindingViewModel, ICateBinding>
    {
        public long ID { get; set; }
        public long? IMAGE_ID { get; set; }
        public long? CATE_ID { get; set; }
        public bool? IS_BOUND { get; set; }

        public CateBindingViewModel Self() => this;

        public void UpdateFromTo<From, To>(From from, To to)
            where From : ICateBinding
            where To : ICateBinding {
            to.ID = from.ID;
            to.IMAGE_ID = from.IMAGE_ID;
            to.CATE_ID = from.CATE_ID;
            to.IS_BOUND = from.IS_BOUND;
        }

        public Expression<Func<From, To>> GetSelectorFromTo<From, To>()
            where From : ICateBinding
            where To : class, ICateBinding, new() {
            return x => new To
            {
                ID = x.ID,
                IMAGE_ID = x.IMAGE_ID,
                CATE_ID = x.CATE_ID,
                IS_BOUND = x.IS_BOUND,
            };
        }
    }
}
