namespace chatsolution.Core.Services
{
    public class StockService : IStockService
    {
        IHttpClientFactory httpClient;
        IConfiguration configuration;
        private readonly string stooqUrl;

        public StockService(IHttpClientFactory httpClient, IConfiguration Configuration)
        {
            this.httpClient = httpClient;
            this.configuration = Configuration;

            this.stooqUrl = this.configuration.GetValue<string>("StooqUrl");
        }
        public async Task<string> QueryByCodeAsync(string code)
        {
            var cli = this.httpClient.CreateClient();
            
            //lookup at stooq service the information about that quote
            var response = await cli.GetAsync(stooqUrl.Replace("{code}", code)).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
