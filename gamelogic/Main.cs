using gamelogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gamelogic
{
    public static class DbManager
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
                // пока без шифрации пароля
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
        public static Player GetPlayerByID(int userid)
        {
            var db = DbManager.GetContext();
            return db.Players.Where(o => o.UserID == userid).FirstOrDefault();
        }
        public static Base GetBaseByUserID(int userid)
        {
            var db = DbManager.GetContext();
            return db.Bases.Where(o => o.OwnerID == userid).FirstOrDefault();
        }
        public static IEnumerable<Player> GetPlayerList()
        {
            var db = DbManager.GetContext();
            return db.Players.AsEnumerable();
        }
    }
    public class BaseManager
    {
        public static IEnumerable<Base> GetBaseList()
        {
            var db = DbManager.GetContext();
            return db.Bases.AsEnumerable();
        }
        public static Base GetBaseByID(int baseid)
        {
            var db = DbManager.GetContext();
            return db.Bases.Where(o => o.BaseID == baseid).FirstOrDefault();
        }
        private static bool IsOwner(int baseid, string token)
        {
            return GetBaseByID(baseid).OwnerID == AccountManager.GetAccountByToken(token).UserID;
        }
        private static string CheckInput(BaseAction obj)
        {
            if (obj == null) return "wronginput";
            if (GetBaseByID(obj.baseid) == null) return "wrongbaseid";
            return "success";
        }
        // не прям топ минимизация кода, но пока норма;
        public static string UpgradeBase(BaseAction obj)
        {
            if (CheckInput(obj) != "success") return CheckInput(obj);
            string result;
            try
            {
                Base curbase = GetBaseByID(obj.baseid);
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
            if (CheckInput(obj) != "success") return CheckInput(obj);
            string result;
            try
            {
                Base curbase = GetBaseByID(obj.baseid);
                if (AccountManager.GetAccountByToken(obj.token).UserID == curbase.OwnerID && obj.result != null)
                {
                    var db = DbManager.GetContext();
                    Squad sq = db.Squads.Where(o => o.Key == obj.baseid.ToString()).FirstOrDefault();
                    if (sq == null)
                        db.Squads.Add(new Squad { Key = obj.baseid.ToString(), OwnerID = AccountManager.GetAccountByToken(obj.token).UserID });

                    db.SaveChanges();
                    result = "success";
                }
                else
                {
                    result = "notanowner";
                }
            }
            catch (Exception ex)
            {
                result = "Error#Exception: " + ex.Message;
            }

            return result;
        }
        public static string BuildStructure(BaseAction obj)
        {
            if (CheckInput(obj) != "success") return CheckInput(obj);
            string result;
            try
            {
                Base curbase = GetBaseByID(obj.baseid);
                if (AccountManager.GetAccountByToken(obj.token).UserID == curbase.OwnerID && obj.result != null)
                {
                    obj.result = obj.result ?? "lifeComplex";
                    var db = DbManager.GetContext();
                    db.Structures.Add(new Structure { BaseID = obj.baseid, Type = obj.result, Level = 1 });
                    db.SaveChanges();
                    result = "success";
                }
                else 
                {
                    result = "notanowner";
                }
            }
            catch (Exception ex)
            {
                result = "Error#Exception: " + ex.Message;
            }

            return result;
        }
        public static string RepairBase(BaseAction obj)
        {
            if (CheckInput(obj) != "success") return CheckInput(obj);
            string result;
            try
            {
                Base curbase = GetBaseByID(obj.baseid);
                if (AccountManager.GetAccountByToken(obj.token).UserID == curbase.OwnerID)
                {
                    curbase.IsActive = !curbase.IsActive;
                    DbManager.GetContext().SaveChanges();
                    result = "success";
                }
                else
                {
                    result = "notanowner";
                }
            }
            catch (Exception ex)
            {
                result = "Error#Exception: " + ex.Message;
            }

            return result;
        }

        public static Base GetBaseInfo(Account acc)
        {
            var db = DbManager.GetContext();
            return db.Bases.Where(o => o.BaseID == acc.UserID).FirstOrDefault();
        }
        public static IEnumerable<Structure> GetBaseStructures(int BaseID)
        {
            var db = DbManager.GetContext();
            return db.Structures.Where(o => o.BaseID == BaseID).AsEnumerable();
        }

        public static Structure HasBaseStructure(Base curbase, string structure) 
        {
            var db = DbManager.GetContext();
            return db.Structures.Where(o => o.Type == structure && o.BaseID == curbase.BaseID).FirstOrDefault();
        }
        /*public static bool GetBaseResources(Base curbase)
        {
            var db = DbManager.GetContext();
            return db.Structures.Where(o => o.Type == structure && o.BaseID == curbase.BaseID).FirstOrDefault() != null;
        }
        public static bool SetBaseResources(Base curbase)
        {
            var db = DbManager.GetContext();
            return db.Structures.Where(o => o.Type == structure && o.BaseID == curbase.BaseID).FirstOrDefault() != null;
        }*/
    }
    public class SquadManager
    {
        public static Squad GetSquad(string key)
        {
            var db = DbManager.GetContext();
            return db.Squads.Where(o => o.Key == key).FirstOrDefault();
        }
        public static IEnumerable<Squad> GetSquads()
        {
            var db = DbManager.GetContext();
            return db.Squads.AsEnumerable();
        }
        public static string SendReturnOrder(SquadAction obj) { return "success"; }
        public static string SendAttackOrder(SquadAction obj) {
            ProceedActions.DoBattle(AccountManager.GetAccountByToken(obj.token).UserID, BaseManager.GetBaseByID(obj.to).OwnerID);
            return "success";
            /*var db = DbManager.GetContext();
            Account player = AccountManager.GetAccountByToken(obj.token);
            db.Squads.Add(new Squad { Key = obj.key, OwnerID = player.UserID, MoveTo = obj.to, MoveFrom = 1 });
            db.SaveChanges();
            return "success";*/
        }
    }
    class ProceedActions {
        public static void DoBattle(int attackerID, int victimID) {
            var db = DbManager.GetContext();
            Player attacker = PlayerManager.GetPlayerByID(attackerID);
            Player victim = PlayerManager.GetPlayerByID(victimID);
            attacker.Wins++;
            victim.Loses++;
            db.SaveChanges();
        }
    }
}