using gamelogic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace gamelogic
{
    /* */
    /* Класс менеджера аккаунтов управляет авторизацией, регистрацией и прочим взаимодействием с аккаунтами */
    /* */
    public class AccountManager
    {
        /* Управляет авторизацией пользователя */
        public static string AuthClient(AuthData data)
        {
            using (Entities db = new Entities())
            {
                var us = db.Accounts.FirstOrDefault(o => o.Username == data.username);
                if (us != null)
                {
                    if (us.Password == data.password)
                    {
                        return us.Token;
                    }
                    else
                    {
                        ProceedActions.Log("Event", $"Неудачная попытка авторизоваться под аккаунтом {us.Username}");
                        const string error = "Error#Wrong Password";
                        return error;
                    }
                }
                else
                {
                    return CreateUser(data.username, data.password);
                }
            }
        }
        /* Управляет созданием аккаунта */
        public static string CreateUser(string username, string password)
        {
            using (Entities db = new Entities())
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
                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция CreateUser");
                return "Error#Exception: " + ex.Message;
            }
        }
        /* Возвращает аккаунт по его токену */
        public static Account GetAccountByToken(string token)
        {
            using (Entities db = new Entities())
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
            using (Entities db = new Entities())
                return db.Players.FirstOrDefault(o => o.UserID == userid);
        }
        /* Возвращает базу игрока по его ИД */
        public static Base GetBaseByUserID(int userid)
        {
            using (Entities db = new Entities())
                return db.Bases.FirstOrDefault(o => o.OwnerID == userid);
        }
        /* Возвращает список игроков */
        public static IEnumerable<Player> GetPlayerList()
        {
            using (Entities db = new Entities())
                return db.Players.ToList();
        }
        /* Возвращает статистику игроков */
        public static IEnumerable<StatsData> GetPlayerStats()
        {
            return null;
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
            using (Entities db = new Entities())
                return db.Bases.ToList();
        }
        /* Возвращает данные о базе по ИД */
        public static Base GetBaseByID(int baseid)
        {
            using (Entities db = new Entities())
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
            if (obj == null)
            {
                const string result = "wronginput";
                return result;
            }
            if (GetBaseByID(obj.baseid) == null)
            {
                const string result = "wrongbaseid";
                return result;
            }
            else
            {
                const string result = "success";
                return result;
            }
        }
        /* Управляет улучшением базы */
        public static string UpgradeBase(BaseAction obj)
        {
            if (CheckInput(obj) != "success") return CheckInput(obj);
            try
            {
                Base curbase = GetBaseByID(obj.baseid);
                Account Account = AccountManager.GetAccountByToken(obj.token);
                if (Account.UserID == curbase.OwnerID)
                {
                    int level;
                    using (Entities db = new Entities())
                    {
                        Base BaseEntry = db.Bases.FirstOrDefault(o => o.BaseID == curbase.BaseID);
                        BaseEntry.Level++;
                        level = BaseEntry.Level;
                        db.SaveChanges();
                    }
                    ProceedActions.DoBuyItem(curbase, obj.result, level);
                    ProceedActions.Log("Event", $"{Account.Username} perform base upgrading at 1 level up.");
                    const string result = "success";
                    return result;
                }
                else
                {
                    const string result = "notanowner";
                    return result;
                }
            }
            catch (Exception ex)
            {
                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция UpgradeBase");
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

                    ProceedActions.DoBuyItem(curbase, obj.result, 1);

                    Unit u = SquadManager.GetUnit("bas" + obj.baseid.ToString(), obj.result);
                        
                    using (Entities db = new Entities())
                    {
                        if (u == null)
                            db.Units.Add(new Unit { Instance = "bas" + obj.baseid.ToString(), Type = obj.result, Count = 1 });
                        else
                            u.Count++;
                        db.SaveChanges();
                    }

                    const string result = "success";
                    return result;
                }
                else
                {
                    const string result = "notanowner";
                    return result;
                }
            }
            catch (Exception ex)
            {
                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция MakeUnit");
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

                    ProceedActions.DoBuyItem(curbase, obj.result, 1);

                    using (Entities db = new Entities())
                    {
                        db.Structures.Add(new Structure { BaseID = obj.baseid, Type = obj.result, Level = 1 });
                        db.SaveChanges();
                    }
                    const string result = "success";
                    return result;
                }
                else
                {
                    const string result = "notanowner";
                    return result;
                }
            }
            catch (Exception ex)
            {
                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция BuildStructure");
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
                    if (HasBaseStructure(curbase, obj.result) == null) return "notexists";

                    int level;

                    using (Entities db = new Entities()) 
                    {
                        var str = db.Structures.FirstOrDefault(o => o.Type == obj.result && o.BaseID == curbase.BaseID);
                        str.Level++;
                        level = str.Level;
                        db.SaveChanges();
                    }

                    ProceedActions.DoBuyItem(curbase, obj.result, level);

                    const string result = "success";
                    return result;
                }
                else
                {
                    const string result = "notanowner";
                    return result;
                }
            }
            catch (Exception ex)
            {
                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция UpgradeStructure");
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
                    if (!CanAfford(curbase, "base")) return "notenoughresources";

                    int level;
                    using (Entities db = new Entities())
                    {
                        var BaseEntry = db.Bases.FirstOrDefault(o => o.BaseID == obj.baseid);
                        BaseEntry.IsActive = !BaseEntry.IsActive;
                        level = BaseEntry.Level;
                        db.SaveChanges(); 
                    }
                    ProceedActions.DoBuyItem(curbase, "base", level);
                    const string result = "success";
                    return result;
                }
                else
                {
                    const string result = "notanowner";
                    return result;
                }
            }
            catch (Exception ex)
            {
                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция RepairBase");
                return "Error#Exception: " + ex.Message;
            }
        }
        /* Проверяет на возможность приобрести предмет */
        public static bool CanAfford(Base curbase, string itemName, int level = 1)
        {
            Resource resources = GetBaseResources(curbase.BaseID);
            var cost = GameItems.ItemsVars.GetCost(itemName);
            if (resources.Credits < cost.Credits*level || resources.Energy < cost.Energy*level) return false;
            return true;
        }
        /* Возвращает данные о базе */
        public static Base GetBaseInfo(Account acc)
        {
            Base result;
            using (Entities db = new Entities())
                result = db.Bases.FirstOrDefault(o => o.BaseID == acc.UserID);
            return result;
        }
        /* Возвращает массив всех построек на базе */
        public static IEnumerable<Structure> GetBaseStructures(int BaseID)
        {
            using (Entities db = new Entities())
                return db.Structures.Where(o => o.BaseID == BaseID).ToList();
        }
        /* Возвращает постройку, если она была построена на базе */
        public static Structure HasBaseStructure(Base curbase, string structure) 
        {
            Structure result;
            using (Entities db = new Entities())
                result = db.Structures.FirstOrDefault(o => o.Type == structure && o.BaseID == curbase.BaseID);
            return result;
        }
        /* Возвращает массив всех ресурсов на базе */
        public static Resource GetBaseResources(int BaseID)
        {
            Resource result;
            using (Entities db = new Entities())
                result = db.Resources.FirstOrDefault(o => o.Instance == "bas" + BaseID.ToString());
            return result;
        }
        /* Возвращает массив всех юнитов на базе */
        public static IEnumerable<Unit> GetBaseUnits(int BaseID)
        {
            using (Entities db = new Entities())
                return db.Units.Where(o => o.Instance == "bas" + BaseID.ToString() && o.Count > 0).ToList();
        }
        /* Возвращает общее количество юнитов на базе */
        public static int GetBaseUnitsCount(int BaseID)
        {
            return 1;
            /*using (Entities db = new Entities())
                return db.Units
                .Where(o => o.Instance == "bas" + BaseID.ToString() && o.Count > 0)
                .GroupBy(o => o.Instance)
                .Select(p => new { Total = p.Sum(i => i.Count) }).FirstOrDefault().Total;*/
        }
        /* Назначает ("добывает") всем базам ресурсы исходя из наличия необходимых строений */
        public static bool BaseGatherResources()
        {
            // дичайший костылище
            using (Entities DB = new Entities())
            {
                var Bases = DB.Bases.ToList();
                foreach (Base CurrentBase in Bases)
                {
                    if (CurrentBase.IsActive)
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

                }

                ProceedActions.Log("Event", "Routine() iteration");

                DB.SaveChanges();
            }
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
            Squad result;
            using (Entities db = new Entities())
                result = db.Squads.FirstOrDefault(o => o.Key == key);
            return result;
        }
        /* Возвращает список всех отрядов */
        public static IEnumerable<Squad> GetSquads()
        {
            using (Entities db = new Entities())
            return db.Squads.ToList();
        }
        /* Получает приказ на возвращение отряда */
        public static string SendReturnOrder(SquadAction obj)
        {
            const string result = "success";
            return result;
        }
        /* */
        public static Unit GetUnit(string Instance, string Type)
        {
            Unit result;
            using (Entities db = new Entities())
                result = db.Units.FirstOrDefault(o => o.Instance == Instance && o.Type == Type);
            return result;
        }
        /* Получает приказ на атаку отряда */
        public static string SendAttackOrder(SquadAction obj) 
        {
            Account attacker = AccountManager.GetAccountByToken(obj.token);
            Base victimBase = BaseManager.GetBaseByID(obj.to);
            if (attacker.UserID == victimBase.OwnerID) 
            {
                const string result = "cannotuseatyourself";
                return result;
            }
            if (!victimBase.IsActive)
            {
                const string result = "baseisinactive";
                return result;
            }

            return ProceedActions.Battle(attacker.UserID, victimBase.OwnerID);

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
    public class ProceedActions {
        /* Проводит атаку на базу */
        public static string Battle(int attackerID, int victimID)
        {
            string result;
            Player attacker = PlayerManager.GetPlayerByID(attackerID);
            Player victim = PlayerManager.GetPlayerByID(victimID);
            IEnumerable<Unit> attackerUnits = BaseManager.GetBaseUnits(attackerID);
            IEnumerable<Unit> victimUnits = BaseManager.GetBaseUnits(victimID);

            if (BaseManager.GetBaseUnitsCount(attackerID) == 0) 
            {
                return "nounits";
            }

            Log("Event", $"Player {attacker.Playername} initiated a battle.");

            int attackerPower = 0;
            int victimPower = 0;
            foreach (Unit unit in attackerUnits)
            {
                attackerPower += unit.Count * 10;
                Log("Battle", $"Player {attacker.Playername} has {unit.Count} units of {unit.Type} class.");
            }
            foreach (Unit unit in victimUnits)
            {
                Log("Battle", $"Player {victim.Playername} has {unit.Count} units of {unit.Type} class.");
                victimPower += unit.Count * 10; // todo: getPower(unit.Type);
            }

            if (attackerPower > victimPower)
            {
                double delta = (attackerPower > 0 && victimPower > 0 ? attackerPower / victimPower : 1);
                DoBattle(ref attacker, ref attackerUnits, ref victim, ref victimUnits, delta);
                result = "youwin";
            }
            else
            {
                double delta = (attackerPower > 0 && victimPower > 0 ? attackerPower / victimPower : 1);
                DoBattle(ref victim, ref victimUnits, ref attacker, ref attackerUnits, delta);
                result = "youlose";
            }
            Log("Event", $"Player {attacker.Playername} (units power is {attackerPower}) attacked {victim.Playername} (units power is {victimPower}) and {(attackerPower > victimPower ? "wins" : "loses")} that battle.");

            return result;
        }
        /* */
        /* Реализует механизм обработки последствий битвы (занесение очков в стату, обнуление юнитов у проигравшего и вычитание части у победителя) */
        /* */
        private static void DoBattle(ref Player winner, ref IEnumerable<Unit> winnerUnits, ref Player loser, ref IEnumerable<Unit> loserUnits, double delta)
        {
            using (Entities db = new Entities())
            {
                winner.Wins++;

                foreach (Unit unit in winnerUnits)
                {
                    unit.Count = (int)(unit.Count / delta);
                }

                loser.Loses++;

                foreach (Unit unit in loserUnits)
                {
                    db.Entry(unit).State = EntityState.Deleted;
                }
                db.SaveChanges();
            }
        }
        /* Реализует механизм покупки */
        public static void DoBuyItem(Base curbase, string itemName, int level = 1)
        {
            using (Entities db = new Entities()) {
                Resource resources = db.Resources.FirstOrDefault(o => o.Instance == "bas" + curbase.BaseID.ToString());
                var cost = GameItems.ItemsVars.GetCost(itemName);
                resources.Credits -= cost.Credits * level;
                resources.Energy -= cost.Energy * level;
                resources.Neutrino -= cost.Neutrino * level;
                db.SaveChanges();
            }

        System.Diagnostics.Debug.WriteLine("успешно купил эту шляпу");
        }
        /* */
        /* Реализует механизм логгирования */
        /* */
        public async static void Log(string type, string text)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            using (FileStream fstream = new FileStream($"{dir}/../csharpgame.log", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes($"{DateTime.Now.ToString("h:mm:ss tt")} [{type}] {text}{Environment.NewLine}");
                fstream.Seek(0, SeekOrigin.End);
                await fstream.WriteAsync(array, 0, array.Length);
            }
            // Вечная память нашему брату, с которым дебажили в консоли, имя ему - System.Diagnostics.Debug.WriteLine
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
        public static dynamic GetCost(string ItemType) // восхитительный полукостыль
        {
            switch (ItemType)
            {
                case "lifeComplex": return new { Credits = 300, Energy = 25, Neutrino = 0.0 };
                case "resourceComplex": return new { Credits = 100, Energy = 25, Neutrino = 0.0 };
                case "energyComplex": return new { Credits = 100, Energy = 25, Neutrino = 0.0 };
                case "aircraftsComplex": return new { Credits = 1000, Energy = 250, Neutrino = 0.0 };
                case "researchStation": return new { Credits = 100000, Energy = 250000, Neutrino = 0.0 };
                /**/
                case "droneUnit": return new { Credits = 100, Energy = 25, Neutrino = 0.0 };
                case "jetUnit": return new { Credits = 1000, Energy = 250, Neutrino = 0.0 };
                case "lincorUnit": return new { Credits = 10000, Energy = 2500, Neutrino = 0.0 };
                case "someGiantShitUnit": return new { Credits = 1000000, Energy = 250000, Neutrino = 1.0 };
                    /**/
                case "base": return new { Credits = 2000, Energy = 2000, Neutrino = 0.0 };
            }
            return new { Credits = 0, Energy = 0, Neutrino = 0.0 };
        }
    }
}