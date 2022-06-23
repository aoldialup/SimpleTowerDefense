
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class ArrowTurret : Defense
    {
        public static readonly DefenseInfo info = new DefenseInfo("Arrow Turret", 300);

        private static Texture2D texture;

        private float timeUntilNextShot = 0f;

        public ArrowTurret(ContentManager content, Point position)
            : base(EntityType.ARROW_TURRET, DamageType.ARROW, 500f, 0.5f, 10, Game1.CreateEntityRectangle(position))
        {
            if (texture == null)
            {
                texture = content.Load<Texture2D>("Textures/Defenses/ArcherTurret");
            }
        }

        public override void UpdateProjectiles(Game1 game, GameTime gameTime, List<Zombie> zombies)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].IsOutOfBounds())
                {
                    projectiles.RemoveAt(i);
                }
                else
                {
                    bool hitZombie = false;

                    for (int j = 0; j < zombies.Count; j++)
                    {
                        if (projectiles[i].IntersectsWithZombie(zombies[j]))
                        {
                            if (zombies[j].TakeDamage(damage, damageType))
                            {
                                if (game.ProfileManager.AreProfilesEnabled)
                                {
                                    game.GetPlayingScene().GameSaveData.DamageDealt += damage;
                                }
                            }

                            projectiles.RemoveAt(i);

                            hitZombie = true;
                            break;
                        }
                    }

                    if (!hitZombie)
                    {
                        projectiles[i].Update(game, gameTime);
                    }
                }
            }
        }

        public override void ClearProjectiles()
        {
            projectiles.Clear();
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
                    ArrowProjectile projectile = new ArrowProjectile(game.Content, destRec.Center, nearestZombie.GetCenter());
                    projectiles.Add(projectile);

                    timeUntilNextShot = attackRate;
                }
                else
                {
                    timeUntilNextShot -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }
    }
}
