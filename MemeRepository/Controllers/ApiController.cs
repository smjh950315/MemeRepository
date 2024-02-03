using Cyh.DataHelper;
using Cyh.EFCore;
using Cyh.EFCore.Interface;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using DocumentFormat.OpenXml.Bibliography;
using MemeRepository.Db.Models;
using Microsoft.AspNetCore.Mvc;
using Tag = MemeRepository.Db.Models.Tag;

namespace MemeRepository.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : MyControllerBase
    {
        IDbContext _DbContext;

        IMyDataAccesser<Tag>? _TagAccesser;
        IMyDataAccesser<Image>? _ImageAccesser;
        IMyDataAccesser<Category>? _CategoryAccesser;

        IMyDataAccesser<Tag>? TagAccesser {
            get {
                _TagAccesser ??= new DbEntityCRUD<Tag>(this._DbContext);
                return _TagAccesser;
            }
        }
        IMyDataAccesser<Image>? ImageAccesser {
            get {
                _ImageAccesser ??= new DbEntityCRUD<Image>(this._DbContext);
                return _ImageAccesser;
            }
        }
        IMyDataAccesser<Category>? CategoryAccesser {
            get {
                _CategoryAccesser ??= new DbEntityCRUD<Category>(this._DbContext);
                return _CategoryAccesser;
            }
        }

        bool AllAccesserIsValid {
            get => TagAccesser != null && ImageAccesser != null && CategoryAccesser != null;
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
            if (!AllAccesserIsValid)
                return Enumerable.Empty<long>();

            return TagAccesser?.TryGetDatasAs(x => x.TagID) ?? Enumerable.Empty<long>();
        }
        [Route("tag/get_names")]
        [HttpGet]
        public IEnumerable<string> GetTagNames() {
            if (!AllAccesserIsValid)
                return Enumerable.Empty<string>();

            return TagAccesser?.TryGetDatasAs(x => x.TagName) ?? Enumerable.Empty<string>();
        }

        [Route("tag/get_all")]
        [HttpGet]
        public IEnumerable<Tag> GetTags() {
            if (!AllAccesserIsValid)
                return Enumerable.Empty<Tag>();

            return TagAccesser?.TryGetDatas() ?? Enumerable.Empty<Tag>();
        }

        [Route("category/get_all")]
        [HttpGet]
        public IEnumerable<Category> GetCategory() {
            if (!AllAccesserIsValid)
                return Enumerable.Empty<Category>();

            return CategoryAccesser?.TryGetDatas().OrderByDescending(x => x.UpdateTime) ?? Enumerable.Empty<Category>();
        }

        [Route("category/Add_category")]
        [HttpPost]
        public bool AddCategory(string categoryName) {
            if (!AllAccesserIsValid)
                return false;
            var cat = new Category();
            cat.CategoryName = categoryName;
            cat.CreateTime = DateTime.Now;
            cat.UpdateTime = DateTime.Now;
            return CategoryAccesser.TryAddOrUpdateSingleT(cat);
        }
    }
}
