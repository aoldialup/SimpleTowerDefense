﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    internal class Text
    {
        public Vector2 Position { get; set; }

        public SpriteFont Font { get; set; }

        public string DisplayedString { get; set; }

        public Color Color { get; set; } = Color.White;

        public Text(Vector2 position, SpriteFont font, string displayedString)
        {
            Position = position;
            Font = font;
            DisplayedString = displayedString;
        }

        public Text(Vector2 position, SpriteFont font)
        {
            Position = position;
            Font = font;

            DisplayedString = string.Empty;
        }

        public Vector2 Measure()
        {
            return Font.MeasureString(DisplayedString);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(Font, DisplayedString, Position, Color);
        }
    }
}
