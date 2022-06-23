
using Microsoft.Xna.Framework;
using Animation2D;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SimpleTowerDefense
{
    internal abstract class Zombie : Entity
    {
        public const int EASY_INDEX = 0;
        public const int MEDIUM_INDEX = 1;
        public const int HARD_INDEX = 2;
        public const int BOSS_INDEX = 3;

                private const float DAMAGE_MULTIPLIER = 0.010f;

                public const int TOTAL_TYPES = 4;

                protected MovementDirection currentDirection;
        protected Dictionary<MovementDirection, Animation> movementAnimations;
        protected Animation currentAnimation;

                protected PathQueue pathQueue;
        protected Point currentPath;
        protected Vector2 speed;

                protected float timeFrozen = 0f;
        protected int damageToTower;

                public DamageType ImmunityType { get; }

                private int health;

                public bool IsDead
        {
            get
            {
                                return health <= 0;
            }
        }

                public bool IsMoving
        {
            get
            {
                return timeFrozen <= 0f;
            }
        }

                protected Zombie(EntityType type, DamageType immunityType, PathQueue pathQueue, int health, int damageToTower, int waveNumber, Vector2 speed)
            : base(type)
        {
                        this.speed = speed;
            this.damageToTower = damageToTower;
            this.pathQueue = pathQueue;

                        this.damageToTower = (int)(damageToTower * (1 + waveNumber * DAMAGE_MULTIPLIER));

                        ImmunityType = immunityType;
            this.health = health;
        }

                                public void SetupNextPath()
        {
                        if (!pathQueue.IsEmpty)
            {
                                currentPath = pathQueue.Dequeue();

                                MovementDirection newDirection;

                                if (currentAnimation == null)
                {
                                        newDirection = GetMovementDirection(Game1.ZombieSpawnPoint);
                }
                else
                {
                                        newDirection = GetMovementDirection(currentAnimation.destRec.Location);

                                        movementAnimations[newDirection].destRec.Location = movementAnimations[currentDirection].destRec.Location;
                }

                                currentDirection = newDirection;

                                currentAnimation = movementAnimations[newDirection];
            }
        }

                                public void Freeze(float seconds)
        {
                        if (ImmunityType != DamageType.ICE)
            {
                                timeFrozen = seconds;
            }
        }

                                private bool HasNoImmunity()
        {
                        return ImmunityType == DamageType.NONE;
        }

                                public virtual bool TakeDamage(int damage, DamageType damageType)
        {
                        if (HasNoImmunity() || ImmunityType != damageType)
            {
                                health -= damage;
                return true;
            }

                        return false;
        }

                                public override void Update(Game1 game, GameTime gameTime)
        {
                        if (IsMoving)
            {
                                currentAnimation.Update(gameTime);

                                if (currentAnimation.destRec.Location != currentPath)
                {
                                        switch (currentDirection)
                    {
                        case MovementDirection.RIGHT:
                        currentAnimation.destRec.X += (int)Math.Min(speed.Y, currentPath.X - currentAnimation.destRec.X);
                        break;

                        case MovementDirection.LEFT:
                        currentAnimation.destRec.X += (int)Math.Max(-speed.X, currentPath.X - currentAnimation.destRec.X);
                        break;

                        case MovementDirection.UP:
                        currentAnimation.destRec.Y += (int)Math.Max(-speed.Y, currentPath.Y - currentAnimation.destRec.Y);
                        break;

                        case MovementDirection.DOWN:
                        currentAnimation.destRec.Y += (int)Math.Min(speed.Y, currentPath.Y - currentAnimation.destRec.Y);
                        break;
                    }
                }
                else
                {
                                        if (!game.GetPlayingScene().GetTower().Intersects(currentAnimation.destRec) && !IsDead)
                    {
                                                SetupNextPath();
                    }
                }
            }
            else
            {
                                timeFrozen -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }


        }

                                public bool IntersectsWith(Rectangle other)
        {
                        return currentAnimation.destRec.Intersects(other);
        }

                                public Point GetCenter()
        {
                        return currentAnimation.destRec.Center;
        }

                                protected MovementDirection GetMovementDirection(Point position)
        {
                        if (position.Y == currentPath.Y && position.X < currentPath.X)
            {
                                return MovementDirection.RIGHT;
            }

                        if (position.Y == currentPath.Y && position.X > currentPath.X)
            {
                                return MovementDirection.LEFT;
            }

                        if (position.X == currentPath.X && position.Y < currentPath.Y)
            {
                                return MovementDirection.DOWN;
            }

                        if (position.X == currentPath.X && position.Y > currentPath.Y)
            {
                                return MovementDirection.UP;
            }

                        return MovementDirection.NONE;
        }

                                public override void Draw(SpriteBatch sb)
        {
                        currentAnimation.Draw(sb, Color.White, Animation.FLIP_NONE);
        }

                                public override Point GetPosition()
        {
                        return currentAnimation.destRec.Location;
        }

                                public void DamageTower(Tower tower)
        {
                        tower.TakeDamage(damageToTower);
        }

                                public bool IntersectsWithTower(Tower tower)
        {
                        return tower.Intersects(currentAnimation.destRec);
        }

                                protected abstract Animation CreateAnimation(ContentManager content, string filepath);

                                public static int GetIndex(EntityType zombieType)
        {
                        switch (zombieType)
            {
                case EntityType.EASY_ZOMBIE:
                return EASY_INDEX;

                case EntityType.MEDIUM_ZOMBIE:
                return MEDIUM_INDEX;

                case EntityType.HARD_ZOMBIE:
                return HARD_INDEX;

                case EntityType.BOSS_ZOMBIE:
                return BOSS_INDEX;

                default:
                throw new ArgumentException("Not a valid type");
            }
        }
    }
}
