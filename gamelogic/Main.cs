using gamelogic.Models;
using System;
using System.Linq;

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
        public static int UpgradeBase(int baseid) {
            DB = GetContext();
            int result = 0;
            try
            {
                var bas = DB.Bases.Find(baseid);
                bas.Level++;
                DB.SaveChanges();
                result = bas.Level;
            }
            catch
            {
            }
            return result;
        }
        public static Account FindUser(string username) {
            DB = GetContext();
            return DB.Accounts.Find(username);
        }
        public static string AuthClient(AuthData data) {
            var db = GetContext();
            string token;
            var us = db.Accounts.Where(o => o.Username == data.username);
            if (us.Count() > 0)
            {
                if (us.First().Password == data.password) token = us.First().Token;
                else token = "Error#Wrong Password";
            }
            else token = CreateUser(data.username, data.password);
            return token;
        }
        public static string CreateUser(string username, string password)
        {
            DB = GetContext();
            string token;
            try {
                Account user = new Account { Username = username, Password = password, Role = 0, Token = "Token=49rh23489rh+salt" };
                DB.Accounts.Add(user);
                DB.SaveChanges();
                int newIdentityValue = user.UserID;
                DB.Players.Add(new Player { UserID = newIdentityValue, Playername = username });
                DB.Bases.Add(new Base { Basename = username+"Base", OwnerID = newIdentityValue, CoordX = 1, CoordY = 1, Level = 0 });
                DB.SaveChanges();
                token = user.Token;
            }
            catch (Exception ex) {
                token = "Error#Exception: "+ex.Message;
            }
            return token;
        }
    }
}
