
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SimpleTowerDefense
{
    internal class DefenseIcon
    {
        private Rectangle mouseCursorRec;
        private Rectangle iconDestRec;

        private Texture2D texture;

        private Text costText;
        private Text nameText;

        public DefenseInfo DefenseInfo { get; }

        public EntityType Type { get; }


        private static Texture2D noSymbolTexture;
        private Rectangle noSymbolDestRec;

        public Func<Entity, bool> CanBePlacedOnEntity { get; }

        public bool IsPurchasable { get; private set; }

        public DefenseIcon(ContentManager content, EntityType type, DefenseInfo defenseInfo, Vector2 position, Texture2D texture,
            Func<Entity, bool> canDefenseBePlaced)
        {
            Type = type;
            DefenseInfo = defenseInfo;
            CanBePlacedOnEntity = canDefenseBePlaced;
            this.texture = texture;

            iconDestRec = new Rectangle(position.ToPoint(), new Point((int)(texture.Width * 0.2), (int)(texture.Height * 0.2)));
            nameText = new Text(position + new Vector2(0, 30), Game1.Font, defenseInfo.Name);
            costText = new Text(position + new Vector2(0, 52), Game1.Font, $"${defenseInfo.Cost}");

            mouseCursorRec = new Rectangle(Point.Zero, iconDestRec.Size);

            noSymbolTexture = content.Load<Texture2D>("Textures/UI/NoSymbol");
            noSymbolDestRec = new Rectangle(iconDestRec.Center + new Point(-15, -20), new Point(noSymbolTexture.Width / 7, noSymbolTexture.Height / 7));
        }

        public bool Contains(Point position)
        {
            return iconDestRec.Contains(position);
        }

        public void ResetPosition()
        {
            mouseCursorRec.Location = Point.Zero;
        }

        public void UpdateCursorPosition(Point position)
        {
            mouseCursorRec.Location = position;
        }

        public void UpdateNoSymbol(int playerBobux)
        {
            IsPurchasable = playerBobux - DefenseInfo.Cost >= 0;
        }

        public void Draw(SpriteBatch sb, bool drawAsMouseCursor)
        {
            if (drawAsMouseCursor)
            {
                sb.Draw(texture, mouseCursorRec, Color.White);
            }
            else
            {
                sb.Draw(texture, iconDestRec, Color.White);
            }

            costText.Draw(sb);
            nameText.Draw(sb);

            if (!IsPurchasable)
            {
                sb.Draw(noSymbolTexture, noSymbolDestRec, Color.White);
            }
        }
    }
}
