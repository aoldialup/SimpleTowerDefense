
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleTowerDefense;
using System.Collections.Generic;

namespace Engine
{

    class SceneManager
    {
        public Scene currentScene;

        private Dictionary<string, Scene> scenes;

        private Game1 game;

        public SceneManager(Game1 game)
        {
            scenes = new Dictionary<string, Scene>();

            this.game = game;
        }

        public Scene this[string name]
        {
            get
            {
                return scenes[name];
            }
            set
            {
                scenes[name] = value;
            }
        }

        public void AddScene(Scene scene)
        {
            scene.Game = game;
            scenes.Add(scene.Name, scene);
        }

        public void InitScenes()
        {
            foreach (Scene s in scenes.Values)
            {
                s.Init();
            }
        }

        public void SetCurrentScene(string name)
        {
            currentScene = scenes[name];
        }

        public void Update(GameTime gameTime)
        {
            currentScene.Update(gameTime);

            if (currentScene.SceneToExitTo != null)
            {
                string formerScene = currentScene.Name;

                currentScene.Exit();

                currentScene = scenes[currentScene.SceneToExitTo];

                scenes[formerScene].SceneToExitTo = null;

                currentScene.OnEnable();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            currentScene.Draw(sb);
        }
    }
}