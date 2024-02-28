using Cyh.DataHelper;
using Cyh.DataModels;
using Cyh.EFCore.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Cyh.EFCore
{
    public enum ERR_ACTION_TYPE
    {
        ON_EXECUTING,
        ON_SAVING,
        ON_FETCHING
    }

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
        public DbSet<TEntity>? Entities => this.GetDbSet<TEntity>(ref this._Entities);

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

        /// <summary>
        /// 指出當前資料源是否可以存取
        /// </summary>
        /// <param name="dataTransResult"></param>
        /// <returns>當前資料源是否可以存取，如果不行，回傳false並將結果寫入 dataTransResult</returns>
        private bool _CanAccessDataSource(IDataTransResult? dataTransResult) {
            if (!this.IsAccessable) {
                if (dataTransResult != null) {
                    dataTransResult.Message = "Cannot open data source, transaction is suspended!";
                    dataTransResult.BatchOnFinish(false);
                }
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// 發生例外時呼叫，用來寫入交易紀錄並決定是否繼續執行
        /// </summary>
        /// <param name="dataTransResult">交易紀錄</param>
        /// <param name="ex">原生的例外狀況</param>
        /// <param name="type">執行中的動作類型</param>
        /// <param name="isLastBatch">是否是最後一個執行批次</param>
        /// <returns></returns>
        private bool _HandleExcetionAndLog(IDataTransResult? dataTransResult, Exception? ex, ERR_ACTION_TYPE type, bool isLastBatch) {
            if (isLastBatch) { // 最後一個執行批次
                if (dataTransResult != null) {
                    // 須要紀錄結果，開始整理訊息
                    StringBuilder sb = new StringBuilder();
                    // 紀錄失敗時正在執行的動作
                    switch (type) {
                        case ERR_ACTION_TYPE.ON_EXECUTING:
                            sb.AppendLine("Failed on executing\nRow message:\n");
                            break;
                        case ERR_ACTION_TYPE.ON_FETCHING:
                            sb.AppendLine("Failed on fetching\nRow message:\n");
                            break;
                        case ERR_ACTION_TYPE.ON_SAVING:
                            sb.AppendLine("Failed on saving\nRow message:\n");
                            break;
                        default:
                            sb.AppendLine("Unknown failure!\nRow message:\n");
                            break;
                    }
                    // 加入原始的 exception 訊息
                    if (ex != null) {
                        sb.AppendLine(ex.Message);
                    }
                    // 記錄執行結果
                    dataTransResult.Message = sb.ToString();
                    // 由於是最後一個執行批次，故將結果打上失敗標記並鎖定交易紀錄
                    dataTransResult.BatchOnFinish(false);
                }
                // 呼叫內部的例外處理
                this.HandleException(ex);
                // 由於是最後一個執行批次，故回傳 false 表示不可再執行
                return false;
            } else {
                if (dataTransResult != null) {
                    // 紀錄處理這筆資料發生的例外
                    dataTransResult.TryAppendTransResult(false, ex?.Message);
                }
                // 呼叫內部的例外處理
                this.HandleException(ex);
                // 表示後續還有其他執行批次，故回傳 true 表示可以繼續執行
                return true;
            }
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

        private void _Remove(TEntity? entity) {
            if (entity == null || !this.EntityTypeInfo.IsValid || this.Entities == null)
                return;

            if (this.EntityTypeInfo.HasPrimaryKey) {
                TEntity? model = this.Entities.Find(this._TryGetEntityKeyValue(entity));
                if (model != null) {
                    this.Entities.Remove(model);
                }
            } else {
                this.Entities.Remove(entity);
            }
        }

        public void SetAccesserId(string accesserId) {
            this._AccesserId = accesserId;
        }

        public bool TryAddOrUpdate(TEntity? data, IDataTransResult? dataTransResult, bool execNow) {

            if (!this._CanAccessDataSource(dataTransResult)) {
                // 當前資料來源無法使用，結束整批交易
                return false;
            }
#pragma warning disable CS8602
            try {
                this._AddOrUpdate(data);
                if (execNow) {
                    // 立即執行
                    this._Context.SaveChanges();
                    // 此為最後一個執行批次，故加入一次成功結果
                    dataTransResult.TryAppendTransResult(true);
                    // 執行成功，打上成功標記
                    dataTransResult.BatchOnFinish(true);
                } else {
                    // 不立即執行則先設定為 false
                    dataTransResult.TryAppendTransResult(false);
                }
                return true;
            } catch (Exception ex) {
                return this._HandleExcetionAndLog(dataTransResult, ex, ERR_ACTION_TYPE.ON_SAVING, execNow);
            }
#pragma warning restore CS8602
        }

        public bool TryAddOrUpdate(IEnumerable<TEntity> dataInput, IDataTransResult? dataTransResult, bool execNow) {

            if (!this._CanAccessDataSource(dataTransResult)) {
                // 當前資料來源無法使用，結束整批交易
                return false;
            }

#pragma warning disable CS8602, CS8603
            try {
                foreach (TEntity data in dataInput) {
                    // 先不立即執行故設定為 false
                    dataTransResult.TryAppendTransResult(false);
                    this._AddOrUpdate(data);
                }
                if (execNow) {
                    this._Context.SaveChanges();
                    dataTransResult.BatchOnFinish(true);
                    return true;
                } else {
                    return true;
                }
            } catch (Exception ex) {
                return this._HandleExcetionAndLog(dataTransResult, ex, ERR_ACTION_TYPE.ON_SAVING, execNow);
            }
#pragma warning restore CS8602, CS8603
        }

        public bool TryAddOrUpdateSingle(object? dataInput, IDataTransResult? prevResult, bool execNow) {
            return this.TryAddOrUpdateSingleT(dataInput, prevResult, execNow);
        }

        public virtual void HandleException(Exception? exception) {
            if (exception == null)
                return;
            Console.WriteLine(exception.GetDetails().ToString());
        }

        public TEntity? TryGetData(Expression<Func<TEntity, bool>>? filter, IDataTransResult? dataTransResult) {
            if (this.Entities == null) {
                dataTransResult.TryAppendError_DataAccesserNotInit();
                return default;
            }

            try {
                if (filter == null)
                    return this.Entities.FirstOrDefault() ?? default;
                else
                    return this.Entities.Where(filter).FirstOrDefault() ?? default;
            } catch (Exception? ex) {
                dataTransResult.TryAppendError_FailedOnFetching();
                this.HandleException(ex);
                return default;
            }
        }

        public IEnumerable<TEntity> TryGetDatas(Expression<Func<TEntity, bool>>? filter, IDataTransResult? dataTransResult) {
            if (this.Entities == null) {
                dataTransResult.TryAppendError_DataAccesserNotInit();
                return Enumerable.Empty<TEntity>();
            }

            try {
                if (filter == null)
                    return this.Entities.ToList();
                else
                    return this.Entities.Where(filter).ToList();
            } catch (Exception? ex) {
                dataTransResult.TryAppendError_FailedOnFetching();
                this.HandleException(ex);
                return Enumerable.Empty<TEntity>();
            }
        }

        public IEnumerable<TEntity> TryGetDatas(int begin, int count, Expression<Func<TEntity, bool>>? filter, IDataTransResult? dataTransResult) {
            if (count <= 0) {
                dataTransResult.TryAppendError_InvalidDataRange();
                return Enumerable.Empty<TEntity>();
            }
            if (this.Entities == null) {
                dataTransResult.TryAppendError_DataAccesserNotInit();
                return Enumerable.Empty<TEntity>();
            }

            try {
                if (begin < 0)
                    begin = 0;
                if (filter == null)
                    return this.Entities?.Skip(begin).Take(count).ToList() ?? Enumerable.Empty<TEntity>();
                else
                    return this.Entities.Where(filter).Skip(begin).Take(count).ToList();
            } catch (Exception? ex) {
                dataTransResult.TryAppendError_FailedOnFetching();
                this.HandleException(ex);
                return Enumerable.Empty<TEntity>();
            }
        }

        public TOut? TryGetDataAs<TOut>(Expression<Func<TEntity, TOut>>? selctor, Expression<Func<TEntity, bool>>? filter, IDataTransResult? dataTransResult) {
            if (this.Entities == null) {
                dataTransResult.TryAppendError_DataAccesserNotInit();
                return default;
            }
            if (selctor == null) {
                dataTransResult.TryAppendError_CannotConvertInput();
                return default;
            }

            try {
                if (filter == null)
                    return this.Entities.Select(selctor).FirstOrDefault() ?? default;
                else
                    return this.Entities.Where(filter).Select(selctor).FirstOrDefault() ?? default;
            } catch (Exception? ex) {
                dataTransResult.TryAppendError_FailedOnFetching();
                this.HandleException(ex);
                return default;
            }
        }

        public IEnumerable<TOut> TryGetDatasAs<TOut>(Expression<Func<TEntity, TOut>>? selctor, Expression<Func<TEntity, bool>>? filter, IDataTransResult? dataTransResult) {
            if (this.Entities == null) {
                dataTransResult.TryAppendError_DataAccesserNotInit();
                return Enumerable.Empty<TOut>();
            }
            if (selctor == null) {
                dataTransResult.TryAppendError_CannotConvertInput();
                return Enumerable.Empty<TOut>();
            }
            try {
                if (filter == null)
                    return this.Entities.Select(selctor).ToList();
                else
                    return this.Entities.Where(filter).Select(selctor).ToList();
            } catch (Exception? ex) {
                dataTransResult.TryAppendError_FailedOnFetching();
                this.HandleException(ex);
                return Enumerable.Empty<TOut>();
            }
        }

        public IEnumerable<TOut> TryGetDatasAs<TOut>(Expression<Func<TEntity, TOut>>? selctor, int begin, int count, Expression<Func<TEntity, bool>>? filter, IDataTransResult? dataTransResult) {
            if (count <= 0) {
                dataTransResult.TryAppendError_InvalidDataRange();
                return Enumerable.Empty<TOut>();
            }
            if (this.Entities == null) {
                dataTransResult.TryAppendError_DataAccesserNotInit();
                return Enumerable.Empty<TOut>();
            }
            if (selctor == null) {
                dataTransResult.TryAppendError_CannotConvertInput();
                return Enumerable.Empty<TOut>();
            }
            try {
                if (begin < 0)
                    begin = 0;
                if (filter == null)
                    return this.Entities?.Skip(begin).Take(count).Select(selctor).ToList() ?? Enumerable.Empty<TOut>();
                else
                    return this.Entities.Where(filter).Skip(begin).Take(count).Select(selctor).ToList();
            } catch (Exception? ex) {
                dataTransResult.TryAppendError_FailedOnFetching();
                this.HandleException(ex);
                return Enumerable.Empty<TOut>();
            }
        }

        public bool ApplyChanges(IDataTransResult? prevResult) {
            if (this._Context == null) {
                prevResult.TryAppendError_DataAccesserNotInit();
                prevResult.BatchOnFinish(false);
                return false;
            }
            try {
                this._Context.SaveChanges();
                prevResult.BatchOnFinish(true);
                return true;
            } catch (Exception ex) {
                prevResult.TryAppendError_FailedOnApplyChanges();
                this.HandleException(ex);
                prevResult.BatchOnFinish(false);
                return false;
            }
        }

        public bool TryRemove(TEntity? data, IDataTransResult? dataTransResult, bool execNow) {
            if (!this._CanAccessDataSource(dataTransResult)) {
                // 當前資料來源無法使用，結束整批交易
                return false;
            }
#pragma warning disable CS8602
            try {
                this._Remove(data);
                if (execNow) {
                    // 立即執行
                    this._Context.SaveChanges();
                    // 此為最後一個執行批次，故加入一次成功結果
                    dataTransResult.TryAppendTransResult(true);
                    // 執行成功，打上成功標記
                    dataTransResult.BatchOnFinish(true);
                } else {
                    // 不立即執行則先設定為 false
                    dataTransResult.TryAppendTransResult(false);
                }
                return true;
            } catch (Exception ex) {
                return this._HandleExcetionAndLog(dataTransResult, ex, ERR_ACTION_TYPE.ON_SAVING, execNow);
            }
#pragma warning restore CS8602
        }

        public bool TryRemoveSingle(object? data, IDataTransResult? dataTransResult, bool execNow) {
            return this.TryRemoveSingleT(data, dataTransResult, execNow);
        }

        public bool TryRemove(Expression<Func<TEntity, bool>> expression, IDataTransResult? dataTransResult, bool execNow) {
            if (!this._CanAccessDataSource(dataTransResult)) {
                // 當前資料來源無法使用，結束整批交易
                return false;
            }
#pragma warning disable CS8602
            try {
                IEnumerable<TEntity>? entity = this.Entities?.Where(expression);
                if (!entity.IsNullOrEmpty()) {
                    this.Entities.RemoveRange(entity);
                }
                if (execNow) {
                    // 立即執行
                    this._Context.SaveChanges();
                    // 此為最後一個執行批次，故加入一次成功結果
                    dataTransResult.TryAppendTransResult(true);
                    // 執行成功，打上成功標記
                    dataTransResult.BatchOnFinish(true);
                } else {
                    // 不立即執行則先設定為 false
                    dataTransResult.TryAppendTransResult(false);
                }
                return true;
            } catch (Exception ex) {
                return this._HandleExcetionAndLog(dataTransResult, ex, ERR_ACTION_TYPE.ON_SAVING, execNow);
            }
#pragma warning restore CS8602
        }

        public bool Any(Expression<Func<TEntity, bool>>? predicate) {
            if (this.Entities == null) { return false; }
            if (predicate == null) { return this.Entities.Any(); }
            return this.Entities.Any(predicate);
        }
    }
}
