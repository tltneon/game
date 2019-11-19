using gamelogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gamelogic
{
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
            using (var db = new Entities())
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
            var curbase = GetBaseByID(baseid);
            var Account = AccountManager.GetAccountByToken(token);
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
                    var curbase = GetBaseByID(obj.baseid);
                    if (!CanAfford(obj.baseid, "baseUpgrade", curbase.Level))
                    {
                        return "notenoughresources";
                    }
                    ProceedActions.DoBuyItem(obj.baseid, "upgradeBase", curbase.Level);
                    using (Entities db = new Entities())
                    {
                        var BaseEntry = db.Bases.FirstOrDefault(o => o.BaseID == obj.baseid);
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
                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция BaseManager.UpgradeBase");
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
                    ProceedActions.Log("Event", $"Игрок покупает юнит. На базе популяция {ItemsVars.GetPopulation(population.Level)} " +
                        $"и уже {GetBaseUnitsCount(obj.baseid)} юнитов создано");
                    if (ItemsVars.GetPopulation(population.Level) <= GetBaseUnitsCount(obj.baseid))
                    {
                        return "populationLimit";
                    }

                    ProceedActions.DoBuyItem(obj.baseid, obj.result);

                    var u = SquadManager.GetUnit("bas" + obj.baseid.ToString(), obj.result);
                    ProceedActions.Log("Event", $"В инстансе {obj.baseid.ToString()} {u.Count} юнитов типа {obj.result}");

                    using (var db = new Entities())
                    {
                        if (u == null)
                        {
                            db.Units.Add(new Unit { Instance = "bas" + obj.baseid.ToString(), Type = obj.result, Count = 1 });
                        }
                        else
                        {
                            var unit = db.Units.FirstOrDefault(o => o.Instance == u.Instance && o.Type == u.Type);
                            unit.Count++;
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
                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция BaseManager.MakeUnit");
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

                    var Struct = HasBaseStructure(obj.baseid, obj.result);
                    if (Struct != null)
                    {
                        return "alreadyexists";
                    }
                    if (!CanAfford(obj.baseid, obj.result))
                    {
                        return "notenoughresources";
                    }

                    ProceedActions.DoBuyItem(obj.baseid, obj.result);

                    using (var db = new Entities())
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
                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция BaseManager.BuildStructure");
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
                    var Struct = HasBaseStructure(obj.baseid, obj.result);
                    if (Struct == null)
                    {
                        return "notexists";
                    }
                    if (!CanAfford(obj.baseid, obj.result, Struct.Level))
                    {
                        return "notenoughresources";
                    }

                    ProceedActions.DoBuyItem(obj.baseid, obj.result, Struct.Level);

                    using (var db = new Entities())
                    {
                        var str = db.Structures.FirstOrDefault(o => o.Type == obj.result && o.BaseID == obj.baseid);
                        str.Level++;
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
                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция BaseManager.UpgradeStructure");
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
                    var BaseData = GetBaseByID(obj.baseid);
                    if (!CanAfford(obj.baseid, "repairBase", BaseData.Level))
                    {
                        return "notenoughresources";
                    }
                    ProceedActions.DoBuyItem(obj.baseid, "baseRepair", BaseData.Level);

                    using (var db = new Entities())
                    {
                        var BaseEntry = db.Bases.FirstOrDefault(o => o.BaseID == obj.baseid);
                        BaseEntry.IsActive = !BaseEntry.IsActive;
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
                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция BaseManager.RepairBase");
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
        public static bool CanAfford(int baseid, string itemName, int level = 0)
        {
            var resources = GetBaseResources(baseid);
            var cost = ItemsVars.GetCost(itemName);
            level++;
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
            var result = new Base { };
            using (var db = new Entities())
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
            using (var db = new Entities())
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
            var result = new Structure { };
            using (var db = new Entities())
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
            var result = new Resource { };
            using (var db = new Entities())
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
        public static IEnumerable<Unit> GetBaseUnits(int BaseID) => SquadManager.GetInstanceUnits("bas" + BaseID.ToString());

        /// <summary>
        /// Возвращает общее количество юнитов на базе
        /// </summary>
        /// <param name="BaseID"></param>
        /// <returns></returns>
        public static int GetBaseUnitsCount(int BaseID)
        {
            var units = GetBaseUnits(BaseID);
            var sum = 0;
            foreach (var u in units) 
            {
                sum += u.Count;
            }
            return sum;
        }

        /// <summary>
        /// Выдаёт всем базам ресурсы исходя из наличия необходимых строений
        /// </summary>
        /// <returns></returns>
        public static bool BaseGatherResources()
        {
            using (var DB = new Entities())
            {
                var Bases = DB.Bases.ToList();
                foreach (var CurrentBase in Bases)
                {
                    if (CurrentBase.IsActive)
                    {
                        var Resources = DB.Resources.FirstOrDefault(o => o.Instance == "bas" + CurrentBase.BaseID.ToString());

                        var creditsStruct = HasBaseStructure(CurrentBase.BaseID, "resourceComplex");
                        if (creditsStruct != null)
                        {
                            Resources.Credits += 10 * creditsStruct.Level / 10;
                        }

                        var energyStruct = HasBaseStructure(CurrentBase.BaseID, "energyComplex");
                        if (energyStruct != null)
                        {
                            Resources.Energy += 10 * energyStruct.Level / 10;
                        }

                        var neutrinoStruct = HasBaseStructure(CurrentBase.BaseID, "researchStation");
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
}
