
using Cyh.DataHelper;
using Cyh.EFCore;
using Cyh.EFCore.Interface;

namespace MemeRepository.Db.Accesser
{
    public class MyDataActivator : IDataManagerActivator
    {
        IDbContext? _Context;
        public MyDataActivator() { }
        public MyDataActivator(IDbContext dbContext) { _Context = dbContext; }
        public IMyDataAccesser<T>? TryGetDataAccesser<T>() where T : class {
            return new DbEntityCRUD<T>(_Context);
        }
    }
}
