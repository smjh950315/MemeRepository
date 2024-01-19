﻿#pragma warning disable IDE1006 // 命名樣式
using Cyh.DataHelper;

namespace Cyh.Modules.ModPager
{
    /// <summary>
    /// 資料的分頁清單
    /// </summary>
    /// <typeparam name="T">要分頁的資料</typeparam>
    public class MyPageList<T> : IPageList
    {
        delegate IPage FnCreateNewPage();
        FnCreateNewPage _Callback_CreateNewPage;

        int _PageCapacity;

        /// <summary>
        /// 建立新頁面的函數
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        static IPage __Impl_CreateNewPage<U>() where U : class, IPage, new() {
            return new U();
        }

#pragma warning disable
        /// <summary>
        /// 請使用 CreatePageList 建立實體
        /// </summary>
        protected MyPageList() { }
#pragma warning restore

        /// <summary>
        /// 建立新的分頁清單
        /// </summary>
        /// <typeparam name="U">要成為裝載內容的分頁的類型，必須繼承自 IPage</typeparam>
        /// <param name="pageCapacity">每頁容納的內容數</param>
        /// <returns>新建立的分頁清單</returns>
        public static MyPageList<T> CreatePageList<U>(int pageCapacity) where U : class, IPage, new() {

            MyPageList<T> ret = new() {
                _PageCapacity = pageCapacity,
                _Callback_CreateNewPage = __Impl_CreateNewPage<U>
            };
            return ret;
        }

        /// <summary>
        /// 取得空的頁面用來儲存資料
        /// </summary>
        /// <returns>新建的頁面</returns>
        public IPage CreateNewPage() {
            return this._Callback_CreateNewPage();
        }

        /// <summary>
        /// 資料的來源
        /// </summary>
        public IReadOnlyDataAccesser<T>? DataReader { get; set; }

        /// <summary>
        /// 頁數
        /// </summary>
        public int Count {
            get {
                if (this.DataReader == null) return 0;
                return this.DataReader.Count() / this.PageCapacity;
            }
        }

        /// <summary>
        /// 每一頁能容納的內容數量
        /// </summary>
        public int PageCapacity {
            get => this._PageCapacity;
        }

        /// <summary>
        /// 取得指定頁的資料
        /// </summary>
        /// <param name="page_index">頁碼</param>
        /// <returns>取得僅供讀取的頁面資料</returns>
        public IReadOnlyPage GetPage(int page_index) {
#pragma warning disable
            if (page_index < 1 || this.DataReader == null) return null;
            int beginIndex = (page_index - 1) * this.PageCapacity;
            var collection = this.DataReader.TryGetDatas(beginIndex, this.PageCapacity, null);
#pragma warning restore

            if (collection == null)
                return this.CreateNewPage();

            IPage newPage = this.CreateNewPage();
            newPage.SetPageItems(collection);

            return newPage;
        }

        /// <summary>
        /// 設定資料來源
        /// </summary>
        /// <param name="dataReader">資料來源的實體</param>
        /// <returns>是否設定成功</returns>
        public bool SetDataSource(IReadOnlyDataAccesser<T> dataReader) {
            this.DataReader = dataReader;
            return this.DataReader != null;
        }

        public bool SetDataSource(IReadOnlyDataAccesser dataReader) {
            if (dataReader is IReadOnlyDataAccesser<T> ds)
                return this.SetDataSource(ds);
            return false;
        }
    }
}
#pragma warning restore IDE1006 // 命名樣式