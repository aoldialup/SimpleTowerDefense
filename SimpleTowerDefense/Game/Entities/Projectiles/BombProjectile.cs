
using Animation2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SimpleTowerDefense
{
    internal class BombProjectile : Projectile
    {
        private static Texture2D texture;
        private Animation explosionAnimation;

        public bool ShouldBeRemoved { get; private set; }

        public bool IsExploding { get; private set; }

        public BombProjectile(ContentManager content, Point position, Point target)
            : base(position, target, new Vector2(15f, 15f))
        {
            if (texture == null)
            {
                texture = content.Load<Texture2D>("Textures/Defenses/Bomb");
            }

            destRec = Game1.CreateEntityRectangle(position);
            explosionAnimation = new Animation(content.Load<Texture2D>("Textures/Defenses/BombExplosion"),
                5, 5, 25, 0, Animation.NO_IDLE, 1, 1, target.ToVector2() + new Vector2(0f, -30f), 1.5f, false);
        }

        public override void Update(Game1 game, GameTime gameTime)
        {
            explosionAnimation.Update(gameTime);

            if (IsExploding && !explosionAnimation.isAnimating)
            {
                ShouldBeRemoved = true;
            }

            base.Update(game, gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (IsExploding)
            {
                explosionAnimation.Draw(sb, Color.White, Animation.FLIP_NONE);
            }
            else
            {
                sb.Draw(texture, destRec, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }

        public void Explode()
        {
            explosionAnimation.isAnimating = true;
            IsExploding = true;
        }
    }
}
