using System.Linq.Expressions;

namespace Cyh.DataModels
{
    public interface ICastableModelBase<TThisModel, IDataSurface> where TThisModel : class, IDataSurface, new()
    {
        /// <summary>
        /// 模型間以相同介面互相轉換的選擇器
        /// </summary>
        /// <typeparam name="From">來源資料模型，必須要與當前模型有相同介面屬性</typeparam>
        /// <returns>轉換用的選擇器</returns>
        Expression<Func<From, To>> GetSelectorFromTo<From, To>() where From : IDataSurface where To : class, IDataSurface, new();

        void UpdateFromTo<From, To>(From from, To to) where From : IDataSurface where To : IDataSurface;
    }

    /// <summary>
    /// 可以將擁有相同介面屬性的來源資料模型轉換成當前類型的介面
    /// </summary>
    /// <typeparam name="TThisModel">當前模型的類別</typeparam>
    /// <typeparam name="IDataSurface">當前模型要擷取資料的介面屬性</typeparam>
    public interface ISelectableDestModel<TThisModel, IDataSurface>
        : ICastableModelBase<TThisModel, IDataSurface>
        where TThisModel : class, IDataSurface, new()
    {
        /// <summary>
        /// 將輸入的資料模型轉換成當前類型的選擇器
        /// </summary>
        /// <typeparam name="TSrcData">來源資料模型，必須要與當前模型有相同介面屬性</typeparam>
        /// <returns>轉換用的選擇器</returns>
        Expression<Func<TSrcData, TThisModel>> GetSelectorFrom<TSrcData>() where TSrcData : IDataSurface {
            return this.GetSelectorFromTo<TSrcData, TThisModel>();
        }
        void UpdateFrom<From>(From from) where From : IDataSurface {
            this.UpdateFromTo(from, this.Self());
        }
        TThisModel Self();
    }

    /// <summary>
    /// 可以將當前類型轉換成擁有相同介面屬性的資料資料模型介面
    /// </summary>
    /// <typeparam name="TThisModel">當前模型的類別</typeparam>
    /// <typeparam name="IDataSurface">當前模型要擷取資料的介面屬性</typeparam>
    public interface ISelectableSrcsModel<TThisModel, IDataSurface>
        : ICastableModelBase<TThisModel, IDataSurface>
        where TThisModel : class, IDataSurface, new()
    {
        /// <summary>
        /// 將輸入的資料模型轉換成當前類型的選擇器
        /// </summary>
        /// <typeparam name="TDstData">目的資料模型，必須要與當前模型有相同介面屬性</typeparam>
        /// <returns>轉換用的選擇器</returns>
        Expression<Func<TThisModel, TDstData>> GetSelectorTo<TDstData>() where TDstData : class, IDataSurface, new() {
            return this.GetSelectorFromTo<TThisModel, TDstData>();
        }
        void UpdateTo<To>(To dst) where To : class, IDataSurface {
            this.UpdateFromTo(this.Self(), dst);
        }
        TThisModel Self();
    }

    /// <summary>
    /// 可以在相同介面間互相轉換的資料模型 
    /// <para>在 GetSelectorFromTo( ) 有實作的情況下</para>
    /// <para>
    /// GetSelectorFrom( ) 以及 GetSelectorTo( ) 會自行調用 GetSelectorFromTo()做轉換
    /// </para>
    /// <para>
    /// 如果想針對 From 或是 To Selector 特製轉換方法，可以自行實作 GetSelectorFrom( ) 以及 GetSelectorTo( )
    /// </para>
    /// </summary>
    /// <typeparam name="TThisModel">當前類型</typeparam>
    /// <typeparam name="IDataSurface">與當前要互通的介面</typeparam>
    public interface ISelectableModel<TThisModel, IDataSurface>
        : ISelectableDestModel<TThisModel, IDataSurface>
        , ISelectableSrcsModel<TThisModel, IDataSurface>
        where TThisModel : class, IDataSurface, new()
    {
    }
}
