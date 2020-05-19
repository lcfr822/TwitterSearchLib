using TwitterCrawlerLib.Core;

namespace TwitterCrawlerLib.Web
{
    public class APISearchResult
    {
        public Tweet[] statuses;
        public Metadata search_metadata;
    }

    public class Metadata
    {
        public float completed_in;
        public long? max_id;
        public string max_id_str;
        public string next_results;
        public string query;
        public string refresh_url;
        public int count;
        public int since_id;
        public string since_id_str;
    }
}
