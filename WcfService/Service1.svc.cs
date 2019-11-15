using System;
using System.Collections.Generic;
using AutoMapper;
using gamelogic;

namespace WcfService
{
    public class Service1 : IService1
    {
        // >> auth-n-player section
        /* */
        /* Управляет авторизацией и регистрацией пользователей */
        /* */
        public string SendAuthData(AuthData data)
        {
            if (data == null) return "nodatareceived";
            if (data.username == null || data.password == null || data.username.Length < 3 || data.password.Length < 3 || data.username.Length > 20 || data.password.Length > 20) return "wrongdatareceived";
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
        /* */
        /* Возвращает список всех игроков */
        /* */
        public IEnumerable<StatEntity> GetPlayerStats()
        {
            return Tools.EnumSmartMapper<gamelogic.Player, StatEntity>(PlayerManager.GetPlayerList());
        }

        // >> base section
        /* */
        /* возвращает список всех баз */
        /* */
        public IEnumerable<BaseEntity> GetBaseList()
        {
            return Tools.EnumSmartMapper<gamelogic.Base, BaseEntity>(BaseManager.GetBaseList());
        }
        /* */
        /* Реализует управление базой */
        /* */
        public string BaseAction(BaseAction obj)
        {
            string result = Tools.CheckAuthedInput(obj);
            if (result != "passed") return result;

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
        /* */
        /* Возвращает данные о базе */
        /* */
        public BaseEntity GetBaseInfo(BaseAction obj)
        {
            if (Tools.CheckAuthedInput(obj) != "passed") return null;

            Account acc = AccountManager.GetAccountByToken(obj.token);

            gamelogic.Base curbase = BaseManager.GetBaseInfo(acc);

            BaseEntity result = Tools.SmartMapper<gamelogic.Base, BaseEntity>(curbase);

            result.Structures = Tools.EnumSmartMapper<gamelogic.Structure, StructureEntity>(BaseManager.GetBaseStructures(curbase.BaseID)); 
            result.Resources = Tools.SmartMapper<gamelogic.Resource, ResourcesData>(BaseManager.GetBaseResources(curbase.BaseID));
            result.Units = Tools.EnumSmartMapper<gamelogic.Unit, UnitsData>(BaseManager.GetBaseUnits(curbase.BaseID));

            return result;
        }
        /* */
        /* Возвращает список всех строений на базе */
        /* */
        public IEnumerable<StructureEntity> GetBaseStructures(BaseAction obj)
        {
            string result = Tools.CheckAuthedInput(obj);
            if (result != "passed") return null;

            return Tools.EnumSmartMapper<gamelogic.Structure, StructureEntity>(BaseManager.GetBaseStructures(obj.baseid));
        }
        /* */
        /* Возвращает список всех отрядов на базе */
        /* */
        public IEnumerable<UnitsData> GetBaseUnits(BaseAction obj)
        {
            string result = Tools.CheckAuthedInput(obj);
            if (result != "passed") return null;

            return Tools.EnumSmartMapper<gamelogic.Unit, UnitsData>(BaseManager.GetBaseUnits(obj.baseid));
        }

        // >> squad section
        /* */
        /* Возвращает список всех отрядов */
        /* */
        public IEnumerable<SquadEntity> GetSquads(SquadAction obj)
        {
            string result = Tools.CheckAuthedInput(obj);
            if (result != "passed") return null;

            return Tools.EnumSmartMapper<gamelogic.Squad, SquadEntity>(SquadManager.GetSquads());
        }
        /* */
        /* Реализует управление отрядами */
        /* */
        public string SquadAction(SquadAction obj)
        {
            string result = Tools.CheckAuthedInput(obj);
            if (result != "passed") return result;

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
        /* */
        /* Это очень плохая реализация игрового лупа, очень плохая и ничем не защищена */
        /* */
        public void DbStatus()
        {
            BaseManager.BaseGatherResources();
        }
    }
    /* */
    /* Забацал свои умные функции для удобной работы с кодом */
    /* */
    class Tools
    {

        /* */
        /* Маппер для самостоятельных энтити */
        /* */
        public static TDestination SmartMapper<TSource, TDestination>(TSource obj) // мульти-маппер-функция на полную ставку
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<TSource, TDestination>(obj);
        }
        /* */
        /* Маппер для энтити в коллекции */
        /* */
        public static IEnumerable<TDestination> EnumSmartMapper<TSource, TDestination>(IEnumerable<TSource> obj)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(obj);
        }
        /* */
        /* Проверяет пользовательствий ввод на нуль + валидный токен */
        /* */
        public static string CheckAuthedInput(dynamic obj)
        {
            if (obj == null) return "nodatareceived";
            if (obj.token == null) return "notokenreceived";
            if (!AccountManager.CheckToken(obj.token)) return "wrongtoken";
            return "passed";
        }
    }
}