namespace chatsolution.Core.Services
{
    public class StockService : IStockService
    {
        IHttpClientFactory httpClient;

        public StockService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<string> QueryByCodeAsync(string code)
        {
            var cli = this.httpClient.CreateClient();
            
            var response = await cli.GetAsync($"https://stooq.com/q/l/?s={code}&f=sd2t2ohlcv&h&e=csv").ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
