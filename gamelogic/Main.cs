using gamelogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gamelogic
{
    public class DbManager
    {
        private static Entities DB = null;
        public static void ConnectToDB()
        {
            DB = new Entities();
        }
        public static Entities GetContext()
        {
            if (DB == null) ConnectToDB();
            return DB;
        }
    }
    public class AccountManager
    {
        public static string AuthClient(AuthData data)
        {
            var db = DbManager.GetContext();
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
            var db = DbManager.GetContext();
            string token;
            try
            {
                Account user = new Account { Username = username, Password = password, Role = 0, Token = "Token=49rh23489rh+salt" };
                db.Accounts.Add(user);
                db.SaveChanges();
                int newIdentityValue = user.UserID;
                db.Players.Add(new Player { UserID = newIdentityValue, Playername = username });
                db.Bases.Add(new Base { Basename = username + "Base", OwnerID = newIdentityValue, CoordX = 1, CoordY = 1, Level = 0 });
                db.SaveChanges();
                token = user.Token;
            }
            catch (Exception ex)
            {
                token = "Error#Exception: " + ex.Message;
            }
            return token;
        }
    }
        public class TestLogic
    {
        public static int UpgradeBase(int baseid) {
            var db = DbManager.GetContext();
            int result = 0;
            try
            {
                var bas = db.Bases.Find(baseid);
                bas.Level++;
                db.SaveChanges();
                result = bas.Level;
            }
            catch
            {
            }
            return result;
        }
        public static IEnumerable<Player> GetUserList() {
            var db = DbManager.GetContext();
            return db.Players.AsEnumerable();
        }
    }
}
