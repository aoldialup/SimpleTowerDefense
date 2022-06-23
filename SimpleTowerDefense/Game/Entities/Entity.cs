
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleTowerDefense
{
    enum EntityType
    {
        NONE = -1,
        EASY_ZOMBIE,
        MEDIUM_ZOMBIE,
        HARD_ZOMBIE,
        BOSS_ZOMBIE,

        GRASS_TILE,
        BRICK,
        TOWER,

        ARROW_TURRET,
        BOMB_TURRET,
        GRENADE,
        ICE_TURRET,

        PROJECTILE
    }

    internal abstract class Entity
    {
        public EntityType Type { get; }

        protected Entity(EntityType type)
        {
            Type = type;
        }

        public virtual void Update(Game1 game, GameTime gameTime)
        {

        }

        public abstract Point GetPosition();
        public abstract void Draw(SpriteBatch sb);
    }
}