using System;
using System.Collections.Generic;
using AutoMapper;
using gamelogic;

namespace WcfService
{
    public class Service1 : IService1
    {
        // auth-n-player section
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
                return ex.Message;
            }
        }
        public IEnumerable<StatEntity> GetPlayerList()
        {
            return Tools.EnumSmartMapper<gamelogic.Player, StatEntity>(PlayerManager.GetPlayerList());
        }

        // base section
        public IEnumerable<BaseEntity> GetBaseList()
        {
            return Tools.EnumSmartMapper<gamelogic.Base, BaseEntity>(BaseManager.GetBaseList());
        }
        public string BaseAction(BaseAction obj)
        {
            string result = Tools.CheckAuthedInput(obj.token);
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
            }
            return result;
        }
        public BaseEntity GetBaseInfo(BaseAction obj)
        {
            if (Tools.CheckAuthedInput(obj) != "passed") return null;

            System.Diagnostics.Debug.WriteLine("Token passed");
            System.Diagnostics.Debug.WriteLine(string.Format($"{obj.result}, {obj.baseid}, {obj.action}, {obj.token}"));

            Account acc = AccountManager.GetAccountByToken(obj.token);

            gamelogic.Base curbase = BaseManager.GetBaseInfo(acc);

            BaseEntity result = Tools.SmartMapper<gamelogic.Base, BaseEntity>(curbase);

            result.Structures = Tools.EnumSmartMapper<gamelogic.Structure, StructureEntity>(BaseManager.GetBaseStructures(curbase.BaseID)); 
            result.Resources = new object[0]; // заглушки
            result.Units = new object[0];

            return result;
        }
        public IEnumerable<StructureEntity> GetBaseStructures(BaseAction obj)
        {
            string result = Tools.CheckAuthedInput(obj);
            if (result != "passed") return null;

            return Tools.EnumSmartMapper<gamelogic.Structure, StructureEntity>(BaseManager.GetBaseStructures(obj.baseid));
        }
        // squad section
        public IEnumerable<SquadEntity> GetSquads(SquadAction obj)
        {
            string result = Tools.CheckAuthedInput(obj);
            if (result != "passed") return null;

            return Tools.EnumSmartMapper<gamelogic.Squad, SquadEntity>(SquadManager.GetSquads());
        }
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
                System.Diagnostics.Debug.WriteLine("damn err");
                System.Diagnostics.Debug.WriteLine($"Исключение: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Метод: {ex.TargetSite}");
                System.Diagnostics.Debug.WriteLine($"Трассировка стека: {ex.StackTrace}");
                result = "err";
            }
            return result;
        }
        public string DbStatus()
        {
            System.Diagnostics.Debug.WriteLine(gamelogic.DbManager.GetContext());
            return "success request";
        }
    }

    class Tools
    {
        public static TDestination SmartMapper<TSource, TDestination>(TSource obj) // мульти-маппер-функция на полную ставку
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>();
            });
            System.Diagnostics.Debug.WriteLine("=> smart mapper - гордость поколений =>");
            IMapper mapper = config.CreateMapper();
            return mapper.Map<TSource, TDestination>(obj);
        }
        public static IEnumerable<TDestination> EnumSmartMapper<TSource, TDestination>(IEnumerable<TSource> obj)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(obj);
        }
        public static string CheckAuthedInput(dynamic obj)
        {
            if (obj == null) return "nodatareceived";
            if (obj.token == null) return "notokenreceived";
            if (!AccountManager.CheckToken(obj.token)) return "wrongtoken";
            return "passed";
        }
    }
}