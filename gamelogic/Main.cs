using gamelogic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace gamelogic
{
    /* */
    /* Класс менеджера БД управляет соединением и возвращает контекст */
    /* */
    public static class DbManager
    {
        private static Entities DB = null;
        /* Управляет подключением к БД */
        public static void ConnectToDB()
        {
            DB = new Entities();
        }
        /* Возвращает контекст */
        public static Entities GetContext()
        {
            if (DB == null) ConnectToDB();
            return DB;
        }
    }
    /* */
    /* Класс менеджера аккаунтов управляет авторизацией, регистрацией и прочим взаимодействием с аккаунтами */
    /* */
    public class AccountManager
    {
        /* Управляет авторизацией пользователя */
        public static string AuthClient(AuthData data)
        {
            var db = DbManager.GetContext();
            var us = db.Accounts.FirstOrDefault(o => o.Username == data.username);
            if (us != null)
            {
                if (us.Password == data.password) return us.Token;
                else return "Error#Wrong Password";
            }
            else return CreateUser(data.username, data.password);
        }
        /* Управляет созданием аккаунта */
        public static string CreateUser(string username, string password)
        {
            var db = DbManager.GetContext();
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
                return user.Token;
            }
            catch (Exception ex)
            {
                return "Error#Exception: " + ex.Message;
            }
        }
        /* Возвращает аккаунт по его токену */
        public static Account GetAccountByToken(string token)
        {
            var db = DbManager.GetContext();
            return db.Accounts.FirstOrDefault(o => o.Token == token);
        }
        /* Проверяет токен игрока */
        public static bool CheckToken(string token)
        {
            return GetAccountByToken(token) != null;
        }
    }
    /* */
    /* Класс менеджера игроков возвращает данные о игроках ("игрок" - персонаж, создаваемый для аккаунта) */
    /* */
    public class PlayerManager
    {
        /* Возвращает игрока по его ИД */
        public static Player GetPlayerByID(int userid)
        {
            var db = DbManager.GetContext();
            return db.Players.FirstOrDefault(o => o.UserID == userid);
        }
        /* Возвращает базу игрока по его ИД */
        public static Base GetBaseByUserID(int userid)
        {
            var db = DbManager.GetContext();
            return db.Bases.FirstOrDefault(o => o.OwnerID == userid);
        }
        /* Возвращает список игроков */
        public static IEnumerable<Player> GetPlayerList()
        {
            var db = DbManager.GetContext();
            return db.Players.AsEnumerable();
        }
    }
    /* */
    /* Класс менеджера баз принимает и возвращает все сведения о базах игроков */
    /* */
    public class BaseManager
    {
        /* Возвращает список баз */
        public static IEnumerable<Base> GetBaseList()
        {
            var db = DbManager.GetContext();
            return db.Bases.AsEnumerable();
        }
        /* Возвращает данные о базе по ИД */
        public static Base GetBaseByID(int baseid)
        {
            var db = DbManager.GetContext();
            return db.Bases.FirstOrDefault(o => o.BaseID == baseid);
        }
        /* Проверяет, является ли игрок владельцем базы */
        private static bool IsOwner(int baseid, string token)
        {
            return GetBaseByID(baseid).OwnerID == AccountManager.GetAccountByToken(token).UserID;
        }
        /* Проверяет вводимые данные */
        private static string CheckInput(BaseAction obj)
        {
            if (obj == null) return "wronginput";
            if (GetBaseByID(obj.baseid) == null) return "wrongbaseid";
            return "success";
        }
        /* Управляет улучшением базы */
        public static string UpgradeBase(BaseAction obj)
        {
            if (CheckInput(obj) != "success") return CheckInput(obj);
            try
            {
                Base curbase = GetBaseByID(obj.baseid);
                if (AccountManager.GetAccountByToken(obj.token).UserID == curbase.OwnerID) {
                    curbase.Level++;
                    DbManager.GetContext().SaveChanges();
                    return "success";
                }
                else
                    return "notanowner";
                }
            catch (Exception ex)
            {
                return "Error#Exception: " + ex.Message;
            }
        }
        /* Управляет созданием юнитов */
        public static string MakeUnit(BaseAction obj)
        {
            if (CheckInput(obj) != "success") return CheckInput(obj);
            try
            {
                Base curbase = GetBaseByID(obj.baseid);
                if (AccountManager.GetAccountByToken(obj.token).UserID == curbase.OwnerID && obj.result != null)
                {
                    if (HasBaseStructure(curbase, "aircraftsComplex") == null) return "noAircrafts";
                    if (!CanAfford(curbase, obj.result)) return "notenoughresources";

                    var db = DbManager.GetContext();

                    Unit u = db.Units.FirstOrDefault(o => o.Instance == "bas" + obj.baseid.ToString() && o.Type == obj.result);

                    if (u == null)
                        db.Units.Add(new Unit { Instance = "bas" + obj.baseid.ToString(), Type = obj.result, Count = 1 });
                    else
                        u.Count++;

                    db.SaveChanges();
                    return "success";
                }
                else
                {
                    return "notanowner";
                }
            }
            catch (Exception ex)
            {
                return "Error#Exception: " + ex.Message;
            }
        }
        /* Управляет созданием построек */
        public static string BuildStructure(BaseAction obj)
        {
            if (CheckInput(obj) != "success") return CheckInput(obj);
            try
            {
                Base curbase = GetBaseByID(obj.baseid);
                if (AccountManager.GetAccountByToken(obj.token).UserID == curbase.OwnerID && obj.result != null)
                {
                    obj.result = obj.result ?? "lifeComplex";

                    if (!CanAfford(curbase, obj.result)) return "notenoughresources";
                    if (HasBaseStructure(curbase, obj.result) != null) return "alreadyexists";

                    var db = DbManager.GetContext();

                    Resource resources = db.Resources.FirstOrDefault(o => o.Instance == "bas" + curbase.BaseID.ToString());
                    resources.Credits -= 100;
                    resources.Energy -= 100;

                    db.Structures.Add(new Structure { BaseID = obj.baseid, Type = obj.result, Level = 1 });
                    db.SaveChanges();
                    return "success";
                }
                else 
                {
                    return "notanowner";
                }
            }
            catch (Exception ex)
            {
                return "Error#Exception: " + ex.Message;
            }
        }
        /* Управляет улучшением построек */
        public static string UpgradeStructure(BaseAction obj)
        {
            if (CheckInput(obj) != "success") return CheckInput(obj);
            try
            {
                Base curbase = GetBaseByID(obj.baseid);
                if (AccountManager.GetAccountByToken(obj.token).UserID == curbase.OwnerID && obj.result != null)
                {
                    if (!CanAfford(curbase, obj.result)) return "notenoughresources";

                    Structure str = HasBaseStructure(curbase, obj.result);
                    str.Level++;
                    DbManager.GetContext().SaveChanges();
                    return "success";
                }
                else
                {
                    return "notanowner";
                }
            }
            catch (Exception ex)
            {
                return "Error#Exception: " + ex.Message;
            }
        }
        /* Управляет починкой базы */
        public static string RepairBase(BaseAction obj)
        {
            if (CheckInput(obj) != "success") return CheckInput(obj);
            try
            {
                Base curbase = GetBaseByID(obj.baseid);
                if (AccountManager.GetAccountByToken(obj.token).UserID == curbase.OwnerID)
                {
                    if ((curbase.Level*2000) > 0) return "notenoughresources";
                    curbase.IsActive = !curbase.IsActive;
                    DbManager.GetContext().SaveChanges();
                    return "success";
                }
                else
                {
                    return "notanowner";
                }
            }
            catch (Exception ex)
            {
                return "Error#Exception: " + ex.Message;
            }
        }
        /* Проверяет на возможность приобрести предмет */
        public static bool CanAfford(Base curbase, string itemName)
        {
            Resource resources = GetBaseResources(curbase.BaseID);
            if(itemName.Substring(itemName.Length - 4, 4) == "Unit")
            {

            }
            if (resources.Credits < 100 || resources.Energy < 100) return false;

            System.Diagnostics.Debug.WriteLine("успешно купил эту шляпу");
            return true;
        }
        /* Возвращает данные о базе */
        public static Base GetBaseInfo(Account acc)
        {
            var db = DbManager.GetContext();
            return db.Bases.FirstOrDefault(o => o.BaseID == acc.UserID);
        }
        /* Возвращает массив всех построек на базе */
        public static IEnumerable<Structure> GetBaseStructures(int BaseID)
        {
            var db = DbManager.GetContext();
            return db.Structures.Where(o => o.BaseID == BaseID).AsEnumerable();
        }
        /* Возвращает постройку, если она была построена на базе */
        public static Structure HasBaseStructure(Base curbase, string structure) 
        {
            var db = DbManager.GetContext();
            return db.Structures.FirstOrDefault(o => o.Type == structure && o.BaseID == curbase.BaseID);
        }
        /* Возвращает массив всех ресурсов на базе */
        public static Resource GetBaseResources(int BaseID)
        {
            var db = DbManager.GetContext();
            return db.Resources.FirstOrDefault(o => o.Instance == "bas" + BaseID.ToString());
        }
        /* Возвращает массив всех юнитов на базе */
        public static IEnumerable<Unit> GetBaseUnits(int BaseID)
        {
            var db = DbManager.GetContext();
            return db.Units.Where(o => o.Instance == "bas" + BaseID.ToString() && o.Count > 0).AsEnumerable();
        }
        /* Возвращает общее количество юнитов на базе */
        public static int GetBaseUnitsCount(int BaseID)
        {
            var db = DbManager.GetContext();
            return db.Units.Where(o => o.Instance == "bas" + BaseID.ToString() && o.Count > 0).Sum(p => p.Count);
        }
        /* Назначает ("добывает") всем базам ресурсы исходя из наличия необходимых строений */
        public static bool BaseGatherResources()
        {
            // дичайший костылище
            var DB = DbManager.GetContext();
            var Bases = GetBaseList().ToList();
            foreach (Base CurrentBase in Bases)
            {
                Resource Resources = DB.Resources.FirstOrDefault(o => o.Instance == "bas" + CurrentBase.BaseID.ToString());

                Structure creditsStruct = HasBaseStructure(CurrentBase, "resourceComplex");
                if (creditsStruct != null) Resources.Credits += 10 * creditsStruct.Level / 10;

                Structure energyStruct = HasBaseStructure(CurrentBase, "energyComplex");
                if (energyStruct != null) Resources.Energy += 10 * energyStruct.Level / 10;

                Structure neutrinoStruct = HasBaseStructure(CurrentBase, "researchStation");
                if (neutrinoStruct != null) Resources.Neutrino += 0.000001 * neutrinoStruct.Level / 10;

                // базовое значение * уровень здания / 10 частей в минуте
            }

            ProceedActions.Log("засейвил этот факт");

            DB.SaveChanges();

            return true;
        }
    }
    /* */
    /* Класс менеджера отрядов управляет отрядами */
    /* */
    public class SquadManager
    {
        /* Возвращает отряд по его ключу */
        public static Squad GetSquad(string key)
        {
            var db = DbManager.GetContext();
            return db.Squads.FirstOrDefault(o => o.Key == key);
        }
        /* Возвращает список всех отрядов */
        public static IEnumerable<Squad> GetSquads()
        {
            var db = DbManager.GetContext();
            return db.Squads.AsEnumerable();
        }
        /* Получает приказ на возвращение отряда */
        public static string SendReturnOrder(SquadAction obj) 
        {
            return "success"; 
        }
        /* Получает приказ на атаку отряда */
        public static string SendAttackOrder(SquadAction obj) 
        {
            int attackerID = AccountManager.GetAccountByToken(obj.token).UserID;
            int victimID = BaseManager.GetBaseByID(obj.to).OwnerID;
            if (attackerID == victimID) return "failed";

            return ProceedActions.DoBattle(attackerID, victimID);

            /* функция не реализована в полной мере */
            /*var db = DbManager.GetContext();
            Account player = AccountManager.GetAccountByToken(obj.token);
            db.Squads.Add(new Squad { Key = obj.key, OwnerID = player.UserID, MoveTo = obj.to, MoveFrom = 1 });
            db.SaveChanges();
            return "success";*/
        }
    }
    /* */
    /* Класс проведения действий управляет свершениями различных игровых действий */
    /* */
    class ProceedActions {
        /* Проводит атаку на базу */
        public static string DoBattle(int attackerID, int victimID) {
            string result;
            var db = DbManager.GetContext();
            Player attacker = PlayerManager.GetPlayerByID(attackerID);
            Player victim = PlayerManager.GetPlayerByID(victimID);
            IEnumerable<Unit> attackerUnits = BaseManager.GetBaseUnits(attackerID);
            IEnumerable<Unit> victimUnits = BaseManager.GetBaseUnits(victimID);

            System.Diagnostics.Debug.WriteLine("меряемся у кого больше. войска первого");
            int attackerPower = 0;
            int victimPower = 0;
            foreach (Unit unit in attackerUnits) {
                attackerPower += unit.Count * 10;
                System.Diagnostics.Debug.WriteLine(unit.Type + " - " + unit.Count);
            }
            System.Diagnostics.Debug.WriteLine("меряемся у кого больше. войска второго");
            foreach (Unit unit in victimUnits)
            {
                victimPower += unit.Count * 10;
                System.Diagnostics.Debug.WriteLine(unit.Type + " - " + unit.Count);
            }
            System.Diagnostics.Debug.WriteLine($"чекаем кол-во войск {attackerPower} vs {victimPower}");

            System.Diagnostics.Debug.WriteLine("очевидный вин, овации, балл в стату");
            if (attackerPower > victimPower)
            {
                attacker.Wins++;
                victim.Loses++;
                foreach (Unit unit in victimUnits)
                {
                    db.Entry(unit).State = EntityState.Deleted;
                }
                result = "youwin";
            }
            else
            {
                attacker.Loses++;
                victim.Wins++;
                foreach (Unit unit in attackerUnits)
                {
                    db.Entry(unit).State = EntityState.Deleted;
                }
                result = "youlose";
            }
            db.SaveChanges();
            return result;
        }
        /* */
        /* Реализует механизм логгирования */
        /* */
        public async static void Log(string text)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            using (FileStream fstream = new FileStream($"{dir}/../csharpgame.log", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                fstream.Seek(0, SeekOrigin.End);
                await fstream.WriteAsync(array, 0, array.Length);
            }
        }
    }
}

