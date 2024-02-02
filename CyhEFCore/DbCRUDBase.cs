using Cyh.EFCore.Interface;

namespace Cyh.EFCore
{
    public class DbCRUDBase : IDbCRUD
    {
        void _CreateModel<T>(T _model) where T : class {
            if (this._Context == null) return;

            this._Context.Add(_model);
            this._Context.SaveChanges();
        }
        void _DeleteModel<T>(T _model) where T : class {
            if (this._Context == null) return;

            this._Context.Remove(_model);
            this._Context.SaveChanges();
        }
        void _UpdateModel<T>(T _model) where T : class {
            if (this._Context == null) return;

            var ent = this.GetDbSet<T>();
            if (ent != null) {
                ent.Update(_model);
                this._Context.SaveChanges();
            }
        }

        public IDbContext? _Context { get; set; }

        public virtual bool IsAccessable => this._Context != null;

        public DbCRUDBase() { }

        public DbCRUDBase(IDbContext? context) {
            this._Context = context;
        }

        /// <summary>
        /// 新增 Model
        /// </summary>
        /// <typeparam name="T"> Model 類型</typeparam>
        /// <param name="model"> Model 實體 </param>
        /// <returns>成功與否</returns>
        public bool Create<T>(T model) where T : class {
            if (this._Context == null || model == null) return false;
            return TryExecute(fn => this._CreateModel(model));
        }

        /// <summary>
        /// 刪除 Model
        /// </summary>
        /// <typeparam name="T"> Model 類型</typeparam>
        /// <param name="model"> Model 實體 </param>
        /// <returns>成功與否</returns>
        public bool Delete<T>(T model) where T : class {
            if (this._Context == null || model == null) return false;
            return TryExecute(fn => this._DeleteModel(model));
        }

        /// <summary>
        /// 更新 Model
        /// </summary>
        /// <typeparam name="T"> Model 類型</typeparam>
        /// <param name="model"> Model 實體 </param>
        /// <returns>成功與否</returns>
        public bool Update<T>(T model) where T : class {
            if (this._Context == null || model == null) return false;
            return TryExecute(fn => this._UpdateModel(model));
        }
    }
}
