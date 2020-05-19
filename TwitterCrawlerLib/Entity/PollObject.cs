using TwitterCrawlerLib.Core.Data;

namespace TwitterCrawlerLib.Entity
{
    public class PollObject
    {
        public OptionObject[] options;
        public string end_datetime;
        public string duration_minutes;
    }
}
