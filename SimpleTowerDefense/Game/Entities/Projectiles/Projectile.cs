
using Microsoft.Xna.Framework;
using System;

namespace SimpleTowerDefense
{
    internal abstract class Projectile : Entity
    {
        protected Rectangle destRec;
        protected Vector2 speed;
        protected Vector2 direction;
        protected Point target;
        protected float angle;

        protected Projectile(Point position, Point target, Vector2 speed)
            : base(EntityType.PROJECTILE)
        {
            this.speed = speed;
            this.target = target;

            direction = (target - position).ToVector2();
            direction.Normalize();

            angle = (float)Math.Atan2(target.Y - position.Y, target.X - position.X);
        }

        public override void Update(Game1 game, GameTime gameTime)
        {
            destRec.Location += (direction * speed).ToPoint();
        }

        public bool IntersectsWithZombie(Zombie z)
        {
            return z.IntersectsWith(destRec);
        }

        public float DistanceToZombie(Zombie z)
        {
            return (float)Math.Sqrt(destRec.X - z.GetPosition().X + destRec.Y - z.GetPosition().Y);
        }

        public override Point GetPosition()
        {
            return destRec.Location;
        }

        public bool IsOutOfBounds()
        {
            return
                destRec.X < 0 || destRec.X > Game1.SCREEN_WIDTH ||
                destRec.Y < 0 || destRec.Y > Game1.SCREEN_HEIGHT;
        }
    }
}
