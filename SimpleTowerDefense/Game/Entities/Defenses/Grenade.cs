
using Animation2D;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class Grenade : Defense
    {
        private const float TIME_UNTIL_EXPLOSION = 3.7f;

        public static readonly DefenseInfo defenseInfo = new DefenseInfo("Grenade", 300);

        private static Texture2D texture = null;

        private float timeUntilExplosion = TIME_UNTIL_EXPLOSION;

        private Animation explosionAnimation;

        private List<Zombie> zombies;

        private Text timeUntilExplosionText;

        public bool ShouldBeRemoved { get; private set; }

        private bool hasExploded;

        public Grenade(ContentManager content, Point position, List<Zombie> zombies)
            : base(EntityType.GRENADE, DamageType.GRENADE, 30f, -1f, 15, Game1.CreateEntityRectangle(position))
        {
            this.zombies = zombies;

            if (texture == null)
            {
                texture = content.Load<Texture2D>("Textures/Defenses/Grenade");
            }

            timeUntilExplosionText = new Text(destRec.Center.ToVector2(), Game1.Font, timeUntilExplosion.ToString());
            explosionAnimation = new Animation(content.Load<Texture2D>("Textures/Defenses/BombExplosion"),
                5, 5, 25, 0, Animation.NO_IDLE, 1, 3, position.ToVector2(), 1.5f, false);

        }

        public override void Update(Game1 game, GameTime gameTime)
        {
            explosionAnimation.Update(gameTime);

            if (!hasExploded)
            {
                if (timeUntilExplosion <= 0f)
                {
                    Explode(game);
                }
                else
                {
                    timeUntilExplosion -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    timeUntilExplosionText.DisplayedString = ((int)timeUntilExplosion).ToString();
                }
            }
            else
            {
                if (!explosionAnimation.isAnimating)
                {
                    ShouldBeRemoved = true;
                }
            }
        }

        public override Point GetPosition()
        {
            return destRec.Location;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!hasExploded)
            {
                sb.Draw(texture, destRec, Color.White);
                timeUntilExplosionText.Draw(sb);
            }
            else
            {
                explosionAnimation.Draw(sb, Color.White, Animation.FLIP_NONE);
            }
        }

        private void Explode(Game1 game)
        {
            float distance;
            float multiplier;
            int damageToTake;

            foreach (Zombie z in zombies)
            {
                distance = (float)Math.Sqrt(destRec.X - z.GetPosition().X + destRec.Y - z.GetPosition().Y);

                if (distance <= range)
                {
                    multiplier = 1 - distance / range;

                    damageToTake = (int)(multiplier * damage);

                    if (z.TakeDamage(damageToTake, damageType))
                    {
                        if (game.ProfileManager.AreProfilesEnabled)
                        {
                            game.GetPlayingScene().GameSaveData.DamageDealt += damageToTake;
                        }
                    }
                }
            }

            explosionAnimation.isAnimating = true;
            hasExploded = true;
        }
    }
}
