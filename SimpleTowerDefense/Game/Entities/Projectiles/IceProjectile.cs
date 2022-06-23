
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Engine;

namespace SimpleTowerDefense
{
    internal class IceProjectile : Projectile
    {
        private const int FREEZING_SHOTS_CHANCE = 25;

        private const float FREEZE_TIME_MIN = 2f;
        private const float FREEZE_TIME_MAX = 3f;

        private static Texture2D texture;

        public bool IsFreezingShot { get; }

        public float FreezeTime { get; }

        public IceProjectile(ContentManager content, Point position, Point target)
            : base(position, target, new Vector2(10f, 10f))
        {
            if (texture == null)
            {
                texture = content.Load<Texture2D>("Textures/Defenses/IceTurretProjectile");
            }

            IsFreezingShot = Utility.GetRandom(FREEZING_SHOTS_CHANCE, 100 + 1) <= FREEZING_SHOTS_CHANCE;

            if (IsFreezingShot)
            {
                FreezeTime = Utility.GetRandom(FREEZE_TIME_MIN, FREEZE_TIME_MAX);
            }

            destRec = new Rectangle(position, new Point(texture.Width / 6, texture.Height / 6));
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, destRec, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}
