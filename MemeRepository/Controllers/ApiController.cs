using Cyh.DataHelper;
using Cyh.EFCore;
using Cyh.EFCore.Interface;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Db.Models;
using Microsoft.AspNetCore.Mvc;

namespace MemeRepository.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : MyControllerBase
    {
        IDbContext _DbContext;

        IMyDataAccesser<TAG>? _TagAccesser;
        IMyDataAccesser<IMAGE>? _ImageAccesser;
        IMyDataAccesser<TAG_BINDING>? _TagBindingAccesser;

        IMyDataAccesser<TAG>? TagAccesser {
            get {
                _TagAccesser ??= new DbEntityCRUD<TAG>(this._DbContext);
                return _TagAccesser;
            }
        }
        IMyDataAccesser<IMAGE>? ImageAccesser {
            get {
                _ImageAccesser ??= new DbEntityCRUD<IMAGE>(this._DbContext);
                return _ImageAccesser;
            }
        }
        IMyDataAccesser<TAG_BINDING>? TagBindingAccesser {
            get {
                _TagBindingAccesser ??= new DbEntityCRUD<TAG_BINDING>(this._DbContext);
                return _TagBindingAccesser;
            }
        }

        bool AllAccesserIsValid {
            get => TagAccesser != null && ImageAccesser != null && TagBindingAccesser != null;
            set { }
        }

        public ApiController(
            IWebAppConfigurations webAppConfigurations,
            IDbContext dbContext
            ) : base(webAppConfigurations) {
            this._DbContext = dbContext;
        }

        [Route("tag/get_ids")]
        [HttpGet]
        public IEnumerable<long> GetTagIds() {
            if(!AllAccesserIsValid)
                return Enumerable.Empty<long>();

            return TagAccesser?.TryGetDatasAs(x => x.ID) ?? Enumerable.Empty<long>();
        }
        [Route("tag/get_names")]
        [HttpGet]
        public IEnumerable<string> GetTagNames() {
            if (!AllAccesserIsValid)
                return Enumerable.Empty<string>();

            return TagAccesser?.TryGetDatasAs(x => x.NAME) ?? Enumerable.Empty<string>();
        }

        [Route("tag/get_all")]
        [HttpGet]
        public IEnumerable<TAG> GetTags() {
            if (!AllAccesserIsValid)
                return Enumerable.Empty<TAG>();

            return TagAccesser?.TryGetDatas() ?? Enumerable.Empty<TAG>();
        }
    }
}
