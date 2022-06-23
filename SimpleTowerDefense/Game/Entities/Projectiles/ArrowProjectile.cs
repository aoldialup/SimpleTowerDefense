
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleTowerDefense
{
    internal class ArrowProjectile : Projectile
    {
        private static Texture2D texture;

        public ArrowProjectile(ContentManager content, Point position, Point target)
            : base(position, target, new Vector2(10f, 10f))
        {
            if (texture == null)
            {
                texture = content.Load<Texture2D>("Textures/Defenses/Arrow");
            }

            destRec = new Rectangle(position, new Point(texture.Width / 8, texture.Height / 8));
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, destRec, null, Color.Pink, angle, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}
