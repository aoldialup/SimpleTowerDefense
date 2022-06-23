
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SimpleTowerDefense
{
    class GrassEntity : Entity
    {
        private static Texture2D grassTexture;
        private Rectangle destRec;

        public GrassEntity(ContentManager content, Point position)
            : base(EntityType.GRASS_TILE)
        {
            if (grassTexture == null)
            {
                grassTexture = content.Load<Texture2D>("Textures/Grass");
            }

            destRec = Game1.CreateEntityRectangle(position);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(grassTexture, destRec, Color.White);
        }

        public override Point GetPosition()
        {
            return destRec.Location;
        }
    }
}
