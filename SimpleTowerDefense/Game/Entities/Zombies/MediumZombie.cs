using Animation2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class MediumZombie : Zombie
    {
        public MediumZombie(ContentManager content, PathQueue path, int waveNumber)
            : base(EntityType.MEDIUM_ZOMBIE, DamageType.BOMB, path, 45, 7, waveNumber, new Vector2(1.5f, 1.5f))
        {
            movementAnimations = new Dictionary<MovementDirection, Animation>()
            {
                { MovementDirection.UP, CreateAnimation(content, "Textures/Enemies/MediumZombie/LookUp") },
                { MovementDirection.DOWN, CreateAnimation(content, "Textures/Enemies/MediumZombie/LookDown") },
                { MovementDirection.LEFT, CreateAnimation(content, "Textures/Enemies/MediumZombie/LookLeft") },
                { MovementDirection.RIGHT, CreateAnimation(content, "Textures/Enemies/MediumZombie/LookRight")}
            };

            SetupNextPath();
        }

        protected override Animation CreateAnimation(ContentManager content, string filepath)
        {
            return new Animation(content.Load<Texture2D>(filepath), 4, 1, 4, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 240,
                Vector2.Zero, 1.25f, true);
        }
    }
}
