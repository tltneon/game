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
            string result = "";
            try
            {
                result = AccountManager.AuthClient(Tools.SmartMapper<AuthData, gamelogic.Models.AuthData>(data));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("damn err");
                System.Diagnostics.Debug.WriteLine($"Исключение: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Метод: {ex.TargetSite}");
                System.Diagnostics.Debug.WriteLine($"Трассировка стека: {ex.StackTrace}");
            }
            System.Diagnostics.Debug.WriteLine(string.Format("Registering user {0} with pass {1}. Result:" + result, data.username, data.password));
            return result;
        }
        public IEnumerable<StatEntity> GetPlayerList()
        {
            return Tools.EnumSmartMapper<gamelogic.Player, StatEntity>(PlayerManager.GetPlayerList());
        }

        // base section
        public string BaseAction(BaseAction obj)
        {
            if (obj == null) return "nodatareceived";
            if (!AccountManager.CheckToken(obj.token)) return "wrongtoken";

            string result;

            gamelogic.Models.BaseAction mapobj = Tools.SmartMapper<BaseAction, gamelogic.Models.BaseAction>(obj);

            System.Diagnostics.Debug.WriteLine($"дебаг: {mapobj.action} {mapobj.baseid} {mapobj.result} {mapobj.token}");
            
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
                System.Diagnostics.Debug.WriteLine("damn err");
                System.Diagnostics.Debug.WriteLine($"Исключение: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Метод: {ex.TargetSite}");
                System.Diagnostics.Debug.WriteLine($"Трассировка стека: {ex.StackTrace}");
                result = "err";
            }
            return result;
        }
        public BaseEntity GetBaseInfo(BaseAction obj)
        {
            if (obj == null) return null;

            if (!AccountManager.CheckToken(obj.token)) return null;

            System.Diagnostics.Debug.WriteLine("Token passed");
            System.Diagnostics.Debug.WriteLine(string.Format($"{obj.result}, {obj.baseid}, {obj.action}, {obj.token}"));

            Account acc = AccountManager.GetAccountByToken(obj.token);

            BaseEntity result = Tools.SmartMapper<gamelogic.Base, BaseEntity>(BaseManager.GetBaseInfo(acc));

            result.Structures = Tools.EnumSmartMapper<gamelogic.Structure, StructureEntity>(BaseManager.GetBaseStructures(BaseManager.GetBaseInfo(acc))); 
            result.Resources = new object[0];// заглушки
            result.Units = new object[0];

            return result;
        }
        public IEnumerable<StructureEntity> GetBaseStructures(BaseAction obj)
        {
            if (obj == null) return null;

            if (!AccountManager.CheckToken(obj.token)) return null;

            return Tools.EnumSmartMapper<gamelogic.Structure, StructureEntity>(BaseManager.GetBaseStructures(Tools.SmartMapper<BaseAction, gamelogic.Models.BaseAction>(obj)));
        }
        // squad section
        public IEnumerable<SquadEntity> GetSquads(SquadAction obj)
        {
            if (obj == null) return null;
            
            if (!AccountManager.CheckToken(obj.token)) return null;

            return Tools.EnumSmartMapper<gamelogic.Squad, SquadEntity>(SquadManager.GetSquads());
        }
        public string SquadAction(SquadAction obj)
        {
            if (obj == null) return "nodatareceived";
            if (!AccountManager.CheckToken(obj.token)) return "wrongtoken";

            string result;
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
        public static TDestination SmartMapper<TSource, TDestination>(TSource obj) // тестовая мульти-маппер-функция
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>();
            });
            System.Diagnostics.Debug.WriteLine("=> smart mapper - гордость поколений =>");
            IMapper mapper = config.CreateMapper();
            return mapper.Map<TSource, TDestination>(obj); // огонь, это работает!
        }
        public static IEnumerable<TDestination> EnumSmartMapper<TSource, TDestination>(IEnumerable<TSource> obj) // а это младший брат тестовой мульти-маппер-функции
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>();
            });
            System.Diagnostics.Debug.WriteLine("=> smart mapper jr. - гордость поколений =>");
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(obj); // шикос, и эта теперь тоже работает
        }
    }
}