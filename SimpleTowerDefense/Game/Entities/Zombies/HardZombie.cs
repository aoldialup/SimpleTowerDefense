using Animation2D;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class HardZombie : Zombie
    {
        public HardZombie(ContentManager content, PathQueue path, int waveNumber)
            : base(EntityType.HARD_ZOMBIE, DamageType.ARROW, path, 5, 8, waveNumber, new Vector2(2f, 2f))
        {
            movementAnimations = new Dictionary<MovementDirection, Animation>()
            {
                { MovementDirection.RIGHT, CreateAnimation(content, "Textures/Enemies/HardZombie/LookRight") },
                { MovementDirection.LEFT, CreateAnimation(content, "Textures/Enemies/HardZombie/LookLeft") },
                { MovementDirection.UP, CreateAnimation(content, "Textures/Enemies/HardZombie/LookUp") },
                { MovementDirection.DOWN, CreateAnimation(content, "Textures/Enemies/HardZombie/LookDown") }
            };

            SetupNextPath();
        }

        protected override Animation CreateAnimation(ContentManager content, string filepath)
        {
            return new Animation(content.Load<Texture2D>(filepath), 3, 1, 3, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 240,
                Vector2.Zero, 1.2f, true);
        }
    }
}
