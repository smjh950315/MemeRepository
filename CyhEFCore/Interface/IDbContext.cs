using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cyh.EFCore.Interface
{
    /// <summary>
    /// DbContext 的介面，使用時要讓 DbXXXContext 繼承
    /// <para>繼承者需要 建構參數為 DbContextOptions&lt;DbXXXContext&gt; 的建構式 </para>
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        ///     <para>
        ///         Creates a <see cref="DbSet{TEntity}" /> for a shared-type entity type that can be used to query and save
        ///         instances of <typeparamref name="TEntity" />.
        ///     </para>
        ///     <para>
        ///         Shared-type entity types are typically used for the join entity in many-to-many relationships.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-query">Querying data with EF Core</see>,
        ///     <see href="https://aka.ms/efcore-docs-change-tracking">Changing tracking</see>, and
        ///     <see href="https://aka.ms/efcore-docs-shared-types">Shared entity types</see>  for more information.
        /// </remarks>
        /// <param name="name">The name for the shared-type entity type to use.</param>
        /// <typeparam name="TEntity">The type of entity for which a set should be returned.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity, and any other reachable entities that are
        ///         not already being tracked, in the <see cref="EntityState.Added" /> state such that they will
        ///         be inserted into the database when <see cref="SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-change-tracking">EF Core change tracking</see> for more information.
        /// </remarks>
        /// <param name="entity">The entity to add.</param>
        /// <returns>
        ///     The <see cref="EntityEntry" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry Add(object entity);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity and entries reachable from the given entity using
        ///         the <see cref="EntityState.Modified" /> state by default, but see below for cases
        ///         when a different state will be used.
        ///     </para>
        ///     <para>
        ///         Generally, no database interaction will be performed until <see cref="SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. All entities found will be tracked
        ///         by the context.
        ///     </para>
        ///     <para>
        ///         For entity types with generated keys if an entity has its primary key value set
        ///         then it will be tracked in the <see cref="EntityState.Modified" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="EntityState.Added" /> state.
        ///         This helps ensure new entities will be inserted, while existing entities will be updated.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        ///     <para>
        ///         For entity types without generated keys, the state set is always <see cref="EntityState.Modified" />.
        ///     </para>
        ///     <para>
        ///         Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-change-tracking">EF Core change tracking</see> for more information.
        /// </remarks>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        ///     The <see cref="EntityEntry" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry Update(object entity);

        /// <summary>
        ///     Begins tracking the given entity in the <see cref="EntityState.Deleted" /> state such that it will
        ///     be removed from the database when <see cref="SaveChanges()" /> is called.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If the entity is already tracked in the <see cref="EntityState.Added" /> state then the context will
        ///         stop tracking the entity (rather than marking it as <see cref="EntityState.Deleted" />) since the
        ///         entity was previously added to the context and does not exist in the database.
        ///     </para>
        ///     <para>
        ///         Any other reachable entities that are not already being tracked will be tracked in the same way that
        ///         they would be if <see cref="Attach(object)" /> was called before calling this method.
        ///         This allows any cascading actions to be applied when <see cref="SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-change-tracking">EF Core change tracking</see> for more information.
        ///     </para>
        /// </remarks>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>
        ///     The <see cref="EntityEntry" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry Remove(object entity);

        /// <summary>
        ///     <para>
        ///         Gets an <see cref="EntityEntry" /> for the given entity. The entry provides
        ///         access to change tracking information and operations for the entity.
        ///     </para>
        ///     <para>
        ///         This method may be called on an entity that is not tracked. You can then
        ///         set the <see cref="EntityEntry.State" /> property on the returned entry
        ///         to have the context begin tracking the entity in the specified state.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-entity-entries">Accessing tracked entities in EF Core</see> for more information.
        /// </remarks>
        /// <param name="entity">The entity to get the entry for.</param>
        /// <returns>The entry for the given entity.</returns>
        EntityEntry Entry(object entity);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity, and any other reachable entities that are
        ///         not already being tracked, in the <see cref="EntityState.Added" /> state such that
        ///         they will be inserted into the database when <see cref="SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-change-tracking">EF Core change tracking</see> for more information.
        /// </remarks>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to add.</param>
        /// <returns>
        ///     The <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entity and entries reachable from the given entity using
        ///         the <see cref="EntityState.Modified" /> state by default, but see below for cases
        ///         when a different state will be used.
        ///     </para>
        ///     <para>
        ///         Generally, no database interaction will be performed until <see cref="SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. All entities found will be tracked
        ///         by the context.
        ///     </para>
        ///     <para>
        ///         For entity types with generated keys if an entity has its primary key value set
        ///         then it will be tracked in the <see cref="EntityState.Modified" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="EntityState.Added" /> state.
        ///         This helps ensure new entities will be inserted, while existing entities will be updated.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        ///     <para>
        ///         For entity types without generated keys, the state set is always <see cref="EntityState.Modified" />.
        ///     </para>
        ///     <para>
        ///         Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-change-tracking">EF Core change tracking</see> for more information.
        /// </remarks>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        ///     The <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        ///     Begins tracking the given entity in the <see cref="EntityState.Deleted" /> state such that it will
        ///     be removed from the database when <see cref="SaveChanges()" /> is called.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If the entity is already tracked in the <see cref="EntityState.Added" /> state then the context will
        ///         stop tracking the entity (rather than marking it as <see cref="EntityState.Deleted" />) since the
        ///         entity was previously added to the context and does not exist in the database.
        ///     </para>
        ///     <para>
        ///         Any other reachable entities that are not already being tracked will be tracked in the same way that
        ///         they would be if <see cref="Attach{TEntity}(TEntity)" /> was called before calling this method.
        ///         This allows any cascading actions to be applied when <see cref="SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-change-tracking">EF Core change tracking</see> for more information.
        ///     </para>
        /// </remarks>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>
        ///     The <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </returns>
        EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        ///     Gets an <see cref="EntityEntry{TEntity}" /> for the given entity. The entry provides
        ///     access to change tracking information and operations for the entity.
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-entity-entries">Accessing tracked entities in EF Core</see> for more information.
        /// </remarks>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to get the entry for.</param>
        /// <returns>The entry for the given entity.</returns>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        ///     <para>
        ///         Saves all changes made in this context to the database.
        ///     </para>
        ///     <para>
        ///         This method will automatically call <see cref="ChangeTracker.DetectChanges" /> to discover any
        ///         changes to entity instances before saving to the underlying database. This can be disabled via
        ///         <see cref="ChangeTracker.AutoDetectChangesEnabled" />.
        ///     </para>
        ///     <para>
        ///         Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This
        ///         includes both parallel execution of async queries and any explicit concurrent use from multiple threads.
        ///         Therefore, always await async calls immediately, or use separate DbContext instances for operations that execute
        ///         in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-saving-data">Saving data in EF Core</see> for more information.
        /// </remarks>
        /// <returns>
        ///     The number of state entries written to the database.
        /// </returns>
        /// <exception cref="DbUpdateException">
        ///     An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        ///     A concurrency violation is encountered while saving to the database.
        ///     A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///     This is usually because the data in the database has been modified since it was loaded into memory.
        /// </exception>
        int SaveChanges();

        /// <summary>
        ///     Begins tracking the given entities, and any other reachable entities that are
        ///     not already being tracked, in the <see cref="EntityState.Added" /> state such that they will
        ///     be inserted into the database when <see cref="SaveChanges()" /> is called.
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-change-tracking">EF Core change tracking</see>
        ///     and <see href="https://aka.ms/efcore-docs-attach-range">Using AddRange, UpdateRange, AttachRange, and RemoveRange</see>
        ///     for more information.
        /// </remarks>
        /// <param name="entities">The entities to add.</param>
        void AddRange(params object[] entities);

        /// <summary>
        ///     <para>
        ///         Begins tracking the given entities and entries reachable from the given entities using
        ///         the <see cref="EntityState.Modified" /> state by default, but see below for cases
        ///         when a different state will be used.
        ///     </para>
        ///     <para>
        ///         Generally, no database interaction will be performed until <see cref="SaveChanges()" /> is called.
        ///     </para>
        ///     <para>
        ///         A recursive search of the navigation properties will be performed to find reachable entities
        ///         that are not already being tracked by the context. All entities found will be tracked
        ///         by the context.
        ///     </para>
        ///     <para>
        ///         For entity types with generated keys if an entity has its primary key value set
        ///         then it will be tracked in the <see cref="EntityState.Modified" /> state. If the primary key
        ///         value is not set then it will be tracked in the <see cref="EntityState.Added" /> state.
        ///         This helps ensure new entities will be inserted, while existing entities will be updated.
        ///         An entity is considered to have its primary key value set if the primary key property is set
        ///         to anything other than the CLR default for the property type.
        ///     </para>
        ///     <para>
        ///         For entity types without generated keys, the state set is always <see cref="EntityState.Modified" />.
        ///     </para>
        ///     <para>
        ///         Use <see cref="EntityEntry.State" /> to set the state of only a single entity.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-change-tracking">EF Core change tracking</see>
        ///     and <see href="https://aka.ms/efcore-docs-attach-range">Using AddRange, UpdateRange, AttachRange, and RemoveRange</see>
        ///     for more information.
        /// </remarks>
        /// <param name="entities">The entities to update.</param>
        void UpdateRange(params object[] entities);

        /// <summary>
        ///     Begins tracking the given entity in the <see cref="EntityState.Deleted" /> state such that it will
        ///     be removed from the database when <see cref="SaveChanges()" /> is called.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If any of the entities are already tracked in the <see cref="EntityState.Added" /> state then the context will
        ///         stop tracking those entities (rather than marking them as <see cref="EntityState.Deleted" />) since those
        ///         entities were previously added to the context and do not exist in the database.
        ///     </para>
        ///     <para>
        ///         Any other reachable entities that are not already being tracked will be tracked in the same way that
        ///         they would be if <see cref="AttachRange(object[])" /> was called before calling this method.
        ///         This allows any cascading actions to be applied when <see cref="SaveChanges()" /> is called.
        ///     </para>
        /// </remarks>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-change-tracking">EF Core change tracking</see>
        ///     and <see href="https://aka.ms/efcore-docs-attach-range">Using AddRange, UpdateRange, AttachRange, and RemoveRange</see>
        ///     for more information.
        /// </remarks>
        /// <param name="entities">The entities to remove.</param>
        void RemoveRange(params object[] entities);
    }
}
