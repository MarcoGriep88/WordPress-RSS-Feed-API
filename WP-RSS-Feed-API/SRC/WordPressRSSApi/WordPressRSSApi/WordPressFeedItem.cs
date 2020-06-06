using System;

namespace WordPressRSSApi
{
    public class WordPressFeedItem
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public String Summary { get; set; }
        public String Url { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
