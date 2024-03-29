using Cyh.DataHelper;
using MemeRepository.Db.Models;
using MemeRepository.Lib.Managers;

namespace MemeRepository.Db.DataManagers
{
    public class DbTagManager : TagManager<TAG>
    {
        public DbTagManager(IDataManagerActivator dataManagerActivator, IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }
    }
}
