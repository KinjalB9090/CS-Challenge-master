using System;

namespace JokeGenerator
{
    public class Joke
    {
        public string[] categories { get; set; }
        public DateTime created_at { get; set; }
        public string icon_url { get; set; }
        public string id { get; set; }
        public DateTime updated_at { get; set; }
        public string url { get; set; }
        public string value { get; set; }
    }
}
