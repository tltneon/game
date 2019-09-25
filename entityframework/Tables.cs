namespace entityframework
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password{ get; set; }
        public short Role { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
    }
    public class Base
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Owner { get; set; }
        public int Credits { get; set; }
        public int Energy { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public int Level { get; set; }
        public string CurrentTask { get; set; }
        public int FinishTime { get; set; }
        public bool isActive { get; set; }
        public string Units { get; set; }
    }
    public class Building
    {
        public int ID { get; set; }
        public int Level { get; set; }
        public int BaseID { get; set; }
        public string Type { get; set; }
        public string CurrentTask { get; set; }
        public int FinishTime { get; set; }
    }
    public class Squad
    {
        public int ID { get; set; }
        public int Owner { get; set; }
        public int MoveFrom { get; set; }
        public int StartTime { get; set; }
        public int MoveTo { get; set; }
        public int FinishTime { get; set; }
        public string Units { get; set; }
    }
}
