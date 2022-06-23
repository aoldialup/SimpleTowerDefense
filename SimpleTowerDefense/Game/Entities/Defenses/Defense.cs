
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal abstract class Defense : Entity
    {
        protected Rectangle destRec;

        protected float range;
        protected float attackRate;
        protected int damage;
        protected DamageType damageType;

        protected List<Projectile> projectiles;

        protected Defense(EntityType type, DamageType damageType, float range, float attackRate, int damage, Rectangle destRec)
            : base(type)
        {
            this.destRec = destRec;
            this.range = range;
            this.attackRate = attackRate;
            this.damage = damage;
            this.damageType = damageType;

            projectiles = new List<Projectile>();
        }

        protected void DrawProjectiles(SpriteBatch sb)
        {
            foreach (Projectile p in projectiles)
            {
                p.Draw(sb);
            }
        }

        protected Zombie GetNearestZombie(List<Zombie> zombies)
        {
            Zombie nearestZombie = null;
            float currentEnemyDistance;
            float shortestDistance = float.MaxValue;

            foreach (Zombie z in zombies)
            {
                currentEnemyDistance = Vector2.Distance(destRec.Location.ToVector2(), z.GetPosition().ToVector2());

                if (currentEnemyDistance < shortestDistance)
                {
                    shortestDistance = currentEnemyDistance;
                    nearestZombie = z;
                }
            }

            if (nearestZombie != null && shortestDistance <= range && nearestZombie.ImmunityType != damageType)
            {
                return nearestZombie;
            }

            return null;
        }

        public virtual void ClearProjectiles() { }

        public virtual void UpdateProjectiles(Game1 game, GameTime gameTime, List<Zombie> zombies) { }

        public virtual void ShootAtNearestZombie(Game1 game, GameTime gameTime, List<Zombie> zombies) { }
    }
}
