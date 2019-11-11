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
            var us = db.Accounts.Where(o => o.Username == data.username).FirstOrDefault();
            if (us != null)
            {
                if (us.Password == data.password) token = us.Token;
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
                Account user = new Account { Username = username, Password = password, Role = 0, Token = "Token=потом придумаю как шифровать токен для " + username };
                db.Accounts.Add(user);
                db.SaveChanges();
                int newIdentityValue = user.UserID;
                db.Players.Add(new Player { UserID = newIdentityValue, Playername = username });
                db.Bases.Add(new Base { Basename = username + "Base", OwnerID = newIdentityValue, CoordX = 1, CoordY = 1, Level = 0, IsActive = true });
                db.SaveChanges();
                token = user.Token;
            }
            catch (Exception ex)
            {
                token = "Error#Exception: " + ex.Message;
            }
            return token;
        }
        public static Account GetAccountByToken(string token)
        {
            var db = DbManager.GetContext();
            return db.Accounts.Where(o => o.Token == token).FirstOrDefault();
        }
        public static bool CheckToken(string token)
        {
            return GetAccountByToken(token) != null;
        }
    }
    public class PlayerManager
    {
        private static Base GetBaseByUserID(int userid)
        {
            var db = DbManager.GetContext();
            return db.Bases.Where(o => o.OwnerID == userid).FirstOrDefault();
        }
    }
    public class BaseManager
    {
        private static Base GetBase(int baseid)
        {
            var db = DbManager.GetContext();
            return db.Bases.Where(o => o.BaseID == baseid).FirstOrDefault();
        }
        private static bool IsOwner(int baseid, string token)
        {
            return GetBase(baseid).OwnerID == AccountManager.GetAccountByToken(token).UserID;
        }
        private static string CheckInput(BaseAction obj)
        {
            if (obj == null) return "wronginput";
            return "success";
        }
        // не прям топ минимизация кода, но пока норма;
        public static string UpgradeBase(BaseAction obj)
        {
            if (CheckInput(obj) != "success") return CheckInput(obj);
            string result;
            try
            {
                Base curbase = GetBase(obj.baseid);
                if (AccountManager.GetAccountByToken(obj.token).UserID == curbase.OwnerID) {
                    curbase.Level++;
                    DbManager.GetContext().SaveChanges();
                    result = "success";
                }
            else
                result = "notanowner";
            }
            catch (Exception ex)
            {
                result = "Error#Exception: " + ex.Message;
            }
            return result;
        }
        public static string MakeUnit(BaseAction obj)
        {
            throw new NotImplementedException();
        }

        public static string BuildStructure(BaseAction obj)
        {
            if (GetBase(obj.baseid) == null) return "wrongbaseid";
            if (IsOwner(obj.baseid, obj.token)) return "wrongbaseid";
            string result;
            try
            {
                var db = DbManager.GetContext();
                db.Structures.Add(new Structure { BaseID = obj.baseid, Type = obj.result, Level = 1 });
                db.SaveChanges();
                result = "success";
            }
            catch (Exception ex)
            {
                result = "Error#Exception: " + ex.Message;
            }

            return result;
        }

        public static string RepairBase(BaseAction obj)
        {
            Base curbase = GetBase(obj.baseid);
            if (curbase == null) return "wrongbaseid";
            if (IsOwner(obj.baseid, obj.token)) return "wrongbaseid";

            curbase.IsActive = !curbase.IsActive;
            DbManager.GetContext().SaveChanges();

            return "success";
        }

        public static Base GetBaseInfo(Account acc)
        {
            var db = DbManager.GetContext();
            return db.Bases.Where(o => o.BaseID == acc.UserID).FirstOrDefault();
        }
        public static IEnumerable<Structure> GetBaseStructures(BaseAction obj)
        {
            var db = DbManager.GetContext();
            return db.Structures.Where(o => o.BaseID == obj.baseid).AsEnumerable();
        }
    }
    public class TestLogic
    {
        public static IEnumerable<Player> GetUserList() {
            var db = DbManager.GetContext();
            return db.Players.AsEnumerable();
        }
    }
}