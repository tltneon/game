namespace gamelogic.Models
{
    public class WithToken
    {
        public string token { get; set; }
    }
    public class AuthData
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class StatsData
    {
        public string Basename { get; set; }
        public int ResearchLevel { get; set; }
    }
    public class BaseAction : WithToken
    {
        public int baseid { get; set; }
        public string action { get; set; }
        public string result { get; set; }
    }
    public class SquadAction : WithToken
    {
        public string key { get; set; }
        public string action { get; set; }
        public int to { get; set; }
    }
}