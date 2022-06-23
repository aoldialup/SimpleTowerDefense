
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class BombTurret : Defense
    {
        public static readonly DefenseInfo info = new DefenseInfo("Bomb Turret", 450);

        private static Texture2D texture;
        private float timeUntilNextShot = 0f;

        public BombTurret(ContentManager content, Point position)
            : base(EntityType.BOMB_TURRET, DamageType.BOMB, 200f, 0.4f, 5, Game1.CreateEntityRectangle(position))
        {
            if (texture == null)
            {
                texture = content.Load<Texture2D>("Textures/Defenses/BombTurret");
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, destRec, Color.White);
            DrawProjectiles(sb);
        }

        public override Point GetPosition()
        {
            return destRec.Location;
        }

        public override void ShootAtNearestZombie(Game1 game, GameTime gameTime, List<Zombie> zombies)
        {
            Zombie nearestZombie = GetNearestZombie(zombies);

            if (nearestZombie != null)
            {
                if (timeUntilNextShot <= 0f)
                {
                    BombProjectile projectile = new BombProjectile(game.Content, destRec.Center, nearestZombie.GetCenter());
                    projectiles.Add(projectile);

                    timeUntilNextShot = attackRate;
                }
                else
                {
                    timeUntilNextShot -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        public override void UpdateProjectiles(Game1 game, GameTime gameTime, List<Zombie> zombies)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].IsOutOfBounds() || ((BombProjectile)projectiles[i]).ShouldBeRemoved)
                {
                    projectiles.RemoveAt(i);
                }
                else
                {
                    for (int j = 0; j < zombies.Count; j++)
                    {
                        if (projectiles[i].IntersectsWithZombie(zombies[j]))
                        {
                            float distance;
                            float multiplier;

                            foreach (Zombie z in zombies)
                            {
                                distance = ((BombProjectile)projectiles[i]).DistanceToZombie(z);

                                if (distance <= range)
                                {
                                    multiplier = 1 - distance / range;
                                    z.TakeDamage((int)(multiplier * damage), damageType);
                                }
                            }

                                ((BombProjectile)projectiles[i]).Explode();
                            break;
                        }
                    }

                    projectiles[i].Update(game, gameTime);
                }
            }
        }
    }
}
