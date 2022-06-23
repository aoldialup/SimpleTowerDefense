
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleTowerDefense
{
    internal class HelpScene : Scene
    {
        private Text text;

        private Button exitButton;

        public HelpScene()
            : base("Help")
        {

        }

        public override void Init()
        {
            text = new Text(new Vector2(200, 150), Game1.Font, "If you grew up with Bloons Tower Defense, Simple Tower Defense is a great" +
                "\noption for you. " +
                "Enemies move from the start of the track and try to get past the end.\nIf you do not eliminate them " +
                "before they get past the end, you take damage.\nIf you reach zero HP, you lose.\nIf you manage to defend " +
                "against enemies for 30 rounds, you win the game.\nRounds will get progressively more difficult, and more " +
                "defenses will need to be used.");

            exitButton = new Button(Game.Content, new Point(10, 50), new Point(100, 100), Game1.SmallFont, "Exit");
        }

        public override void Update(GameTime gameTime)
        {
            exitButton.Update(Game.MousePosition);

            if (Input.IsMouseButtonPressed(MouseButton.LEFT) && exitButton.IsMouseOver())
            {
                SceneToExitTo = "Title";
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            text.Draw(sb);
            exitButton.Draw(sb);
        }
    }
}
