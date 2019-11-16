using gamelogic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace gamelogic
{
    /// <summary>
    /// Класс менеджера аккаунтов управляет авторизацией, регистрацией и прочим взаимодействием с аккаунтами
    /// </summary>
    public class AccountManager
    {
        /// <summary>
        /// Управляет авторизацией пользователя
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Управляет созданием аккаунта
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Возвращает аккаунт по его токену
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Account GetAccountByToken(string token)
        {
            using (Entities db = new Entities())
            {
                return db.Accounts.FirstOrDefault(o => o.Token == token);
            }
        }

        /// <summary>
        /// Проверяет токен игрока
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool CheckToken(string token)
        {
            return GetAccountByToken(token) != null;
        }
    }

    /// <summary>
    /// Класс менеджера игроков возвращает данные о игроках ("игрок" - персонаж, создаваемый для аккаунта)
    /// </summary>
    public class PlayerManager
    {
        /// <summary>
        /// Возвращает игрока по его ИД
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static Player GetPlayerByID(int userid)
        {
            using (Entities db = new Entities())
            {
                return db.Players.FirstOrDefault(o => o.UserID == userid);
            }
        }

        /// <summary>
        /// Возвращает базу игрока по его ИД
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static Base GetBaseByUserID(int userid)
        {
            using (Entities db = new Entities())
            {
                return db.Bases.FirstOrDefault(o => o.OwnerID == userid);
            }
        }

        /// <summary>
        /// Возвращает список игроков
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Player> GetPlayerList()
        {
            using (Entities db = new Entities())
            {
                return db.Players.ToList();
            }
        }

        /// <summary>
        /// Возвращает статистику игроков
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<StatsData> GetPlayerStats()
        {
            return null;
        }
    }

    /// <summary>
    /// Класс менеджера баз принимает и возвращает все сведения о базах игроков
    /// </summary>
    public class BaseManager
    {
        /// <summary>
        /// Возвращает список баз
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Base> GetBaseList()
        {
            using (Entities db = new Entities())
            {
                return db.Bases.ToList();
            }
        }

        /// <summary>
        /// Возвращает данные о базе по ИД
        /// </summary>
        /// <param name="baseid"></param>
        /// <returns></returns>
        public static Base GetBaseByID(int baseid)
        {
            using (Entities db = new Entities())
            {
                return db.Bases.FirstOrDefault(o => o.BaseID == baseid);
            }
        }

        /// <summary>
        /// Проверяет, является ли игрок владельцем базы
        /// </summary>
        /// <param name="baseid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool IsOwner(int baseid, string token)
        {
            Base curbase = GetBaseByID(baseid);
            Account Account = AccountManager.GetAccountByToken(token);
            return Account.UserID == curbase.OwnerID;
        }

        /// <summary>
        /// Проверяет вводимые данные
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Управляет улучшением базы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string UpgradeBase(BaseAction obj)
        {
            if (CheckInput(obj) != "success")
            {
                return CheckInput(obj);
            }
            try
            {
                if (IsOwner(obj.baseid, obj.token))
                {
                    Base curbase = GetBaseByID(obj.baseid);
                    if (!CanAfford(obj.baseid, "baseUpgrade", curbase.Level + 1))
                    {
                        return "notenoughresources";
                    }
                    ProceedActions.DoBuyItem(obj.baseid, "baseUpgrade", curbase.Level + 1);
                    using (Entities db = new Entities())
                    {
                        Base BaseEntry = db.Bases.FirstOrDefault(o => o.BaseID == obj.baseid);
                        BaseEntry.Level++;
                        db.SaveChanges();
                    }
                    ProceedActions.Log("Event", $"Base ID {obj.baseid} has been upgraded at 1 level up.");
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

        /// <summary>
        /// Управляет созданием юнитов
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string MakeUnit(BaseAction obj)
        {
            if (CheckInput(obj) != "success")
            {
                return CheckInput(obj);
            }
            try
            {
                if (IsOwner(obj.baseid, obj.token) && obj.result != null)
                {
                    if (HasBaseStructure(obj.baseid, "aircraftsComplex") == null)
                    {
                        return "noAircrafts";
                    }

                    if (!CanAfford(obj.baseid, obj.result))
                    {
                        return "notenoughresources";
                    }

                    var population = HasBaseStructure(obj.baseid, "lifeComplex");
                    if (population == null)
                    {
                        return "noLifeComplex";
                    }
                    if (population.Level * 7 <= GetBaseUnitsCount(obj.baseid))
                    {
                        return "populationLimit";
                    }

                    ProceedActions.DoBuyItem(obj.baseid, obj.result, 1);

                    Unit u = SquadManager.GetUnit("bas" + obj.baseid.ToString(), obj.result);

                    using (Entities db = new Entities())
                    {
                        if (u == null)
                        {
                            db.Units.Add(new Unit { Instance = "bas" + obj.baseid.ToString(), Type = obj.result, Count = 1 });
                        }
                        else
                        {
                            u.Count++;
                        }
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

        /// <summary>
        /// Управляет созданием построек
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string BuildStructure(BaseAction obj)
        {
            if (CheckInput(obj) != "success")
            {
                return CheckInput(obj);
            }

            try
            {
                if (IsOwner(obj.baseid, obj.token) && obj.result != null)
                {
                    obj.result = obj.result ?? "lifeComplex";

                    if (!CanAfford(obj.baseid, obj.result))
                    {
                        return "notenoughresources";
                    }
                    if (HasBaseStructure(obj.baseid, obj.result) != null)
                    {
                        return "alreadyexists";
                    }

                    ProceedActions.DoBuyItem(obj.baseid, obj.result, 1);

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

        /// <summary>
        /// Управляет улучшением построек
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string UpgradeStructure(BaseAction obj)
        {
            if (CheckInput(obj) != "success")
            {
                return CheckInput(obj);
            }
            try
            {
                if (IsOwner(obj.baseid, obj.token) && obj.result != null)
                {
                    if (!CanAfford(obj.baseid, obj.result))
                    {
                        return "notenoughresources";
                    }
                    if (HasBaseStructure(obj.baseid, obj.result) == null)
                    {
                        return "notexists";
                    }

                    int level;

                    using (Entities db = new Entities()) 
                    {
                        var str = db.Structures.FirstOrDefault(o => o.Type == obj.result && o.BaseID == obj.baseid);
                        str.Level++;
                        level = str.Level;
                        db.SaveChanges();
                    }

                    ProceedActions.DoBuyItem(obj.baseid, obj.result, level);

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

        /// <summary>
        /// Управляет починкой базы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string RepairBase(BaseAction obj)
        {
            if (CheckInput(obj) != "success")
            {
                return CheckInput(obj);
            }

            try
            {
                if (IsOwner(obj.baseid, obj.token))
                {
                    if (!CanAfford(obj.baseid, "base"))
                    {
                        return "notenoughresources";
                    }

                    int level;
                    using (Entities db = new Entities())
                    {
                        var BaseEntry = db.Bases.FirstOrDefault(o => o.BaseID == obj.baseid);
                        BaseEntry.IsActive = !BaseEntry.IsActive;
                        level = BaseEntry.Level;
                        db.SaveChanges(); 
                    }
                    ProceedActions.DoBuyItem(obj.baseid, "baseRepair", level);
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

        /// <summary>
        /// Проверяет на возможность приобрести предмет
        /// </summary>
        /// <param name="baseid"></param>
        /// <param name="itemName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static bool CanAfford(int baseid, string itemName, int level = 1)
        {
            Resource resources = GetBaseResources(baseid);
            var cost = GameItems.ItemsVars.GetCost(itemName);
            if (resources.Credits < cost.Credits * level || resources.Energy < cost.Energy * level)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Возвращает данные о базе
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public static Base GetBaseInfo(Account acc)
        {
            Base result;
            using (Entities db = new Entities())
            {
                result = db.Bases.FirstOrDefault(o => o.BaseID == acc.UserID);
            }
            return result;
        }

        /// <summary>
        /// Возвращает массив всех построек на базе
        /// </summary>
        /// <param name="BaseID"></param>
        /// <returns></returns>
        public static IEnumerable<Structure> GetBaseStructures(int BaseID)
        {
            using (Entities db = new Entities())
            {
                return db.Structures.Where(o => o.BaseID == BaseID).ToList();
            }
        }

        /// <summary>
        /// Возвращает постройку, если она была построена на базе
        /// </summary>
        /// <param name="baseid"></param>
        /// <param name="structure"></param>
        /// <returns></returns>
        public static Structure HasBaseStructure(int baseid, string structure)
        {
            Structure result;
            using (Entities db = new Entities())
            {
                result = db.Structures.FirstOrDefault(o => o.Type == structure && o.BaseID == baseid);
            }
            return result;
        }

        /// <summary>
        /// Возвращает массив всех ресурсов на базе
        /// </summary>
        /// <param name="BaseID"></param>
        /// <returns></returns>
        public static Resource GetBaseResources(int BaseID)
        {
            Resource result;
            using (Entities db = new Entities())
            {
                result = db.Resources.FirstOrDefault(o => o.Instance == "bas" + BaseID.ToString());
            }
            return result;
        }

        /// <summary>
        /// Возвращает массив всех юнитов на базе
        /// </summary>
        /// <param name="BaseID"></param>
        /// <returns></returns>
        public static IEnumerable<Unit> GetBaseUnits(int BaseID)
        {
            using (Entities db = new Entities())
            {
                return db.Units.Where(o => o.Instance == "bas" + BaseID.ToString() && o.Count > 0).ToList();
            }
        }

        /// <summary>
        /// Возвращает общее количество юнитов на базе
        /// </summary>
        /// <param name="BaseID"></param>
        /// <returns></returns>
        public static int GetBaseUnitsCount(int BaseID)
        {
            return 1;
            /*using (Entities db = new Entities())
                return db.Units
                .Where(o => o.Instance == "bas" + BaseID.ToString() && o.Count > 0)
                .GroupBy(o => o.Instance)
                .Select(p => new { Total = p.Sum(i => i.Count) }).FirstOrDefault().Total;*/
        }

        /// <summary>
        /// Назначает ("добывает") всем базам ресурсы исходя из наличия необходимых строений
        /// </summary>
        /// <returns></returns>
        public static bool BaseGatherResources()
        {
            using (Entities DB = new Entities())
            {
                var Bases = DB.Bases.ToList();
                foreach (Base CurrentBase in Bases)
                {
                    if (CurrentBase.IsActive)
                    {
                        Resource Resources = DB.Resources.FirstOrDefault(o => o.Instance == "bas" + CurrentBase.BaseID.ToString());

                        Structure creditsStruct = HasBaseStructure(CurrentBase.BaseID, "resourceComplex");
                        if (creditsStruct != null)
                        {
                            Resources.Credits += 10 * creditsStruct.Level / 10;
                        }

                        Structure energyStruct = HasBaseStructure(CurrentBase.BaseID, "energyComplex");
                        if (energyStruct != null)
                        {
                            Resources.Energy += 10 * energyStruct.Level / 10;
                        }

                        Structure neutrinoStruct = HasBaseStructure(CurrentBase.BaseID, "researchStation");
                        if (neutrinoStruct != null)
                        {
                            Resources.Neutrino += 0.000001 * neutrinoStruct.Level / 10;
                        }

                        // базовое значение * уровень здания / 10 частей в минуте
                    }

                }

                ProceedActions.Log("Event", "Routine() iteration");

                DB.SaveChanges();
            }
            return true;
        }
    }

    /// <summary>
    /// Класс менеджера отрядов управляет отрядами
    /// </summary>
    public class SquadManager
    {
        /// <summary>
        /// Возвращает отряд по его ключу
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Squad GetSquad(string key)
        {
            Squad result;
            using (Entities db = new Entities())
            {
                result = db.Squads.FirstOrDefault(o => o.Key == key);
            }
            return result;
        }

        /// <summary>
        /// Возвращает список всех отрядов
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Squad> GetSquads()
        {
            using (Entities db = new Entities())
            {
                return db.Squads.ToList();
            }
        }

        /// <summary>
        /// Получает приказ на возвращение отряда
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SendReturnOrder(SquadAction obj)
        {
            const string result = "success";
            return result;
        }

        /// <summary>
        /// Возвращает количество юнитов в инстансе
        /// </summary>
        /// <param name="Instance"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static Unit GetUnit(string Instance, string Type)
        {
            Unit result;
            using (Entities db = new Entities())
            {
                result = db.Units.FirstOrDefault(o => o.Instance == Instance && o.Type == Type);
            }
            return result;
        }

        /// <summary>
        /// Получает приказ на атаку отряда
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        }
    }

    /// <summary>
    /// Класс проведения действий управляет свершениями различных игровых действий
    /// </summary>
    public class ProceedActions
    {
        /// <summary>
        /// Проводит атаку на базу
        /// </summary>
        /// <param name="attackerID"></param>
        /// <param name="victimID"></param>
        /// <returns></returns>
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
                attackerPower += unit.Count * GameItems.ItemsVars.GetCost(unit.Type);
                Log("Battle", $"Player {attacker.Playername} has {unit.Count} units of {unit.Type} class.");
            }
            foreach (Unit unit in victimUnits)
            {
                Log("Battle", $"Player {victim.Playername} has {unit.Count} units of {unit.Type} class.");
                victimPower += unit.Count * GameItems.ItemsVars.GetCost(unit.Type);
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
            Log("Event", $"Player {attacker.Playername} (units power is {attackerPower}) attacked {victim.Playername} " +
                $"(units power is {victimPower}) and {(attackerPower > victimPower ? "wins" : "loses")} that battle.");

            return result;
        }

        /// <summary>
        /// Реализует механизм обработки последствий битвы (занесение очков в стату, обнуление юнитов у проигравшего и вычитание части у победителя)
        /// </summary>
        /// <param name="winner"></param>
        /// <param name="winnerUnits"></param>
        /// <param name="loser"></param>
        /// <param name="loserUnits"></param>
        /// <param name="delta"></param>
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

        /// <summary>
        ///  Реализует механизм покупки
        /// </summary>
        /// <param name="baseid"></param>
        /// <param name="itemName"></param>
        /// <param name="level"></param>
        public static void DoBuyItem(int baseid, string itemName, int level = 1)
        {
            using (Entities db = new Entities())
            {
                Resource resources = db.Resources.FirstOrDefault(o => o.Instance == "bas" + baseid.ToString());
                var cost = GameItems.ItemsVars.GetCost(itemName);
                resources.Credits -= cost.Credits * level;
                resources.Energy -= cost.Energy * level;
                resources.Neutrino -= cost.Neutrino * level;
                db.SaveChanges();
            }

        System.Diagnostics.Debug.WriteLine("успешно купил эту шляпу");
        }

        /// <summary>
        /// Реализует механизм логгирования
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
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

/// <summary>
/// Предполагается, что здесь будут храниться константы игровых единиц (юниты, строения, прочее)
/// </summary>
namespace GameItems 
{
    class ItemsVars
    {
        /// <summary>
        /// Возвращает базовую стоимость игровой единицы
        /// </summary>
        /// <param name="ItemType"></param>
        /// <returns></returns>
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
                case "repairBase": return new { Credits = 200, Energy = 200, Neutrino = 0.0 };
                case "upgradeBase": return new { Credits = 2000, Energy = 2000, Neutrino = 0.0 };
            }
            return new { Credits = 0, Energy = 0, Neutrino = 0.0 };
        }

        /// <summary>
        /// Возвращает показатель силы юнита
        /// </summary>
        /// <param name="ItemType"></param>
        /// <returns></returns>
        public static int GetPower(string ItemType)
        {
            switch (ItemType)
            {
                case "droneUnit": return 10;
                case "jetUnit": return 100;
                case "lincorUnit": return 1500;
                case "someGiantShitUnit": return 200000;
            }
            return 1;
        }
    }
}