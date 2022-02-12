namespace chatsolution.Core.Services
{
    public interface IStockService
    {
        Task<string> QueryByCodeAsync(string code);
    }
}
