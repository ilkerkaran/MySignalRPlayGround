using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRServer.API.Hubs;
using SignalRServer.API.Services;

namespace SignalRServer.API.Controllers
{
    [Produces("application/json")]
    [Route("api/News")]
    public class NewsController : Controller
    {
        private readonly NewsService newsService;
        private readonly NewsHub newsHub;

        public NewsController(NewsService newsService, NewsHub newsHub)
        {
            this.newsService = newsService;
            this.newsHub = newsHub;
        }
        [HttpPost("[action]")]
        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> GenerateNews([FromBody] int topicId)
        {
            var news = newsService.GenerateNewNews(topicId);
            await newsHub.Send(news);
            return await Task.Run(() => Ok(news));
        }

        [HttpGet("{topicId}")]
        public async Task<IActionResult> Get(int topicId)
        {
            return await Task.Run(() => Ok(newsService.GetTopicNews(topicId)));
        }
    }
}