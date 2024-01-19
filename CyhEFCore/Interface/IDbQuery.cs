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

        /// <summary>
        /// 用實體類型加上條件查詢
        /// </summary>
        /// <typeparam name="TEntity">Entity 類型</typeparam>
        /// <param name="_Predicate">查詢條件</param>
        /// <returns></returns>
        IEnumerable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> _Predicate) where TEntity : class
        {
            var setT = TryGetValue(fn => _Context?.Set<TEntity>(), null);
            if (setT == null) { return new List<TEntity>(); }
#pragma warning disable
            return TryGetValue(fn => setT.Where(_Predicate).AsEnumerable(), new List<TEntity>());
#pragma warning restore
        }
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
