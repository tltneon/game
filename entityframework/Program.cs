using System;
using System.ComponentModel;

namespace entityframework
{
    class Program
    { 
        static void Main(string[] args)
        {
            Console.WriteLine("Введите команду:\n1 - вывод содержимого таблиц\n2 - заполнить таблицы");
            ConsoleKeyInfo key = Console.ReadKey();
            Console.Clear();

            using (Entities db = new Entities())
                if (key.KeyChar == '2')
                {
                    User user1 = new User { ID = 0, Name = "Admin", Password = "123456", Role = 1, Wins = 0, Loses = 0 }; 
                    User user2 = new User { ID = 1, Name = "User", Password = "123", Role = 0, Wins = 0, Loses = 0 };
                    db.Users.Add(user1);
                    db.Users.Add(user2);
                    Base base1 = new Base { Name = "AdminBase", Owner = 0, Credits = 100, Energy = 100, CoordX = 1, CoordY = 1, Level = 0 };
                    Base base2 = new Base { Name = "UserBase", Owner = 1, Credits = 100, Energy = 100, CoordX = 4, CoordY = 2, Level = 0 };
                    db.Bases.Add(base1);
                    db.Bases.Add(base2);
                    db.SaveChanges();
                    Console.WriteLine("Объекты успешно сохранены");
                }
                else
                {
                    var users = db.Users;
                    Console.WriteLine("Список объектов:");
                    foreach (User u in users)
                    {
                        Console.WriteLine("{0}.{1} - {2}", u.ID, u.Name, u.Password);
                    }
                    var bases = db.Bases;
                    Console.WriteLine("Список объектов:");
                    foreach (Base u in bases)
                    {
                        Console.WriteLine("{0}.{1} - {2}. Coords: {3}x{4}", u.ID, u.Name, u.Owner, u.CoordX, u.CoordY);
                    }
                }
            Console.Read();
        }
    }
}

/*foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(user2))
{
    string name = descriptor.Name;
    object value = descriptor.GetValue(user2);
    Console.WriteLine("{0}={1}", name, value);
}*/