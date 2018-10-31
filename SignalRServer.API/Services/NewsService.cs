using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalRServer.API.Services
{
    public class NewsService
    {

        private const int politicsId = 1;
        private const int crimeId = 2;
        private const int technologyId = 3;
        private const int worldNewsId = 4;
        private const int sportsId = 5;

        private readonly Dictionary<int, string> topics;
        private Dictionary<int, List<string>> topicNews;
        public NewsService()
        {
            topics = new Dictionary<int, string>();
            topics.Add(politicsId, "Politics");
            topics.Add(crimeId, "Crime");
            topics.Add(technologyId, "Technology");
            topics.Add(worldNewsId, "World News");
            topics.Add(sportsId, "Sports");
            topicNews = new Dictionary<int, List<string>>();
            topicNews.Add(1, GetSeedPoliticsNews());
            topicNews.Add(2, GetSeedCrimesNews());
            topicNews.Add(3, GetSeedTechnologysNews());
            topicNews.Add(4, GetSeedWorldNewsNews());
            topicNews.Add(5, GetSeedSportsNews());
        }

        public string GenerateNewNews(int topicId)
        {
            if (topics.TryGetValue(topicId, out var topicName))
            {
                var generatedNews = $"Generated {topicName} news _ {DateTime.UtcNow}";
                topicNews[topicId].Add(generatedNews);
                return generatedNews;
            }
            else
                return string.Empty;
        }

        public string GenerateNewNews(string topicName)
        {
            var topicItem = topics.FirstOrDefault(topic => string.Equals(topic.Value, topicName, StringComparison.CurrentCultureIgnoreCase));
            return GenerateNewNews(topicItem.Key);

        }

        public List<string> GetTopicNews(int topicId)
        {
            if (topicNews.TryGetValue(topicId, out var topicContent))
            {
                return topicContent;
            }
            else
                return null;
        }

        public List<string> GetTopicNews(string topicName)
        {
            var topicItem = topics.FirstOrDefault(topic => string.Equals(topic.Value, topicName, StringComparison.CurrentCultureIgnoreCase));
            return GetTopicNews(topicItem.Key);
        }

        public bool CheckTopic(string topicName)
        {
            return topics.Values.Any(topic => string.Equals(topic, topicName, StringComparison.CurrentCultureIgnoreCase));
        }


        #region Seed methods
        private List<string> GetSeedSportsNews()
        {
            return new List<string>
            {
                "Seed Sports News1",
                "Seed Sports News2",
                "Seed Sports News3"
            };
        }



        private List<string> GetSeedWorldNewsNews()
        {
            return new List<string>
            {
                "Seed WorldNews News1",
                "Seed WorldNews News2",
                "Seed WorldNews News3"
            };
        }

        private List<string> GetSeedTechnologysNews()
        {
            return new List<string>
            {
                "Seed Technology News1",
                "Seed Technology News2",
                "Seed Technology News3"
            };
        }

        private List<string> GetSeedCrimesNews()
        {
            return new List<string>
            {
                "Seed Crimes News1",
                "Seed Crimes News2",
                "Seed Crimes News3"
            };
        }

        private List<string> GetSeedPoliticsNews()
        {
            return new List<string>
            {
                "Seed Politics News1",
                "Seed Politics News2",
                "Seed Politics News3"
            };
        }
        #endregion

    }
}