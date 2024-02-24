using Cyh.DataHelper;
using MemeRepository.Db.Models;
using MemeRepository.Lib.Managers;

namespace MemeRepository.Db.DataManagers
{
    public class CategoryManager : CategoryViewManager<CATE>
    {
        public CategoryManager(IDataManagerActivator dataManagerActivator, IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }
    }
}
