
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleTowerDefense
{
    internal class Tower : Entity
    {
        private const int MAX_HEALTH = 50;

        private static Texture2D texture;
        private Rectangle destRec;

        private static int health = MAX_HEALTH;

        public static bool IsDead
        {
            get
            {
                return health == 0;
            }
        }

        public Tower(ContentManager content, Point position)
            : base(EntityType.TOWER)
        {
            if (texture == null)
            {
                texture = content.Load<Texture2D>("Textures/Target");
            }

            destRec = Game1.CreateEntityRectangle(position);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, destRec, Color.White);
        }

        public void TakeDamage(int amount)
        {
            health -= amount;

            if (health < 0)
            {
                health = 0;
            }
        }

        public static void ResetHealth()
        {
            health = MAX_HEALTH;
        }

        public override Point GetPosition()
        {
            return destRec.Location;
        }

        public bool Intersects(Rectangle other)
        {
            return destRec.Intersects(other);
        }

        public static int GetHealth()
        {
            return health;
        }
    }
}
