using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cyh.EFCore.Interface
{
    /// <summary>
    /// 執行查詢的類別基底，包含 _Context
    /// </summary>
    public interface IDbQuery
    {
        /// <summary>
        /// 要使用的 DbContext
        /// </summary>
        IDbContext? _Context { get; set; }
    }

    /// <summary>
    /// 執行查詢的類別基底，包含 _Context
    /// </summary>
    public interface IDbQuery<TEntity> : IDbQuery where TEntity : class
    {
        /// <summary>
        /// 查詢時調用的 Entity
        /// </summary>
        DbSet<TEntity>? Entities { get; }
    }
}
