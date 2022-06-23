
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SimpleTowerDefense
{
    class BrickEntity : Entity
    {
        private static Texture2D texture = null;

        private Rectangle destRec;

        public BrickEntity(ContentManager content, Point position)
            : base(EntityType.BRICK)
        {
            if (texture == null)
            {
                texture = content.Load<Texture2D>("Textures/Brick");
            }

            destRec = Game1.CreateEntityRectangle(position);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, destRec, Color.White);
        }

        public override Point GetPosition()
        {
            return destRec.Location;
        }
    }
}
