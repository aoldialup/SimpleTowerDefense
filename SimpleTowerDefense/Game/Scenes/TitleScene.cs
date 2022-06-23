
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleTowerDefense
{
    internal class TitleScene : Scene
    {
        private Text titleText;

        private Button playButton;
        private Button profilesButton;
        private Button helpButton;
        private Button exitButton;

        public TitleScene()
            : base("Title")
        {

        }

        public override void Init()
        {
            titleText = new Text(new Vector2(300, 25), Game1.Font, "Simple Tower Defense");

            playButton = new Button(Game.Content, new Point(300, 75), new Point(150, 75), Game1.Font, "Play");
            profilesButton = new Button(Game.Content, new Point(300, 175), new Point(150, 75), Game1.Font, "Profiles");
            helpButton = new Button(Game.Content, new Point(300, 275), new Point(150, 75), Game1.Font, "Help");
            exitButton = new Button(Game.Content, new Point(300, 375), new Point(150, 75), Game1.Font, "Exit");
        }

        public override void Update(GameTime gameTime)
        {
            if (!Game.ContentFailedToLoad)
            {
                playButton.Update(Game.MousePosition);
                profilesButton.Update(Game.MousePosition);
                helpButton.Update(Game.MousePosition);
                exitButton.Update(Game.MousePosition);

                if (Input.IsMouseButtonPressed(MouseButton.LEFT))
                {

                    if (playButton.IsMouseOver() || profilesButton.IsMouseOver())
                    {
                        SceneToExitTo = "Profiles";
                    }
                    else if (exitButton.IsMouseOver())
                    {
                        Game.Exit();
                    }
                    else if (helpButton.IsMouseOver())
                    {
                        SceneToExitTo = "Help";
                    }
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Game.ContentFailedToLoad)
            {
                sb.DrawString(Game1.Font, "One or more levels failed to load. Fix the files and restart the game.",
                    new Vector2(150, 250), Color.Red);
            }
            else
            {
                titleText.Draw(sb);
                playButton.Draw(sb);
                profilesButton.Draw(sb);
                helpButton.Draw(sb);
                exitButton.Draw(sb);
            }
        }
    }
}
