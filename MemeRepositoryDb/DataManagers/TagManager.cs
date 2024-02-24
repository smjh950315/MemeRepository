using Cyh.DataHelper;
using MemeRepository.Db.Models;
using MemeRepository.Lib.Managers;

namespace MemeRepository.Db.DataManagers
{
    public class TagManager : TagViewManager<TAG>
    {
        public TagManager(IDataManagerActivator dataManagerActivator, IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }
    }
}
