
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Engine
{
    class Button
    {
        private Text text;

        private Rectangle destRec;

        public bool IsVisible { get; set; } = true;
        public bool IsActive { get; set; } = true;

        public Color HoverColor { get; set; } = Color.Green;

        private Texture2D blankTexture = null;

        private SpriteFont font;

        private Point lastMousePosition;

        public int Left
        {
            get
            {
                return destRec.Left;
            }
        }

        public int Bottom
        {
            get
            {
                return destRec.Bottom;
            }
        }

        public int Top
        {
            get
            {
                return destRec.Top;
            }
        }

        public Button(ContentManager content, Point position, Point size, SpriteFont font, string displayedString)
        {
            this.font = font;
            if (blankTexture == null)
            {
                blankTexture = content.Load<Texture2D>("Textures/UI/BlankTexture");
            }

            destRec = new Rectangle(position, size);

            text = new Text(position.ToVector2(), font);
            text.Color = Color.Black;
            DisplayedString = displayedString;

            Position = position;
        }

        public Color TextColor
        {
            get
            {
                return text.Color;
            }
            set
            {
                text.Color = value;
            }
        }

        public string DisplayedString
        {
            get
            {
                return text.DisplayedString;
            }
            set
            {
                text.DisplayedString = value;

                float x = (destRec.X + (destRec.Width / 2)) - (font.MeasureString(text.DisplayedString).X / 2);
                float y = (destRec.Y + (destRec.Height / 2)) - (font.MeasureString(text.DisplayedString).Y / 2);

                text.Position = new Vector2(x, y);
            }
        }

        public Point Position
        {
            get
            {
                return destRec.Location;
            }
            set
            {
                destRec.Location = value;
            }
        }

        private Vector2 FindTextCenter()
        {
            return destRec.Center.ToVector2() - text.Measure();
        }

        public bool IsMouseOver()
        {
            return destRec.Contains(lastMousePosition);
        }

        public void Update(Point mousePosition)
        {
            lastMousePosition = mousePosition;
        }

        public bool IsPressed()
        {
            return IsMouseOver() && Input.IsMouseButtonPressed(MouseButton.LEFT);
        }

        public void Draw(SpriteBatch sb)
        {
            if (IsVisible)
            {
                if (IsActive && IsMouseOver())
                {
                    HoverColor = Color.Green;
                }

                sb.Draw(blankTexture, destRec, HoverColor);
                text.Draw(sb);

                if (IsActive)
                {
                    HoverColor = Color.White;
                }
            }
        }
    }
}
