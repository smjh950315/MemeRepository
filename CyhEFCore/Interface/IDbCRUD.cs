namespace Cyh.EFCore.Interface
{
    /// <summary>
    /// DB CRUD 的介面
    /// </summary>
    public interface IDbCRUD : IDbQuery
    {
    }

    /// <summary>
    /// DB CRUD 的介面
    /// </summary>
    /// <typeparam name="TEntity">要CRUD的類型</typeparam>
    public interface IDbCRUD<TEntity> : IDbQuery<TEntity> where TEntity : class
    {
    }
}
