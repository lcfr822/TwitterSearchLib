namespace TwitterCrawlerLib.Core.Data
{
    public class User
    {
        public long? id;
        public string id_str;
        public string name;
        public string screen_name;
        public string location;
        public string url;
        public string description;
        public bool _protected;
        public bool verified;
        public int followers_count;
        public int friends_count;
        public int favourites_count;
        public int statuses_count;
        public string created_at;
        public string profile_banner_url;
        public string profile_image_url_https;
        public bool default_profile;
        public bool default_profile_image;
        public string[] withheld_in_countries;
        public string withheld_scope;

        public User() { }
    }
}
