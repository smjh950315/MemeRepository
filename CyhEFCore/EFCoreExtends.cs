using Cyh.EFCore.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cyh.EFCore
{
    public static class EFCoreExtends
    {
        /// <summary>
        /// 用 DbContext 初始化要使用的 Entity
        /// </summary>
        public static DbSet<TEntity>? GetDbSet<TEntity>(this IDbQuery dbQuery) where TEntity : class {
#if DEBUG
            return dbQuery._Context?.Set<TEntity>();
#else
            return TryGetValue(fn => dbQuery._Context?.Set<TEntity>(), null, new Exception($"Invalid DbEntity of {typeof(TEntity).Name}"));
#endif
        }

        /// <summary>
        /// 用 DbContext 初始化要使用的 Entity
        /// </summary>
        public static DbSet<TEntity>? GetDbSet<TEntity>(this IDbQuery dbQuery, ref object? dst) where TEntity : class {
            dst = dbQuery.GetDbSet<TEntity>(); return dst as DbSet<TEntity>;
        }

        /// <summary>
        /// 用 DbContext 初始化要使用的 Entity
        /// </summary>
        public static DbSet<TInterface>? GetDbSet<TInterface, TEntity>(this IDbQuery dbQuery, ref object? dst) where TInterface : class where TEntity : class, TInterface {
            return dbQuery.GetDbSet<TEntity>(ref dst) as DbSet<TInterface>;
        }

        /// <summary>
        /// 用實體類型加上條件查詢
        /// </summary>
        /// <typeparam name="TEntity">Entity 類型</typeparam>
        /// <param name="_Predicate">查詢條件</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> Query<TEntity>(this IDbQuery dbQuery, Expression<Func<TEntity, bool>> _Predicate) where TEntity : class {
            var setT = dbQuery.GetDbSet<TEntity>();
            if (setT == null) { return new List<TEntity>(); }
#pragma warning disable
            return TryGetValue(fn => setT.Where(_Predicate).AsEnumerable(), new List<TEntity>());
#pragma warning restore
        }
    }
}
