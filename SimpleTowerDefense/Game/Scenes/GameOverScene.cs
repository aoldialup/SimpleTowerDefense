
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleTowerDefense
{
    internal class GameOverScene : Scene
    {
        private Button restartButton;
        private Button gameOverText;

        private bool isFirstUpdate = true;

        public GameOverScene()
            : base("GameOver")
        {

        }

        public override void Init()
        {
            gameOverText = new Button(Game.Content, new Point(300, 100),
            new Point(100, 100), Game1.Font, string.Empty);

            restartButton = new Button(Game.Content, new Point(gameOverText.Left, gameOverText.Bottom + 20),
            new Point(100, 50), Game1.Font, "Restart");
        }

        public override void Update(GameTime gameTime)
        {
            restartButton.Update(Game.MousePosition);

            if (isFirstUpdate)
            {
                string winString = Game.GetPlayingScene().GameSaveData.IsWin ? "won" : "lost";
                gameOverText.DisplayedString = $"Game Over!\nYou {winString}";

                isFirstUpdate = false;
            }

            if (restartButton.IsPressed())
            {
                Game.GetPlayingScene().Reset();
                SceneToExitTo = "Title";
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            Game.GetPlayingScene().Draw(sb);

            Game.DrawShadowSprite(sb);
            gameOverText.Draw(sb);
            restartButton.Draw(sb);

            Game.ProfileManager.DisplayProfileStats(sb, new Point(5, 100));
        }
    }
}
