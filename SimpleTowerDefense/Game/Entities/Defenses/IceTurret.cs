
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class IceTurret : Defense
    {
        public static readonly DefenseInfo info = new DefenseInfo("Ice Turret", 150);

        private float timeUntilNextShot = 0f;

        private static Texture2D texture = null;

        private static Vector2 imageCenter;

        public IceTurret(ContentManager content, Point position)
            : base(EntityType.ICE_TURRET, DamageType.ICE, 300f, 0.1f, 5, Game1.CreateEntityRectangle(position))
        {
            if (texture == null)
            {
                texture = content.Load<Texture2D>("Textures/Defenses/IceTurret");
                imageCenter = new Vector2(texture.Width / 2, texture.Height / 2);
            }
        }

        public override void ShootAtNearestZombie(Game1 game, GameTime gameTime, List<Zombie> zombies)
        {
            Zombie nearestZombie = GetNearestZombie(zombies);

            if (nearestZombie != null)
            {
                if (timeUntilNextShot <= 0f)
                {
                    IceProjectile projectile = new IceProjectile(game.Content, destRec.Center, nearestZombie.GetCenter()
    + new Point(5, 5));
                    projectiles.Add(projectile);

                    timeUntilNextShot = attackRate;


                }
                else
                {
                    timeUntilNextShot -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                timeUntilNextShot -= (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                            if (((IceProjectile)projectiles[i]).IsFreezingShot)
                            {
                                zombies[j].Freeze(((IceProjectile)projectiles[i]).FreezeTime);
                            }

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
    }
}
