using System;
using System.Collections.Generic;
using AutoMapper;
using gamelogic;

namespace WcfService
{
    public class Service1 : IService1
    {
        // auth section
        public string SendAuthData(AuthData data)
        {
            string result = "";
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<AuthData, gamelogic.Models.AuthData>();
            });
            try
            {
                IMapper mapper = config.CreateMapper();
                result = AccountManager.AuthClient(mapper.Map<AuthData, gamelogic.Models.AuthData>(data));
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
        public IEnumerable<StatEntity> GetUserList()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<gamelogic.Player, StatEntity>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map< IEnumerable<gamelogic.Player>, IEnumerable< StatEntity >>(TestLogic.GetUserList());
        }

        // base section
        public string BaseAction(BaseAction obj)
        {
            if (obj == null) return "nodatareceived";
            if (!AccountManager.CheckToken(obj.token)) return "wrongtoken";

            string result;

            gamelogic.Models.BaseAction mapobj = Tools.BaseActionMapper(obj);

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

            gamelogic.Models.BaseAction mapobj = Tools.BaseActionMapper(obj);

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<gamelogic.Base, BaseEntity>();
                cfg.CreateMap<gamelogic.Structure, StructureEntity>();
            });
            IMapper mapper = config.CreateMapper();
            Account acc = AccountManager.GetAccountByToken(obj.token);
            BaseEntity result = mapper.Map<gamelogic.Base, BaseEntity>(BaseManager.GetBaseInfo(acc));

            System.Diagnostics.Debug.WriteLine(string.Format($"{obj.result}, {obj.baseid}, {obj.action}, {obj.token}"));
            result.Structures = mapper.Map<IEnumerable<gamelogic.Structure>, IEnumerable<StructureEntity>>(BaseManager.GetBaseStructures(BaseManager.GetBaseInfo(acc))); 
            result.Resources = new object[0];// заглушки
            result.Units = new object[0];
            return result;
        }
        public IEnumerable<StructureEntity> GetBaseStructures(BaseAction obj)
        {
            if (obj == null) return null;

            gamelogic.Models.BaseAction mapobj = Tools.BaseActionMapper(obj);

            if (!AccountManager.CheckToken(mapobj.token)) return null;

            var config2 = new MapperConfiguration(cfg => {
                cfg.CreateMap<gamelogic.Structure, StructureEntity>();
            });
            IMapper mapper2 = config2.CreateMapper();
            return mapper2.Map< IEnumerable<gamelogic.Structure>, IEnumerable<StructureEntity>>(BaseManager.GetBaseStructures(mapobj));
        }
        public IEnumerable<Squad> GetSquads(SquadAction obj)
        {
            /*if (obj == null) return null;

            gamelogic.Models.SquadAction mapobj = Tools.BaseActionMapper(obj);

            if (!AccountManager.CheckToken(mapobj.token)) return null;

            var config2 = new MapperConfiguration(cfg => {
                cfg.CreateMap<gamelogic.Structure, StructureEntity>();
            });
            IMapper mapper2 = config2.CreateMapper();
            return mapper2.Map<IEnumerable<gamelogic.Structure>, IEnumerable<StructureEntity>>(BaseManager.GetBaseStructures(mapobj));*/
            return null;
        }
        public string SquadAction(SquadAction obj)
        {
            if (obj == null) return "nodatareceived";

            string result = null;

            gamelogic.Models.SquadAction mapobj = Tools.SmartMapper<gamelogic.Models.SquadAction>(obj);

            if (!AccountManager.CheckToken(mapobj.token)) return "wrongtoken";
            try
            {
                /*switch (mapobj.action)
                {
                    case "attack":
                        result = BaseManager.UpgradeBase(mapobj);
                        break;
                    case "return":
                        result = BaseManager.RepairBase(mapobj);
                        break;
                    default:
                        result = "wrongaction";
                        break;
                }*/
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
        public static gamelogic.Models.BaseAction BaseActionMapper(BaseAction obj) {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<BaseAction, gamelogic.Models.BaseAction>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<BaseAction, gamelogic.Models.BaseAction>(obj);
        }
        public static T SmartMapper<T>(dynamic obj) // тестовая мульти-маппер-функция
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<dynamic, T>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<dynamic, T>(obj);
        }
    }
}