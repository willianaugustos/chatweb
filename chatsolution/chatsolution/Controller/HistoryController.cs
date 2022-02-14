using chatsolution.Data;
using chatsolution.Models;
using Microsoft.AspNetCore.Mvc;

namespace chatsolution.Controller
{
    [ApiController]
    [Route("api/history")]
    public class HistoryController : ControllerBase
    {
        IMessageRepository messageRepository;

        public HistoryController(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }
        [HttpGet]
        public async Task<List<HistoryModel>> Index()
        {
            var history = await this.messageRepository.QueryLastMessages(50);
            var historyModelList = history.ConvertAll(r => new HistoryModel(r.From, r.DateTime, r.Text));
            return Enumerable.Reverse(historyModelList).ToList();
        }
    }
}