/* */
/* Предполагается, что здесь будут храниться константы игровых единиц (юниты, строения, прочее) */
/* */
namespace GameItems 
{
    class ItemsVars 
    {
        internal class StructureVars
        {
            public StructureVars(string name)
            {
                Type = name;
            }
            public string Type { get; set; }
            public int Credits { get; set; }
            public int Energy { get; set; }
            public double Neutrino { get; set; }
        }
        /* class UnitsVars : StructureVars
        {
            new public UnitsVars(string name)
            {
                Type = name;
            }
            new public string Type { get; set; }
            public int Power { get; set; }
        }
            internal readonly Dictionary<string, StructureVars> Structures = new Dictionary<string, StructureVars> {
                { "lifeComplex", new StructureVars {Credits = 190, Energy = 210, Neutrino = 0} },
                { "resourceComplex", new StructureVars {Credits = 100, Energy = 50, Neutrino = 0} },
                { "energyComplex", new StructureVars {Credits = 100, Energy = 150, Neutrino = 0} },
                { "aircraftsComplex", new StructureVars {Credits = 250, Energy = 320, Neutrino = 0} },
                { "researchStation", new StructureVars {Credits = 2000, Energy = 1000, Neutrino = 0} },
            };
            internal readonly Dictionary<string, UnitsVars> Units = new Dictionary<string, UnitsVars> {
                { "droneUnit", new UnitsVars {Credits = 100, Energy = 100, Neutrino = 0, Power = 100} },
                { "jetUnit", new UnitsVars {Credits = 100, Energy = 100, Neutrino = 0, Power = 1000} },
                { "lincorUnit", new UnitsVars {Credits = 100, Energy = 100, Neutrino = 0, Power = 1500} },
                { "someGiantShitUnit", new UnitsVars {Credits = 100, Energy = 100, Neutrino = 1, Power = 200000} },
            };
        public StructureVars GetItemInfo(string str) {
            return Structures.str;
        }*/
    }
}