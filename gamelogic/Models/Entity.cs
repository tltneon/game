using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace gamelogic
{
    /// <summary>
    /// Класс, наследующий механизм обнуления ДБ при изменении модели
    /// </summary>
    class EntitiesInitializer : DropCreateDatabaseIfModelChanges<Entities>
    {
        /// <summary>
        /// Записывает дефолтные данные в БД
        /// </summary>
        /// <param name="db"></param>
        protected override void Seed(Entities db)
        {
            db.Accounts.Add(new Account { UserID = 1, Username = "admin", Password = AccountManager.Base64Encode("123456"), Role = 1, Token = "Tokenfgio" });
            db.Accounts.Add(new Account { UserID = 2, Username = "user", Password = AccountManager.Base64Encode("123"), Role = 0, Token = "Tokenfl4o" });
            db.Players.Add(new Player { UserID = 1, Playername = "admin" });
            db.Players.Add(new Player { UserID = 2, Playername = "user" });
            db.Bases.Add(new Base { Basename = "adminBase", OwnerID = 1, CoordX = 1, CoordY = 1, Level = 1, IsActive = true });
            db.Bases.Add(new Base { Basename = "userBase", OwnerID = 2, CoordX = 4, CoordY = 2, Level = 1, IsActive = true });
            db.Resources.Add(new Resource { Instance = "bas1", Credits = 200, Energy = 200, Neutrino = 0.0 });
            db.Resources.Add(new Resource { Instance = "bas2", Credits = 200, Energy = 200, Neutrino = 0.0 });
            db.Units.Add(new Unit { Instance = "bas1", Type = "droneUnit", Count = 1 });
            db.Units.Add(new Unit { Instance = "bas2", Type = "droneUnit", Count = 2 });
            db.SaveChanges();
            ProceedActions.Log("Event", "Database default data successfully filled. EntitiesInitializer.Seed");
        }
    }

    /// <summary>
    /// Контекст для общения с базой
    /// </summary>
    public class Entities : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ProceedActions.Log("Event", "Database model successfully created. Entities.OnModelCreating");
        }
        static Entities() { 
            Database.SetInitializer(new EntitiesInitializer());
        }
        public Entities() : base("DbConnection") { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Base> Bases { get; set; }
        public DbSet<Structure> Structures { get; set; }
        public DbSet<Squad> Squads { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Message> Messages { get; set; }
    }

    /// <summary>
    /// Таблица аккаунта
    /// </summary>
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public short Role { get; set; }
        public string Token { get; set; }
    }

    /// <summary>
    /// Таблица игрока
    /// </summary>
    public class Player
    {
        [Key]
        public int UserID { get; set; }
        public string Playername { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
    }

    /// <summary>
    /// Таблица базы
    /// </summary>
    public class Base
    {
        [Key]
        public int BaseID { get; set; }
        public string Basename { get; set; }
        public int OwnerID { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public int Level { get; set; }
        public string CurrentTask { get; set; }
        public int FinishTime { get; set; }
        public bool IsActive { get; set; }
        public int LastAttacked { get; set; }
    }

    /// <summary>
    /// Таблица строений на базе
    /// </summary>
    public class Structure
    {  
        [Key]
        public int ID { get; set; }
        public int BaseID { get; set; }
        public int Level { get; set; }
        public string Type { get; set; }
        public string CurrentTask { get; set; }
        public int FinishTime { get; set; }
    }

    /// <summary>
    /// Таблица отрядов
    /// </summary>
    public class Squad
    {
        [Key]
        public string Key { get; set; }
        public int OwnerI2D { get; set; }
        public int MoveFrom { get; set; }
        public int StartTime { get; set; }
        public int MoveTo { get; set; }
        public int FinishTime { get; set; }
    }

    /// <summary>
    /// Таблица юнитов
    /// </summary>
    public class Unit
    {
        [Key]
        public int ID { get; set; }
        public string Instance { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// Таблица ресурсов
    /// </summary>
    public class Resource
    {
        [Key]
        public int ID { get; set; }
        public string Instance { get; set; }
        public int Credits { get; set; }
        public int Energy { get; set; }
        public double Neutrino { get; set; }
    }

    /// <summary>
    /// Таблица игровых сообщений
    /// </summary>
    public class Message
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Text { get; set; }
    }
}
