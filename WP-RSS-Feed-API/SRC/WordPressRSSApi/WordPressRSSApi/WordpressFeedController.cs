using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;

namespace WordPressRSSApi
{
    public class WordpressFeedController
    {
        private List<WordPressFeedItem> list = new List<WordPressFeedItem>();

        private static WordpressFeedController _instance;

        private WordpressFeedController() { }

        public static WordpressFeedController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WordpressFeedController();
                }
                return _instance;
            }
        }


        /// <summary>
        /// Removes all HTML Tags from a String
        /// </summary>
        /// <param name="text">The HTML Text</param>
        /// <returns>String</returns>
        public String RemoveHTMLFromText(String text)
        {
            return Regex.Replace(text, "<.*?>", String.Empty);
        }

        /// <summary>
        /// Loads a list of WordPressFeedItems
        /// </summary>
        /// <param name="url">Your Wordpress RSS Url (example: http://www.mydomain.de\/rss)</param>
        public void LoadRSS(String url)
        {
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            foreach (SyndicationItem item in feed.Items)
            {
                this.MapWordpressFeed(item);
            }
        }

        /// <summary>
        /// Loads a list of WordPressFeedItems
        /// </summary>
        /// <param name="Uri">Your Wordpress RSS Url (example: http://www.mydomain.de\/rss)</param>
        public void LoadRSS(Uri Uri)
        {
            this.LoadRSS(Uri.AbsoluteUri);
        }

        /// <summary>
        /// Adds an WordpressFeedItem to the private List
        /// (Converts the Syndication Item)
        /// </summary>
        /// <param name="item">Syndication Item</param>
        private void MapWordpressFeed(SyndicationItem item)
        {
            var wpItem = new WordPressFeedItem()
            {
                Id = item.Id,
                Title = item.Title.Text,
                Summary = item.Summary.Text,
                Url = item.Id.ToString(),
                PublishDate = item.PublishDate.DateTime
            };

            list.Add(wpItem);
        }

        private Random random = new Random();

        /// <summary>
        /// Returns an Random Wordpress Feed Item
        /// </summary>
        /// <returns></returns>
        public WordPressFeedItem GetRandomItem()
        {
            int selectionId = random.Next(0, list.Count);

            if (list[selectionId] != null)
                return list[selectionId];
            else
                return list.FirstOrDefault();
        }

        public IEnumerable<WordPressFeedItem> GetAll()
        {
            return list;
        }

        /// <summary>
        /// Returns the first Wordpress Feed Item in the list
        /// </summary>
        /// <returns></returns>
        public WordPressFeedItem GetFirstItem()
        {
            return list.FirstOrDefault();
        }

        /// <summary>
        /// Returns the last Wordpress Feed item in the list
        /// </summary>
        /// <returns></returns>
        public WordPressFeedItem GetLastItem()
        {
            return list.LastOrDefault();
        }

        /// <summary>
        /// Returns a list of Items, containing an specific text in the summary
        /// </summary>
        /// <param name="searchString">The String you are searching in the summary</param>
        /// <returns>IEnumerable</returns>
        public IEnumerable<WordPressFeedItem> GetItemSummaryContains(String searchString)
        {
            return list.Where(s => s.Summary.ToLower().Contains(searchString.ToLower()));
        }

        /// <summary>
        /// Returns a list of Items, containing an specific text in the Title
        /// </summary>
        /// <param name="searchString">The String you are searching in the summary</param>
        /// <returns>IEnumerable</returns>
        public IEnumerable<WordPressFeedItem> GetItemTitleContains(String searchString)
        {
            return list.Where(s => s.Title.ToLower().Contains(searchString.ToLower()));
        }

        /// <summary>
        /// Returns the newest Feed Item
        /// </summary>
        /// <returns></returns>
        public WordPressFeedItem GetNewestItem()
        {
            return list.OrderByDescending(d => d.PublishDate).FirstOrDefault();
        }

        /// <summary>
        /// Returns the oldest Feed Item
        /// </summary>
        /// <returns></returns>
        public WordPressFeedItem GetOldestItem()
        {
            return list.OrderBy(d => d.PublishDate).FirstOrDefault();
        }
    }
}
