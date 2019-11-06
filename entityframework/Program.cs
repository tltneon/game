using System;
using System.Data.Entity;
using System.Linq;
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
                Console.WriteLine("[Database deployment tool]\nEnter the command:\n1 - show data from tables\n2 - fill database with dummy data\n3 - drop databases\n4 - make new user\n5 - find user by name");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.Clear();
                
                switch (key.KeyChar)
                {
                    case '2':
                        db.Accounts.Add(new Account { Username = "Admin", Password = "123456", Role = 1 });
                        db.Accounts.Add(new Account { Username = "User", Password = "123", Role = 0 });
                        db.Players.Add(new Player { Playername = "Admin" });
                        db.Players.Add(new Player { Playername = "User" });
                        db.Bases.Add(new Base { Basename = "AdminBase", OwnerID = 1, CoordX = 1, CoordY = 1, Level = 0 });
                        db.Bases.Add(new Base { Basename = "UserBase", OwnerID = 2, CoordX = 4, CoordY = 2, Level = 0 });
                        db.SaveChanges();
                        Console.WriteLine("TABLES FILLED");
                        break;

                    case '3':
                        Console.WriteLine("Executing...");
                        db.Database.Delete();
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

                    case '5':
                        Console.WriteLine("Find user by name: ");
                        string username = Console.ReadLine();
                        var us = db.Accounts.Where(o => o.Username == username);
                        foreach (Account customer in us)
                        {
                            Console.WriteLine("Found: id{0}, {1}, Password {2}, Role {3} ", customer.UserID, customer.Username, customer.Password, customer.Role);
                        }
                        Console.WriteLine("DONE");
                        break;

                    default:
                        Console.WriteLine("Opening connection to DB...");

                        Console.WriteLine("ACCOUNTS TABLE:");
                        foreach (Account u in db.Accounts) Console.WriteLine("{0}. {1} - {2}", u.UserID, u.Username, u.Password);

                        Console.WriteLine("PLAYERS TABLE:");
                        foreach (Player u in db.Players) Console.WriteLine("{0}. {1}", u.UserID, u.Playername);

                        Console.WriteLine("BASES TABLE:");
                        foreach (Base u in db.Bases) Console.WriteLine("{0}. {1} ({2} lvl) - {3}. Coords: {4}x{5}", u.BaseID, u.Basename, u.Level, u.OwnerID, u.CoordX, u.CoordY);

                        Console.WriteLine("BUILDINGS TABLE:");
                        foreach (Building u in db.Buildings) Console.WriteLine("{0}. {1} - {2}", u.BaseID, u.Type, u.Level);

                        Console.WriteLine("SQUADS TABLE:");
                        foreach (Squad u in db.Squads) Console.WriteLine("{0}. {1} - {2}", u.Key, u.MoveFrom, u.MoveTo);

                        Console.WriteLine("DONE");
                        break;
                }

                Console.WriteLine("Another command? '1' to yes.");
                if (Console.ReadKey().KeyChar == '1') goto OneMore;
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

//var saloane = _context.Saloane.Where(c => c.SomeProperty == "something").ToList();