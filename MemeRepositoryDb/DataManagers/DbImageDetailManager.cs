using Cyh.DataHelper;
using MemeRepository.Db.Models;
using MemeRepository.Lib.Interface;
using MemeRepository.Lib.Managers;

namespace MemeRepository.Db.DataManagers
{
    public class DbImageDetailManager : ImageManager<IMAGE, TAG, CATE, TAG_BINDING, CATE_BINDING>
    {
        public DbImageDetailManager(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase,
            ITagManager tagManager,
            ICateManager cateManager)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }
    }
}
