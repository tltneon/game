using gamelogic.Models;
using System.Collections.Generic;
using System.Linq;

namespace gamelogic
{
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
            using (var db = new Entities())
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
            using (var db = new Entities())
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
            using (var db = new Entities())
            {
                return db.Players.ToList();
            }
        }

        /// <summary>
        /// Ищет игрока по нику
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Player FindByName(string name)
        {
            using (var db = new Entities())
            {
                return db.Players.FirstOrDefault(o => o.Playername == name);
            }
        }

        public static IEnumerable<StatsData> GetStats()
        {
            using (var db = new Entities())
            {
                return db.Database.SqlQuery<StatsData>("" +
                    "SELECT p.Playername, p.Wins, p.Loses, b.Basename, b.Level " +
                    "FROM Players p, Bases b " +
                    "WHERE p.UserID = b.OwnerID").ToList();
            }
        }
    }
}
