using gamelogic.Models;
using System.Collections.Generic;
using System.Linq;

namespace gamelogic
{
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
            var result = new Squad { };
            using (Entities db = new Entities())
            {
                result = db.Squads.FirstOrDefault(o => o.Key == key);
            }
            return result;
        }

        /// <summary>
        /// Возвращает список юнитов в инстансе
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Unit> GetInstanceUnits(string Instance)
        {
            using (var db = new Entities())
            {
                return db.Units.Where(o => o.Instance == Instance && o.Count > 0).ToList();
            }
        }

        /// <summary>
        /// Возвращает список всех отрядов
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Squad> GetSquads()
        {
            using (var db = new Entities())
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
            var result = new Unit { };
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
            var attacker = AccountManager.GetAccountByToken(obj.token);
            var victimBase = BaseManager.GetBaseByID(obj.to);
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

            return ProceedActions.Battle(attacker, victimBase);
        }
    }
}
