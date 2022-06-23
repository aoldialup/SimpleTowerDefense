
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    internal class Sprite
    {
        public Rectangle DestRec { get; set; }

        public Texture2D Texture { get; set; }

        public Color Color { get; set; }

        public Sprite(ContentManager content, string path)
        {
            Texture = content.Load<Texture2D>(path);

            Color = Color.White;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, DestRec, Color);
        }
    }
}
