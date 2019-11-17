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

        public static Player FindByName(string name)
        {
            using (Entities db = new Entities())
            {
                return db.Players.FirstOrDefault(o => o.Playername == name);
            }
        }
    }
}
