using Cyh.EFCore.Interface;
using Microsoft.EntityFrameworkCore;

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
            return TryGetValue(fn => dbQuery._Context?.Set<TEntity>(), null);
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

    }
}
