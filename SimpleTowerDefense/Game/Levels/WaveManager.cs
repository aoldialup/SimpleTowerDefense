
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class WaveManager
    {
        public const int WAVES_COUNT = 30;

        private List<Wave> waves;
        private ZombieStack currentZombieStack;

        public int WaveIndex { get; private set; } = -1;

        public WaveManager(Game1 game)
        {
            waves = new List<Wave>(WAVES_COUNT);
            Refill(game);
        }

        public int GetTowerIndex()
        {
            return waves[WaveIndex].TowerIndex;
        }

        public Zombie PopNextZombie()
        {
            return currentZombieStack.Pop();
        }

        public void MoveNextWave()
        {
            WaveIndex++;

            currentZombieStack = waves[WaveIndex].GetZombies();
        }

        private void Refill(Game1 game)
        {
            waves.Clear();

            for (int i = 0; i < WAVES_COUNT; i++)
            {
                Wave wave = new Wave(game, $"Waves/{i + 1}.txt", i + 1);

                if (wave.IsValid)
                {
                    waves.Add(wave);
                }
                else
                {
                    game.ContentFailedToLoad = true;
                    break;
                }
            }
        }

        public void Reset(Game1 game)
        {
            Refill(game);
            WaveIndex = -1;
        }

        public bool IsWaveOver()
        {
            return currentZombieStack.IsEmpty;
        }

        public List<Entity> GetWaveLayout()
        {
            return waves[WaveIndex].GetLayout();
        }

        public int GetEnemyCount()
        {
            return currentZombieStack.Count;
        }
    }
}
