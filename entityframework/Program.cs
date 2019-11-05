using System;
using System.Data.Entity;
using gamelogic;

namespace entityframework
{
    class Program
    { 
        static void Main(string[] args)
        {
            TestLogic.ConnectToDB();
            using (Entities db = TestLogic.GetContext())
            {
            OneMore:
                Console.Clear();
                Console.WriteLine("[Database deployment tool]\nEnter the command:\n1 - show data from tables\n2 - fill database with dummy data\n3 - truncate databases\n4 - make new user");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.Clear();
                
                switch (key.KeyChar)
                {
                    case '2':
                        db.Users.Add(new User { ID = 0, Name = "Admin", Password = "123456", Role = 1, Wins = 0, Loses = 0 });
                        db.Users.Add(new User { ID = 1, Name = "User", Password = "123", Role = 0, Wins = 0, Loses = 0 });
                        db.Bases.Add(new Base { Name = "AdminBase", Owner = 0, Credits = 100, Energy = 100, CoordX = 1, CoordY = 1, Level = 0 });
                        db.Bases.Add(new Base { Name = "UserBase", Owner = 1, Credits = 100, Energy = 100, CoordX = 4, CoordY = 2, Level = 0 });
                        db.SaveChanges();
                        Console.WriteLine("TABLES FILLED");
                        break;

                    case '3':
                        Console.WriteLine("Executing...");
                        /*var comps = db.Database.SqlQuery<User>("SELECT * FROM Users");
                        foreach (var company in comps)
                            Console.WriteLine(company.Name);*/
                        db.Database.ExecuteSqlCommand("TRUNCATE TABLE Users");
                        db.Database.ExecuteSqlCommand("TRUNCATE TABLE Bases");
                        db.Database.ExecuteSqlCommand("TRUNCATE TABLE Buildings");
                        db.Database.ExecuteSqlCommand("TRUNCATE TABLE Squads");
                        RecreateDatabase("SELECT 1+1");
                        //db.Database.Delete();
                        Console.WriteLine("DONE");
                        break;

                    case '4':
                        Console.WriteLine("username:");
                        string usr = Console.ReadLine();
                        Console.WriteLine("password:");
                        string pas = Console.ReadLine();
                        Console.WriteLine("doin' magic...");
                        Console.WriteLine(TestLogic.CreateUser(usr, pas));
                        break;

                    default:
                        Console.WriteLine("Opening connection to DB...");

                        Console.WriteLine("USERS TABLE:");
                        foreach (User u in db.Users) Console.WriteLine("{0}. {1} - {2}", u.ID, u.Name, u.Password);

                        Console.WriteLine("BASES TABLE:");
                        foreach (Base u in db.Bases) Console.WriteLine("{0}. {1} - {2}. Coords: {3}x{4}", u.ID, u.Name, u.Owner, u.CoordX, u.CoordY);

                        Console.WriteLine("BUILDINGS TABLE:");
                        foreach (Building u in db.Buildings) Console.WriteLine("{0}. {1} - {2}", u.ID, u.Type, u.Level);

                        Console.WriteLine("SQUADS TABLE:");
                        foreach (Squad u in db.Squads) Console.WriteLine("{0}. {1} - {2}", u.ID, u.MoveFrom, u.MoveTo);

                        Console.WriteLine("DONE");
                        break;
                }

                Console.WriteLine("Another command? '1' to yes.");
                if (Console.ReadKey().KeyChar == '1') goto OneMore;
            }
        }
        public static void RecreateDatabase(string schemaScript)
        {
            Entities db = new Entities();
            new Entities().Database.Delete(); // Opens and disposes its own connection

            using (db = new Entities()) // New connection
            {
                Database database = db.Database;
                database.Create(); // Works!
                database.ExecuteSqlCommand(schemaScript);
            }
        }
    }
}

/*foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(user2))
{
    string name = descriptor.Name;
    object value = descriptor.GetValue(user2);
    Console.WriteLine("{0}={1}", name, value);
}*/