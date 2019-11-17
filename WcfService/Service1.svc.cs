using System;
using System.Collections.Generic;
using AutoMapper;
using gamelogic;

namespace wcfservice
{
    /// <summary>
    /// Класс для обращения к сервису WCF
    /// </summary>
    public class Service1 : IService1
    {
        // >> auth-n-player section
        /// <summary>
        /// Управляет авторизацией и регистрацией пользователей
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SendAuthData(AuthData data)
        {
            if (data == null)
            {
                return "nodatareceived";
            }
            if (data.username == null || data.password == null || data.username.Length < 3 || 
                data.password.Length < 3 || data.username.Length > 20 || data.password.Length > 20)
            {
                return "wrongdatareceived";
            }
            try
            {
                return AccountManager.AuthClient(Tools.SmartMapper<AuthData, gamelogic.Models.AuthData>(data));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Исключение: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Метод: {ex.TargetSite}");
                System.Diagnostics.Debug.WriteLine($"Трассировка стека: {ex.StackTrace}");

                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция SendAuthData");
                return ex.Message;
            }
        }

        /// <summary>
        /// Возвращает список всех игроков
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StatEntity> GetPlayerList() => 
            Tools.EnumSmartMapper<gamelogic.Player, StatEntity>(PlayerManager.GetPlayerList());

        // >> base section
        /// <summary>
        /// возвращает список всех баз
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseEntity> GetBaseList() => 
            Tools.EnumSmartMapper<gamelogic.Base, BaseEntity>(BaseManager.GetBaseList());

        /// <summary>
        /// Реализует управление базой
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string BaseAction(BaseAction obj)
        {
            string result = Tools.CheckAuthedInput(obj);
            if (result != "passed")
            {
                return result;
            }

            gamelogic.Models.BaseAction mapobj = Tools.SmartMapper<BaseAction, gamelogic.Models.BaseAction>(obj);
            try
            {
                switch (mapobj.action) {
                    case "upgrade":
                        result = BaseManager.UpgradeBase(mapobj);
                        break;
                    case "repair":
                        result = BaseManager.RepairBase(mapobj);
                        break;
                    case "build":
                        result = BaseManager.BuildStructure(mapobj);
                        break;
                    case "upgradestructure":
                        result = BaseManager.UpgradeStructure(mapobj);
                        break;
                    case "makeunit":
                        result = BaseManager.MakeUnit(mapobj);
                        break;
                    default:
                        result = "wrongaction";
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Исключение: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Метод: {ex.TargetSite}");
                System.Diagnostics.Debug.WriteLine($"Трассировка стека: {ex.StackTrace}");
                result = ex.Message;
                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция BaseAction");
            }
            return result;
        }

        /// <summary>
        /// Возвращает данные о базе
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public BaseEntity GetBaseInfo(BaseAction obj)
        {
            if (Tools.CheckAuthedInput(obj) != "passed")
            {
                return null;
            }

            Account acc = AccountManager.GetAccountByToken(obj.token);

            gamelogic.Base curbase = BaseManager.GetBaseInfo(acc);

            BaseEntity result = Tools.SmartMapper<gamelogic.Base, BaseEntity>(curbase);

            result.Structures = Tools.EnumSmartMapper<gamelogic.Structure, StructureEntity>(BaseManager.GetBaseStructures(curbase.BaseID)); 
            result.Resources = Tools.SmartMapper<gamelogic.Resource, ResourcesData>(BaseManager.GetBaseResources(curbase.BaseID));
            result.Units = Tools.EnumSmartMapper<gamelogic.Unit, UnitsData>(BaseManager.GetBaseUnits(curbase.BaseID));

            return result;
        }

        /// <summary>
        /// Возвращает список всех строений на базе
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<StructureEntity> GetBaseStructures(BaseAction obj)
        {
            string result = Tools.CheckAuthedInput(obj);
            if (result != "passed")
            {
                return null;
            }

            return Tools.EnumSmartMapper<gamelogic.Structure, StructureEntity>(BaseManager.GetBaseStructures(obj.baseid));
        }

        /// <summary>
        /// Возвращает список всех отрядов на базе
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<UnitsData> GetBaseUnits(BaseAction obj)
        {
            string result = Tools.CheckAuthedInput(obj);
            if (result != "passed")
            {
                return null;
            }

            return Tools.EnumSmartMapper<gamelogic.Unit, UnitsData>(BaseManager.GetBaseUnits(obj.baseid));
        }

        // >> squad section
        /// <summary>
        /// Возвращает список всех отрядов
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<SquadEntity> GetSquads(SquadAction obj)
        {
            string result = Tools.CheckAuthedInput(obj);
            if (result != "passed")
            {
                return null;
            }

            return Tools.EnumSmartMapper<gamelogic.Squad, SquadEntity>(SquadManager.GetSquads());
        }

        /// <summary>
        /// Реализует управление отрядами
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SquadAction(SquadAction obj)
        {
            string result = Tools.CheckAuthedInput(obj);
            if (result != "passed")
            {
                return result;
            }

            try
            {
                gamelogic.Models.SquadAction mapobj = Tools.SmartMapper<SquadAction, gamelogic.Models.SquadAction>(obj);
                switch (obj.action)
                {
                    case "attack":
                        result = SquadManager.SendAttackOrder(mapobj);
                        break;
                    case "return":
                        result = SquadManager.SendReturnOrder(mapobj);
                        break;
                    default:
                        result = "wrongaction";
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Исключение: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Метод: {ex.TargetSite}");
                System.Diagnostics.Debug.WriteLine($"Трассировка стека: {ex.StackTrace}");

                ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция SquadAction");
                result = "err";
            }
            return result;
        }

        /// <summary>
        ///  Это очень плохая реализация игрового лупа, очень плохая и ничем не защищена
        /// </summary>
        public void DbStatus() => BaseManager.BaseGatherResources();
    }
}