namespace gamelogic.Models
{
    public class AuthData
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class StatsData
    {
        public string username { get; set; }
        public string wins { get; set; }
        public string loses { get; set; }
        public string basename { get; set; }
        public string baselevel { get; set; }
    }
    public class BaseAction
    {
        public int baseid { get; set; }
        public string action { get; set; }
        public string result { get; set; }
        public string token { get; set; }
    }
    public class SquadAction
    {
        public string key { get; set; }
        public string action { get; set; }
        public int to { get; set; }
        public string token { get; set; }
    }
}