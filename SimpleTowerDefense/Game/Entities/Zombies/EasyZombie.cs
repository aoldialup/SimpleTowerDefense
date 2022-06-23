
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Animation2D;
using System;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class EasyZombie : Zombie
    {
        public const string NAME = "Easy Zombie";
        public const string DESCRIPTION = "Easiest zombie in the game. Just make sure not to die to it";

        public EasyZombie(ContentManager content, PathQueue path, int waveNumber)
            : base(EntityType.EASY_ZOMBIE, DamageType.NONE, path, 20, 2, waveNumber, new Vector2(3f, 3f))
        {
            movementAnimations = new Dictionary<MovementDirection, Animation>()
            {
                { MovementDirection.RIGHT, CreateAnimation(content, "Textures/Enemies/EasyZombie/LookRight") },
                { MovementDirection.LEFT, CreateAnimation(content, "Textures/Enemies/EasyZombie/LookLeft") },
                { MovementDirection.UP, CreateAnimation(content, "Textures/Enemies/EasyZombie/LookUp") },
                { MovementDirection.DOWN, CreateAnimation(content, "Textures/Enemies/EasyZombie/LookDown") }
            };

            SetupNextPath();
        }

        protected override Animation CreateAnimation(ContentManager content, string filepath)
        {
            return new Animation(content.Load<Texture2D>(filepath), 3, 1, 3, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 240,
                Vector2.Zero, 1f, true);
        }
    }
}
