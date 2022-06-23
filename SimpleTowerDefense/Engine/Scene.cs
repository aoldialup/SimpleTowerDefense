
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleTowerDefense;

namespace Engine
{
    abstract class Scene
    {
        public string Name { get; }

        public string SceneToExitTo { get; set; }

        public Game1 Game { get; set; }

        protected Scene(string name)
        {
            Name = name;
        }

        public abstract void Init();

        public virtual void OnEnable() { }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch sb);

        public virtual void Exit() { }
    }
}