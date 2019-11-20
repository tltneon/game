namespace gamelogic
{
    /// <summary>
    /// Здесь будут хранятся константы игровых единиц (юниты, строения, прочее)
    /// </summary>
    internal class ItemsVars
    {
        /// <summary>
        /// Возвращает базовую стоимость игровой единицы
        /// </summary>
        /// <param name="ItemType"></param>
        /// <returns></returns>
        public static dynamic GetCost(string ItemType)
        {
            switch (ItemType)
            {
                case "lifeComplex": return new { Credits = 45, Energy = 25, Neutrino = 0.0 };
                case "resourceComplex": return new { Credits = 23, Energy = 5, Neutrino = 0.0 };
                case "energyComplex": return new { Credits = 17, Energy = 10, Neutrino = 0.0 };
                case "aircraftsComplex": return new { Credits = 75, Energy = 40, Neutrino = 0.0 };
                case "researchStation": return new { Credits = 10000, Energy = 25000, Neutrino = 0.0 };
                /**/
                case "droneUnit": return new { Credits = 15, Energy = 25, Neutrino = 0.0 };
                case "jetUnit": return new { Credits = 1000, Energy = 250, Neutrino = 0.0 };
                case "lincorUnit": return new { Credits = 10000, Energy = 2500, Neutrino = 0.0 };
                case "someGiantShitUnit": return new { Credits = 1000000, Energy = 250000, Neutrino = 1.0 };
                /**/
                case "repairBase": return new { Credits = 20, Energy = 12, Neutrino = 0.0 };
                case "upgradeBase": return new { Credits = 45, Energy = 70, Neutrino = 0.0 };
            }
            return new { Credits = 0, Energy = 0, Neutrino = 0.0 };
        }

        /// <summary>
        /// Возвращает показатель силы юнита
        /// </summary>
        /// <param name="ItemType"></param>
        /// <returns></returns>
        public static int GetPower(string ItemType)
        {
            switch (ItemType)
            {
                case "droneUnit": return 10;
                case "jetUnit": return 100;
                case "lincorUnit": return 1500;
                case "someGiantShitUnit": return 200000;
            }
            return 1;
        }

        /// <summary>
        /// Возвращает текущее значение максимальной популяции на базе
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetPopulation(int level = 1) => level * 7;

        /// <summary>
        /// Возвращает количество добываемых кредитов за уровень здания
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetCreditProducingAmount(int level = 1) => level * 10;

        /// <summary>
        /// Возвращает количество добываемой энергии за уровень здания
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetEnergyProducingAmount(int level = 1) => level * 10;

        /// <summary>
        /// Возвращает количество добываемого нейтрино за уровень здания
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static double GetNeutrinoProducingAmount(int level = 1) => level * 0.000001;

        /// <summary>
        /// Возвращает множитель силы защиты отряда
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static double GetBaseDefenceMultiplier(int level = 1) => 1 + level * 0.16;

        /// <summary>
        /// Возвращает множитель силы атаки отряда
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static double GetBaseAttackMultiplier(int level = 1) => 1 + level * 0.13;
    }
}