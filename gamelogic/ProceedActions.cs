using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace gamelogic
{
    /// <summary>
    /// Класс проведения действий управляет свершениями различных игровых действий
    /// </summary>
    public class ProceedActions
    {
        /// <summary>
        /// Проводит атаку на базу
        /// </summary>
        /// <param name="attackerID"></param>
        /// <param name="victimID"></param>
        /// <returns></returns>
        public static string Battle(Account attackerAcc, Base victimBase)
        {
            if (BaseManager.GetBaseUnitsCount(attackerAcc.UserID) == 0)
            {
                return "nounits";
            }
            var attacker = PlayerManager.GetPlayerByID(attackerAcc.UserID);
            var victim = PlayerManager.GetPlayerByID(victimBase.OwnerID);
            var attackerUnits = BaseManager.GetBaseUnits(attackerAcc.UserID);
            var victimUnits = BaseManager.GetBaseUnits(victimBase.OwnerID);

            Log("Event", $"Player {attacker.Playername} initiated a battle.");

            // Расчёт базовой силы на основе числа юнитов
            var attackerPower = 0.0;
            var victimPower = 0.0;
            foreach (var unit in attackerUnits)
            {
                attackerPower += (double)(unit.Count * ItemsVars.GetPower(unit.Type));
                Log("Battle", $"Player {attacker.Playername} has {unit.Count} units of {unit.Type} class.");
            }
            foreach (var unit in victimUnits)
            {
                victimPower += (double)(unit.Count * ItemsVars.GetPower(unit.Type));
                Log("Battle", $"Player {victim.Playername} has {unit.Count} units of {unit.Type} class.");
            }

            // Множители силы
            var attackerBaseMultiplier = BaseManager.HasBaseStructure(
                BaseManager.GetBaseInfo(attackerAcc).BaseID,
                "aircraftsComplex"
                );
            attackerPower *= attackerBaseMultiplier != null
                ? ItemsVars.GetBaseAttackMultiplier(attackerBaseMultiplier.Level)
                : 1;
            victimPower *= ItemsVars.GetBaseDefenceMultiplier(victimBase.Level);

            var result = "";
            if (attackerPower > victimPower)
            {
                var delta = (attackerPower > 0 && victimPower > 0 ? 1 - (victimPower / attackerPower) : 1.0);
                DoBattle(attacker, victim, delta);
                result = "youwin";
            }
            else
            {
                var delta = (attackerPower > 0 && victimPower > 0 ? 1 - (attackerPower / victimPower) : 1.0);
                DoBattle(victim, attacker, delta);
                result = "youlose";
            }
            Log("Event", $"Player {attacker.Playername} (units power is {attackerPower}) attacked {victim.Playername} " +
                $"(units power is {victimPower}) and {(attackerPower > victimPower ? "wins" : "loses")} that battle.");

            return result;
        }

        /// <summary>
        /// Реализует механизм обработки последствий битвы (занесение очков в стату, обнуление юнитов у проигравшего и вычитание части у победителя)
        /// </summary>
        /// <param name="winner"></param>
        /// <param name="winnerUnits"></param>
        /// <param name="loser"></param>
        /// <param name="loserUnits"></param>
        /// <param name="delta"></param>
        private static void DoBattle(Player winner, Player loser, double delta)
        {
            using (var db = new Entities())
            {
                db.Players.FirstOrDefault(o => o.UserID == winner.UserID).Wins++;

                var units = db.Units.Where(o => o.Instance == "bas" + winner.UserID && o.Count > 0);
                foreach (var unit in units)
                {
                    unit.Count = (int)Math.Round(unit.Count * delta);
                }

                db.Players.FirstOrDefault(o => o.UserID == loser.UserID).Loses++;

                units = db.Units.Where(o => o.Instance == "bas" + loser.UserID && o.Count > 0);
                foreach (var unit in units)
                {
                    db.Entry(unit).State = EntityState.Deleted;
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        ///  Реализует механизм покупки
        /// </summary>
        /// <param name="baseid"></param>
        /// <param name="itemName"></param>
        /// <param name="level"></param>
        public static void DoBuyItem(int baseid, string itemName, int level = 0)
        {
            level++;
            var cost = ItemsVars.GetCost(itemName);
            using (var db = new Entities())
            {
                var resources = db.Resources.FirstOrDefault(o => o.Instance == "bas" + baseid.ToString());
                resources.Credits -= cost.Credits * level;
                resources.Energy -= cost.Energy * level;
                resources.Neutrino -= cost.Neutrino * level;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Реализует механизм логгирования
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        public async static void Log(string type, string text)
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            try
            {
                using (var fstream = new FileStream($"{dir}/../csharpgame.log", FileMode.OpenOrCreate))
                {
                    var array = System.Text.Encoding.Default.GetBytes($"{DateTime.Now.ToString("h:mm:ss tt")} [{type}] {text}{Environment.NewLine}");
                    fstream.Seek(0, SeekOrigin.End);
                    await fstream.WriteAsync(array, 0, array.Length);
                }
            }
            catch
            {
                Log(type, text);
            }
            // System.Diagnostics.Debug.WriteLine
        }
    }
}
