using Cyh.DataHelper;
using Cyh.DataModels;

namespace MemeRepository.Lib.Prototype
{
    public interface IMemeRepositoryManager<TViewModel> : IDataManager<TViewModel>
    {
        TViewModel? GetById(long id);
        IEnumerable<TViewModel> GetAll(int begin, int count);
        IDataTransResult Save(TViewModel viewModel, IDataTransResult? result, bool execNow);
        IDataTransResult Save(IEnumerable<TViewModel> viewModels, IDataTransResult? result, bool execNow);
    }
}
