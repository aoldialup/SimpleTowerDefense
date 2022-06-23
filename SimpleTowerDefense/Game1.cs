

using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleTowerDefense
{
    class Game1 : Game
    {
        public const int WAVE_MIN_ENTITIES = (TILES_HIGH * TILES_WIDE) + 1;

        public const int TILES_HIGH = 8;
        public const int TILES_WIDE = 12;
        public const int TILE_SIZE = 50;

        public const int SCREEN_WIDTH = 1000;
        public const int SCREEN_HEIGHT = 1000;

        public const int ACTUAL_SCREEN_WIDTH = 800;
        public const int ACTUAL_SCREEN_HEIGHT = 480;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Point MousePosition { get; private set; } = Point.Zero;

        public static SpriteFont Font { get; private set; }
        public static SpriteFont SmallFont { get; private set; }

        public ProfileManager ProfileManager { get; set; }
        private SceneManager sceneManager;
        public TextManager TextManager { get; set; }

        public bool ContentFailedToLoad { get; set; }

        private Sprite shadowSprite;

        public static Point ZombieSpawnPoint
        {
            get
            {
                return Point.Zero;
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Font = Content.Load<SpriteFont>("Fonts/RegularFont");
            SmallFont = Content.Load<SpriteFont>("Fonts/SmallFont");

            sceneManager = new SceneManager(this);
            sceneManager.AddScene(new PlayingScene());
            sceneManager.AddScene(new TitleScene());

            if (!ContentFailedToLoad)
            {
                shadowSprite = new Sprite(Content, "Textures/UI/GameOverShadow");
                shadowSprite.DestRec = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
                shadowSprite.Color = Color.White * 0.5f;

                sceneManager.AddScene(new ProfilesScene());
                sceneManager.AddScene(new GameOverScene());
                sceneManager.AddScene(new HelpScene());

                TextManager = new TextManager();
                ProfileManager = new ProfileManager(Content);

                sceneManager.InitScenes();
            }

            sceneManager.SetCurrentScene("Title");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            MousePosition = Input.GetMousePosition();

            sceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            sceneManager.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static Point CoordToWorldPosition(int xCoord, int yCoord)
        {
            return new Point(xCoord * TILE_SIZE, yCoord * TILE_SIZE);
        }

        public static Rectangle CreateEntityRectangle(Point position)
        {
            return new Rectangle(position, new Point(TILE_SIZE, TILE_SIZE));
        }

        public static bool IsCoordValid(int row, int col)
        {
            return row >= 0 && row < TILES_HIGH &&
    col >= 0 && col < TILES_WIDE;
        }

        public PlayingScene GetPlayingScene()
        {
            return (PlayingScene)sceneManager["Playing"];
        }


        public void DrawShadowSprite(SpriteBatch sb)
        {
            shadowSprite.Draw(sb);
        }
    }
}