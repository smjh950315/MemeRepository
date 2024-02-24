using Cyh.DataHelper;
using MemeRepository.Db.Models;
using MemeRepository.Lib.Managers;

namespace MemeRepository.Db.DataManagers
{
    public class ImageManager : ImageViewManager<IMAGE>
    {
        public ImageManager(IDataManagerActivator dataManagerActivator, IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }
    }
}
