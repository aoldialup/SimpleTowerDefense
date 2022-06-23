
using System;
using System.Linq;

namespace SimpleTowerDefense
{
    internal class GameSaveData
    {
        public bool IsWin { get; set; }

        public int DefensesPurchased { get; set; }

        public int DamageDealt { get; set; }

        public int[] ZombiesKilledOfEachType { get; set; }

        public int WavesCompleted { get; set; }

        public int TotalZombiesKilled
        {
            get
            {
                return ZombiesKilledOfEachType.Sum();
            }
        }

        public GameSaveData()
        {
            ZombiesKilledOfEachType = new int[Zombie.TOTAL_TYPES];
        }

        public void Reset()
        {
            IsWin = false;
            DefensesPurchased = 0;
            DamageDealt = 0;
            Array.Clear(ZombiesKilledOfEachType, 0, ZombiesKilledOfEachType.Length);
            WavesCompleted = 0;
        }
    }
}
