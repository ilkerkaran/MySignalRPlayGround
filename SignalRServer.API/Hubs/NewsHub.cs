using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRServer.API.Model;
using SignalRServer.API.Services;

namespace SignalRServer.API.Hubs
{
    [Authorize]
    public class NewsHub : Hub
    {
        private readonly NewsService newsService;

        public NewsHub(NewsService newsService)
        {
            this.newsService = newsService;
        }
       
        public async Task Send((string groupName, string generatedNews) news)
        {
            if (!newsService.CheckTopic(news.groupName))
            {
                throw new System.Exception("cannot send a news item to a group which does not exist.");
            }
            if (Clients != null)
                await Clients.Group(news.groupName).SendAsync("NewsFeed", news.generatedNews);
        }

        public async Task JoinGroup(string groupName)
        {

            if (!newsService.CheckTopic(groupName))
            {
                throw new System.Exception("cannot join a group which does not exist.");
            }

            await Groups?.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients?.Group(groupName).SendAsync("JoinGroup", groupName);

            var history = newsService.GetTopicNews(groupName);
            await Clients?.Client(Context.ConnectionId).SendAsync("History", history);
        }

        public async Task LeaveGroup(string groupName)
        {
            if (!newsService.CheckTopic(groupName))
            {
                throw new System.Exception("cannot leave a group which does not exist.");
            }

            await Clients?.Group(groupName).SendAsync("LeaveGroup", groupName);
            await Groups?.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}