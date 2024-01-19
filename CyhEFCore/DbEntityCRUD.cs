using Cyh.DataHelper;
using Cyh.EFCore.Interface;
using Microsoft.EntityFrameworkCore;

namespace Cyh.EFCore
{
    public class DbEntityCRUD<TEntity>
        : DbCRUDBase
        , IDbCRUD<TEntity>
        , IMyDataAccesser<TEntity>
        where TEntity : class
    {
        object? _Entities;

        public DbSet<TEntity>? Entities => this.GetDbSet<TEntity>(ref _Entities);

        public override bool IsAccessable => this.Entities != null;

        public Type DataType => typeof(TEntity);

        public int Count => this.Entities?.Count() ?? 0;

        public IDataTransResult EmptyResult => new DbTransResult();

#pragma warning disable CS8603
        public IQueryable<TEntity>? Queryable => this.Entities;
#pragma warning restore CS8603

        public DbEntityCRUD() { }

        public DbEntityCRUD(IDbContext? dbContext) : base(dbContext) { }

        public bool TryAddOrUpdate(TEntity data) {
            if (!this.IsAccessable)
                return false;
            try {
#pragma warning disable CS8602
                this.Entities.Update(data);
                this._Context.SaveChanges();
#pragma warning restore CS8602
                return true;
            } catch (Exception ex) {
                this.ExceptionHandler(ex);
                return false;
            }

        }

        public IDataTransResult TryAddOrUpdate(IEnumerable<TEntity> dataInput) {
            if (!this.IsAccessable) 
                return this.EmptyResult;

            var result = this.EmptyResult;

            foreach (var data in dataInput) {
#pragma warning disable CS8602
                this.Entities.Update(data);

                result.TotalTransCount++;
                try {
                    this._Context.SaveChanges();
                    result.SucceedTransCount++;
                } catch (Exception ex) {
                    this.ExceptionHandler(ex);
                }
#pragma warning restore CS8602
            }

            return result;
        }

        public bool TryAddOrUpdateSingle(object? dataInput) {
            return this.TryAddOrUpdateSingleT(dataInput);
        }

        public virtual void ExceptionHandler(Exception? exception) {
            //throw new NotImplementedException();
        }
    }

}
