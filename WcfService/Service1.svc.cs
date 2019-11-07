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

            string result;

            gamelogic.Models.BaseAction mapobj = Tools.BaseActionMapper(obj);

            System.Diagnostics.Debug.WriteLine($"дебаг: {mapobj.action} {mapobj.baseid} {mapobj.result} {mapobj.token}");

            if (!AccountManager.CheckToken(mapobj.token)) return "wrongtoken";
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

            gamelogic.Models.BaseAction mapobj = Tools.BaseActionMapper(obj);

            System.Diagnostics.Debug.WriteLine($"дебаг: {mapobj.action} {mapobj.baseid} {mapobj.result} {mapobj.token}");

            if (!AccountManager.CheckToken(mapobj.token)) return null;

            var config2 = new MapperConfiguration(cfg => {
                cfg.CreateMap<gamelogic.Base, BaseEntity>();
            });
            IMapper mapper2 = config2.CreateMapper();
            return mapper2.Map<gamelogic.Base, BaseEntity>(BaseManager.GetBaseInfo(mapobj));
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

        //other

        public string SendData(string username, string password)
        {
            string test = "";
            try
            {
                test = AccountManager.CreateUser(username, password);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("damn err");
            }
            return string.Format("Registering user {0} with pass {1}. Result:" + test, username, password);
        }
        public AuthData GetDummyUserData() { 
            return new AuthData { username = "testuser", password = "testpass" };
        }
        public string GetData(int value)
        {
            string test ="";
            try
            {
                test = AccountManager.CreateUser("noshit","bull");
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("damn err");
            }
            return string.Format("Registering user {0} with pass {1}. Salt: {2}. Result:" + test, "noshit", "bull", value);
        }
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
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
        public void CheckCorrectInput() { return; }
    }
}