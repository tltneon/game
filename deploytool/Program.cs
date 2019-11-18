using System;
using gamelogic;

namespace deploytool
{
    internal class Program
    { 
        private static void Main()
        {
            using (Entities db = new Entities())
            {
                bool flag = true;
                while (flag)
                {
                    Console.Clear();
                    Console.WriteLine("[Database deployment tool]\nEnter the command:\n1 - show data from tables\n" +
                        "2 - fill database with dummy data\n3 - drop databases\n4 - make new user\n5 - find user by name");
                    ConsoleKeyInfo key = Console.ReadKey();
                    Console.Clear();

                    switch (key.KeyChar)
                    {
                        case '2':
                            try
                            {
                                db.Accounts.Add(new Account { UserID = 1, Username = "Admin", Password = AccountManager.Base64Encode("123456"), Role = 1, Token = "fgio" });
                                db.Accounts.Add(new Account { UserID = 2, Username = "User", Password = AccountManager.Base64Encode("123"), Role = 0, Token = "fl4o" });
                                db.Players.Add(new Player { UserID = 1, Playername = "Admin" });
                                db.Players.Add(new Player { UserID = 2, Playername = "User" });
                                db.Bases.Add(new Base { Basename = "AdminBase", OwnerID = 1, CoordX = 1, CoordY = 1, Level = 1, IsActive = true });
                                db.Bases.Add(new Base { Basename = "UserBase", OwnerID = 2, CoordX = 4, CoordY = 2, Level = 1, IsActive = true });
                                db.Resources.Add(new Resource { Instance = "bas1", Credits = 200, Energy = 200, Neutrino = 0.0 });
                                db.Resources.Add(new Resource { Instance = "bas2", Credits = 200, Energy = 200, Neutrino = 0.0 });
                                db.Units.Add(new Unit { Instance = "bas1", Type = "droneUnit", Count = 1 });
                                db.Units.Add(new Unit { Instance = "bas2", Type = "droneUnit", Count = 2 });
                                db.SaveChanges();
                            }
                            catch
                            {
                                Console.WriteLine("\n[Warn] Who knows that just happen here.\n");
                            }
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
                            Console.WriteLine(AccountManager.CreateUser(usr, pas));
                            break;

                        case '5':
                            Console.WriteLine("Find user by name: ");
                            string username = Console.ReadLine();
                            Player player = PlayerManager.FindByName(username);
                            Console.WriteLine($"Found: id{player.UserID}, {player.Playername}, Password {player.Wins}, Role {player.Loses} ");
                            Console.WriteLine("DONE");
                            break;

                        default:
                            Console.WriteLine("Opening connection to DB...");

                            try
                            {
                                Console.WriteLine("ACCOUNTS TABLE:");
                                foreach (Account u in db.Accounts)
                                {
                                    Console.WriteLine($"ID{u.UserID}. {u.Username} - {u.Password}, Role {u.Role}, Token={u.Token}");
                                }

                                Console.WriteLine("PLAYERS TABLE:");
                                foreach (Player u in db.Players)
                                {
                                    Console.WriteLine($"ID{u.UserID}. {u.Playername}: W{u.Wins}|L{u.Loses}");
                                }

                                Console.WriteLine("BASES TABLE:");
                                foreach (Base u in db.Bases)
                                {
                                    Console.WriteLine($"ID{u.BaseID}. {u.Basename} (lvl{u.Level}) - OwnerID{u.OwnerID}");
                                }

                                Console.WriteLine("STRUCTURES TABLE:");
                                foreach (Structure u in db.Structures)
                                {
                                    Console.WriteLine($"BaseID{u.BaseID}. {u.Type} (lvl{u.Level})");
                                }

                                Console.WriteLine("SQUADS TABLE:");
                                foreach (Squad u in db.Squads)
                                {
                                    Console.WriteLine($"{u.Key}. {u.MoveFrom} => {u.MoveTo}");
                                }

                                Console.WriteLine("UNITS TABLE:");
                                foreach (Unit u in db.Units)
                                {
                                    Console.WriteLine($"{u.Instance}. Type: {u.Type} x{u.Count}");
                                }

                                Console.WriteLine("RESOURCES TABLE:");
                                foreach (Resource u in db.Resources)
                                {
                                    Console.WriteLine($"{u.Instance} contains: {u.Credits} Credits, {u.Energy} Energy, {u.Neutrino} Neutrino");
                                }
                            }
                            catch
                            {
                                Console.WriteLine("\n[Warn] Database models has changed. You need to drop datatables first.\n");
                            }

                            Console.WriteLine("DONE");
                            break;
                    }

                    Console.WriteLine("Another command? '1' to try something else.");
                    if (Console.ReadKey().KeyChar != '1')
                    {
                        flag = false;
                    }
                }
            }
        }
    }
}