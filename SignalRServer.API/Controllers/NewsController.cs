using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalRServer.API.Services;

namespace SignalRServer.API.Controllers
{
    [Produces("application/json")]
    [Route("api/News")]
    public class NewsController : Controller
    {
        private readonly NewsService newsService;

        public NewsController(NewsService newsService)
        {
            this.newsService = newsService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> GenerateNews([FromBody] int topicId)
        {
            return await Task.Run(() => Ok(newsService.GenerateNewNews(topicId)));
        }

        [HttpGet("{topicId}")]
        public async Task<IActionResult> Get(int topicId)
        {
            return await Task.Run(() => Ok(newsService.GetTopicNews(topicId)));
        }
    }
}