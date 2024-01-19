namespace Cyh.EFCore.Interface
{
    public interface IDbCRUD : IDbQuery
    {

    }
    public interface IDbCRUD<TEntity> : IDbQuery<TEntity> where TEntity : class
    {
        IEnumerable<TEntity>? Where(Func<TEntity, bool> predicate) => this.Entities?.Where(predicate);
    }
}
