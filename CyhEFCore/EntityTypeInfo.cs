using Cyh.EFCore.Interface;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace Cyh.EFCore
{
    public class EntityTypeInfo
    {
        IEnumerable<MemberInfo>? _Attributes;
        void _SetToInvaild() {
            this.IsValid = false;
            this.HasPrimaryKey = false;
            this.PrimaryKeyInfo = null;
        }
        void _LoadFromDbContext(IDbContext dbContext, Type modelType) {
            try {
                IModel model = dbContext.Model;
                IEntityType? entityType = model.FindEntityType(modelType);
                if (entityType == null) {
                    this._SetToInvaild();
                    return;
                } else {
                    IKey? primaryKey = entityType.FindPrimaryKey();
                    this.HasPrimaryKey = primaryKey != null;
                    if (this.HasPrimaryKey) {
#pragma warning disable CS8600, CS8602, CS8604
                        string keyName = primaryKey.Properties.Select(x => x.Name).FirstOrDefault();
                        this.PrimaryKeyInfo = modelType.GetMember(keyName).FirstOrDefault();
#pragma warning restore CS8600, CS8602, CS8604
                        this._Attributes = modelType.GetMembers().Where(x => x.Name != keyName);
                    } else {
                        this._Attributes = modelType.GetMembers();
                    }
                }
                this.IsValid = true;
            } catch {
                this._SetToInvaild();
            }
        }

        public bool IsValid { get; private set; }

        public bool HasPrimaryKey { get; private set; }

        public MemberInfo? PrimaryKeyInfo { get; private set; }

        public IEnumerable<MemberInfo> Attributes {
            get => this._Attributes ?? Enumerable.Empty<MemberInfo>();
        }

        public void Reload(IDbContext? dbContext, Type? modelType) {
            if (dbContext == null || modelType == null) {
                this._SetToInvaild();
            } else {
                this._LoadFromDbContext(dbContext, modelType);
            }
        }

        public EntityTypeInfo(IDbContext? dbContext, Type? modelType) {
            if (dbContext == null || modelType == null) {
                this._SetToInvaild();
            } else {
                this._LoadFromDbContext(dbContext, modelType);
            }
        }
    }
}
