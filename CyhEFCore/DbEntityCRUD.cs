using Cyh.DataHelper;
using Cyh.DataModels;
using Cyh.EFCore.Interface;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace Cyh.EFCore
{
    /// <summary>
    /// DB 特定類型 CRUD 的 Helper
    /// </summary>
    /// <typeparam name="TEntity">要處理的類型</typeparam>
    public class DbEntityCRUD<TEntity>
        : DbCRUDBase
        , IDbCRUD<TEntity>
        , IMyDataAccesser<TEntity>
        where TEntity : class
    {
        object? _Entities;
        string? _AccesserId;
        static EntityTypeInfo? _EntityTypeInfo;

        EntityTypeInfo EntityTypeInfo {
            get {
                _TryInitializeEntityTypeInfo(this._Context, this.DataType);
#pragma warning disable CS8603
                return _EntityTypeInfo;
#pragma warning restore CS8603
            }
        }

        static private void _TryInitializeEntityTypeInfo(IDbContext? dbContext, Type entityType) {
            if (_EntityTypeInfo == null) {
                _EntityTypeInfo = new(dbContext, entityType);
            } else if (!_EntityTypeInfo.IsValid) {
                _EntityTypeInfo.Reload(dbContext, entityType);
            } else { }
        }

        /// <summary>
        /// 特定類型的TABLE
        /// </summary>
        public DbSet<TEntity>? Entities => this.GetDbSet<TEntity>(ref _Entities);

        /// <summary>
        /// 當前的資料來源是否可以存取
        /// </summary>
        public override bool IsAccessable => this.Entities != null;

        public Type DataType => typeof(TEntity);

        /// <summary>
        /// 當前資料來源的資料筆數
        /// </summary>
        public int Count => this.Entities?.Count() ?? 0;

        public IDataTransResult EmptyResult => new DbTransResult()
        {
            Accesser = this._AccesserId ?? String.Empty,
            BeginTime = DateTime.Now
        };

        public IQueryable<TEntity>? Queryable => this.Entities;

        public bool VeryDetail { get; set; }

        public DbEntityCRUD() { }

        public DbEntityCRUD(IDbContext? dbContext) : base(dbContext) {
            _TryInitializeEntityTypeInfo(dbContext, this.DataType);
        }

        private object? _TryGetEntityKeyValue(TEntity? entity) {
            if (entity == null) return null;
            try {
                if (_EntityTypeInfo != null) {
                    return ObjectHelper.TryGetValue(entity, _EntityTypeInfo.PrimaryKeyInfo);
                } else {
                    return null;
                }
            } catch {
                return null;
            }
        }

        private static int _TryUpdateAllExceptPrimaryKey(EntityTypeInfo entityTypeInfo, TEntity? entityOnBinding, TEntity? entityInput, bool passIfNull) {
            if (entityOnBinding == null || entityInput == null) return 0;
            IEnumerable<MemberInfo> entityMemberInfos = entityTypeInfo.Attributes;
            int updateCount = 0;
            foreach (MemberInfo entityMemberInfo in entityMemberInfos) {
                object? inputValue = ObjectHelper.TryGetValue(entityInput, entityMemberInfo);
                if (inputValue == null && passIfNull) {
                    // do nothing...
                } else {
                    bool isSucceed = ObjectHelper.TrySetValue(entityOnBinding, entityMemberInfo, inputValue);
                    if (isSucceed) updateCount++;
                }
            }
            return updateCount;
        }

        private void _AddOrUpdate(TEntity? entity) {
            if (entity == null || !this.EntityTypeInfo.IsValid || this.Entities == null)
                return;

            if (this.EntityTypeInfo.HasPrimaryKey) {
                TEntity? model = this.Entities.Find(this._TryGetEntityKeyValue(entity));
                if (model == null) {
                    this.Entities.Add(entity);
                } else {
                    if (_TryUpdateAllExceptPrimaryKey(this.EntityTypeInfo, model, entity, false) != 0) {
                        this.Entities.Update(model);
                    }
                }
            } else {
                this.Entities.Add(entity);
            }
        }

        public void SetAccesserId(string accesserId) {
            this._AccesserId = accesserId;
        }

        public bool TryAddOrUpdate(TEntity data, IDataTransResult? prevResult = null, bool execNow = true) {
            if (!this.IsAccessable)
                return false;
#pragma warning disable CS8602
            try {
                // 不立即執行則先設定為 false
                prevResult.TryAppendTransResult(false);

                this._AddOrUpdate(data);

                if (execNow) {
                    // 立即執行
                    this._Context.SaveChanges();
                    // 執行成功，
                    prevResult.BatchOnFinish(true);
                }
                return true;
            } catch (Exception ex) {
                this.HandleException(ex);
                if (prevResult != null)
                    prevResult.Message = ex.Message;

                if (!execNow)
                    return true;

                prevResult.BatchOnFinish(false);
                return false;
            }
#pragma warning restore CS8602
        }

        public IDataTransResult TryAddOrUpdate(IEnumerable<TEntity> dataInput, IDataTransResult? prevResult = null, bool execNow = true) {
            if (!this.IsAccessable || dataInput.IsNullOrEmpty())
                return this.EmptyResult;

            IDataTransResult result = prevResult ?? this.EmptyResult;
#pragma warning disable CS8602
            try {
                foreach (TEntity data in dataInput) {
                    result.TryAppendTransResult(false);

                    this._AddOrUpdate(data);

                }
                if (execNow) {
                    this._Context.SaveChanges();
                    result.BatchOnFinish(true);
                }
            } catch (Exception ex) {
                this.HandleException(ex);

                if (prevResult != null)
                    prevResult.Message = ex.Message;

                if (execNow)
                    result.BatchOnFinish(false);
            }
#pragma warning restore CS8602
            return result;
        }

        public bool TryAddOrUpdateSingle(object? dataInput, IDataTransResult? prevResult = null, bool execNow = true) {
            return this.TryAddOrUpdateSingleT(dataInput, prevResult, execNow);
        }

        public virtual void HandleException(Exception? exception) {
            if (exception == null)
                return;
            Console.WriteLine(exception.GetDetails().ToString());
        }
    }
}
