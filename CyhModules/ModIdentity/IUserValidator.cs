namespace Cyh.Modules.ModIdentity
{
    /// <summary>
    /// 使用者身分驗證器，通常透過DB存取類別庫繼承
    /// </summary>
    public interface IUserValidator
    {
        string? GetUserIdIfValid(string? account, string? password);
    }
}
