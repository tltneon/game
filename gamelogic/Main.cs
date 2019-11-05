/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;*/
using System;

namespace gamelogic
{
    public class TestLogic
    {
        private static Entities DB = null;
        public static void ConnectToDB() {
            DB = new Entities();
        }
        public static Entities GetContext()
        {
            if (DB == null) ConnectToDB();
            return DB;
        }
        public static string UpgradeBase(string username, string password) {
            //getbasebyid()
            //setdata()
            CreateUser(username, password);
            return "gamelogicsayswhaturgay";
        }
        public static string debug() {
            string output = "USERS TABLE:\n";
            using (Entities db = new Entities())
                foreach (User u in db.Users) output += u.Name + "\n";
            return output;
        }
        public static User FindUser(string username) {
            DB = GetContext();
            return DB.Users.Find(username);
        }
        public static bool CreateUser(string username, string password)
        {
            DB = GetContext();
            bool result = false;
            try {
                DB.Users.Add(new User { ID = 7, Name = username, Password = password, Role = 0, Wins = 0, Loses = 0 });
                DB.SaveChanges();
                result = true;
            }
            catch {
            }
            return result;
        }
        public string Print(string str) {
            System.Diagnostics.Debug.WriteLine("debug functions");
            return str;
        }
    }
}
