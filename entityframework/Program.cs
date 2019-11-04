using System;
using System.ComponentModel;
using System.Data.Entity;

namespace entityframework
{
    class Program
    { 
        static void Main(string[] args)
        {
            Console.WriteLine("[Database deployment tool]\nEnter the command:\n1 - show data from tables\n2 - fill database with dummy data\n3 - truncate databases");
            ConsoleKeyInfo key = Console.ReadKey();
            Console.Clear();

            using (Entities db = new Entities())
                switch (key.KeyChar) {
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
                        RecreateDatabase("SELECT 1+1");
                        //db.Database.Delete();
                        Console.WriteLine("DONE");
                        break;
                    default:
                        Console.WriteLine("USERS TABLE:");
                        foreach (User u in db.Users) Console.WriteLine("{0}.{1} - {2}", u.ID, u.Name, u.Password);

                        Console.WriteLine("BASES TABLE:");
                        foreach (Base u in db.Bases) Console.WriteLine("{0}.{1} - {2}. Coords: {3}x{4}", u.ID, u.Name, u.Owner, u.CoordX, u.CoordY);

                        Console.WriteLine("BUILDINGS TABLE:");
                        foreach (Building u in db.Buildings) Console.WriteLine("{0}.{1} - {2}", u.ID, u.Type, u.Level);

                        Console.WriteLine("SQUADS TABLE:");
                        foreach (Squad u in db.Squads) Console.WriteLine("{0}.{1} - {2}", u.ID, u.MoveFrom, u.MoveTo);

                        Console.WriteLine("DONE");
                        break;
                }
            Console.Read();
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