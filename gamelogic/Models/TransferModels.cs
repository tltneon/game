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
        public int UserID { get; set; }
        public string Playername { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public string Basename { get; set; }
        public int Level { get; set; }
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


    public class ReturnCode
    {
        public bool success { get; set; }
    }
    public class ReturnMessage : ReturnCode
    {
        public string message { get; set; }
    }
    public class ReturnAuthData : ReturnMessage
    {
        public string token { get; set; }
        public int role { get; set; }
    }
}