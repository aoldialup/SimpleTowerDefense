
using Animation2D;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class BossZombie : Zombie
    {
        public BossZombie(ContentManager content, PathQueue path, int waveNumber)
            : base(EntityType.BOSS_ZOMBIE, DamageType.ICE, path, 5, 10, waveNumber, new Vector2(2f, 2f))
        {
            movementAnimations = new Dictionary<MovementDirection, Animation>()
            {
                { MovementDirection.RIGHT, CreateAnimation(content, "Textures/Enemies/BossZombie/LookRight") },
                { MovementDirection.LEFT, CreateAnimation(content, "Textures/Enemies/BossZombie/LookLeft") },
                { MovementDirection.UP, CreateAnimation(content, "Textures/Enemies/BossZombie/LookUp") },
                { MovementDirection.DOWN, CreateAnimation(content, "Textures/Enemies/BossZombie/LookDown") }
            };

            SetupNextPath();
        }

        protected override Animation CreateAnimation(ContentManager content, string filepath)
        {
            return new Animation(content.Load<Texture2D>(filepath), 4, 1, 4, 0,
                Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 60, Vector2.Zero, 1f, true);
        }
    }
}
