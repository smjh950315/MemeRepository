using Cyh.DataHelper;
using MemeRepository.Db.Models;
using MemeRepository.Lib.Managers;

namespace MemeRepository.Db.DataManagers
{
    public class DbCategoryManager : CategoryManager<CATE>
    {
        public DbCategoryManager(IDataManagerActivator dataManagerActivator, IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }
    }
}
