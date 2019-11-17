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
        public static dynamic GetCost(string ItemType) // восхитительный полукостыль
        {
            switch (ItemType)
            {
                case "lifeComplex": return new { Credits = 300, Energy = 25, Neutrino = 0.0 };
                case "resourceComplex": return new { Credits = 100, Energy = 25, Neutrino = 0.0 };
                case "energyComplex": return new { Credits = 100, Energy = 25, Neutrino = 0.0 };
                case "aircraftsComplex": return new { Credits = 1000, Energy = 250, Neutrino = 0.0 };
                case "researchStation": return new { Credits = 100000, Energy = 250000, Neutrino = 0.0 };
                /**/
                case "droneUnit": return new { Credits = 100, Energy = 25, Neutrino = 0.0 };
                case "jetUnit": return new { Credits = 1000, Energy = 250, Neutrino = 0.0 };
                case "lincorUnit": return new { Credits = 10000, Energy = 2500, Neutrino = 0.0 };
                case "someGiantShitUnit": return new { Credits = 1000000, Energy = 250000, Neutrino = 1.0 };
                /**/
                case "repairBase": return new { Credits = 200, Energy = 200, Neutrino = 0.0 };
                case "upgradeBase": return new { Credits = 2000, Energy = 2000, Neutrino = 0.0 };
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
    }
}