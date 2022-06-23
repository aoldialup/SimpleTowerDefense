
using System;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class ZombieStack
    {
        private List<Zombie> enemies;

        public int Count
        {
            get
            {
                return enemies.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        public ZombieStack()
        {
            enemies = new List<Zombie>();
        }

        public ZombieStack(ZombieStack other)
        {
            enemies = new List<Zombie>(other.enemies);
        }

        public void Push(Zombie e)
        {
            enemies.Add(e);
        }

        public Zombie Pop()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Stack is empty");
            }

            Zombie e = enemies[enemies.Count - 1];

            enemies.RemoveAt(enemies.Count - 1);

            return e;
        }

        public Zombie Top()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Stack is empty");
            }

            return enemies[enemies.Count - 1];
        }
    }
}
