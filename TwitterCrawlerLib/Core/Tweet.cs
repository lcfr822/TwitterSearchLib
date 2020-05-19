using TwitterCrawlerLib.Core.Data;
using TwitterCrawlerLib.Entity;

namespace TwitterCrawlerLib.Core
{
    public class Tweet
    {
        public string created_at;
        public long? id;
        public string id_str;
        public string text;
        public string source;
        public bool truncated;
        public long? in_reply_to_status_id;
        public string in_reply_to_status_id_str;
        public long? in_reply_to_user_id;
        public string in_reply_to_user_id_str;
        public string in_reply_to_screen_name;
        public User user;
        public Coordinates coordinates;
        public PlaceObject place;
        public long? quoted_status_id;
        public string quoted_status_id_str;
        public bool is_quote_status;
        public Tweet quoted_status;
        public Tweet retweeted_status;
        public int quote_count;
        public int reply_count;
        public int retweet_count;
        public int favorite_count;
        public EntitiesObject entities;
        // Extended Entities WIP
        public bool favorited;
        public bool retweeted;
        public bool possibly_sensitive;
        public string filter_level;
        public string lang;
        public MatchingRulesObject matching_rules;
    }
}
