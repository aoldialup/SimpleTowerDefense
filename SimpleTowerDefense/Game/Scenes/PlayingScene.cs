
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTowerDefense
{
    internal class PlayingScene : Scene
    {
        private enum WaveState
        {
            NOT_LOADED,
            LOADED,
            LOADED_AND_PLAYING,
            OVER
        }

        private enum TextType
        {
            TOWER_HEALTH,
            PLAYER_BOBUX,
            DAMAGE_DEALT,
            WAVE_NUMBER,
            ZOMBIES_LEFT_IN_WAVE,
            DEFENSES_OWNED,
            TOTAL_ZOMBIES_KILLED
        }

        private const int STARTING_BOBUX = 450;
        private const int BOBUX_MULTIPLIER_MIN = 2;
        private const int BOBUX_MULTIPLIER_MAX = 3;

        private const float ZOMBIE_SPAWN_TIME_MIN = 0.9f;
        private const float ZOMBIE_SPAWN_TIME_MAX = 1.2f;

        private List<Entity> entities;
        private List<Zombie> zombies;
        private List<Defense> defenses;

        private WaveManager waveManager;
        private DefenseIconManager defenseIconManager;

        private Sprite rightHudSprite;
        private Sprite bottomHudSprite;

        private Sprite bobuxSprite;
        private Sprite heartSprite;

        private Button goButton;
        private Button cancelButton;

        public GameSaveData GameSaveData { get; set; }

        private int zombiesLeftInWave = 0;

        private float timeUntilZombieSpawn = 0f;

        private int playerBobux = STARTING_BOBUX;

        private WaveState currentLevelState = WaveState.LOADED;

        public int TowerIndex { get; set; }

        private bool isLastRound = false;

        public PlayingScene()
            : base("Playing")
        {

        }

        public override void Init()
        {
            waveManager = new WaveManager(Game);

            if (!Game.ContentFailedToLoad)
            {
                entities = new List<Entity>(Game1.WAVE_MIN_ENTITIES);
                zombies = new List<Zombie>();
                defenses = new List<Defense>();

                GameSaveData = new GameSaveData();

                InitUI();
                LoadNextWave();
                InitText();

                defenseIconManager = new DefenseIconManager(Game, playerBobux);
            }
        }

        public override void Update(GameTime gameTime)
        {
            goButton.Update(Game.MousePosition);
            cancelButton.Update(Game.MousePosition);

            HandleGoButtonPress();

            UpdateDefenseIcons();

            SpawnZombies(gameTime);

            UpdateEntities(gameTime);

            CheckGameOver();
        }

        public override void Draw(SpriteBatch sb)
        {
            entities.ForEach(e => e.Draw(sb));
            defenses.ForEach(d => d.Draw(sb));
            zombies.ForEach(z => z.Draw(sb));

            rightHudSprite.Draw(sb);
            bottomHudSprite.Draw(sb);

            bobuxSprite.Draw(sb);
            heartSprite.Draw(sb);

            goButton.Draw(sb);
            cancelButton.Draw(sb);

            Game.TextManager.Draw(sb);
            defenseIconManager.Draw(sb);
        }

        private void InitUI()
        {
            rightHudSprite = new Sprite(Game.Content, "Textures/HUDBar");
            rightHudSprite.DestRec = new Rectangle(new Point(Game1.SCREEN_WIDTH - 400, 0), new Point(400,
                Game1.TILE_SIZE * Game1.TILES_HIGH));

            bobuxSprite = new Sprite(Game.Content, "Textures/UI/Bobux");
            bobuxSprite.DestRec = new Rectangle(rightHudSprite.DestRec.X + 10, rightHudSprite.DestRec.Top + 10, (int)(bobuxSprite.Texture.Width * 0.25),
                (int)(bobuxSprite.Texture.Height * 0.25));

            heartSprite = new Sprite(Game.Content, "Textures/UI/Heart");
            heartSprite.DestRec = new Rectangle(bobuxSprite.DestRec.Location + new Point(-20, 40), new Point(75, 75));

            goButton = new Button(Game.Content, new Point(610, 350), new Point(50, 50), Game1.Font, "Go");
            cancelButton = new Button(Game.Content, new Point(680, 350), new Point(85, 50), Game1.Font, "Cancel");

            bottomHudSprite = new Sprite(Game.Content, "Textures/HUDBar");
            bottomHudSprite.DestRec = new Rectangle(0, Game1.SCREEN_HEIGHT -
                (Game1.TILES_WIDE * Game1.TILE_SIZE),
                Game1.SCREEN_WIDTH,
                Game1.SCREEN_HEIGHT - (Game1.TILES_WIDE * Game1.TILE_SIZE));
        }

        private void BuyDefense(DefenseIcon icon)
        {
            playerBobux -= icon.DefenseInfo.Cost;

            if (Game.ProfileManager.AreProfilesEnabled)
            {
                GameSaveData.DefensesPurchased++;
            }

            defenseIconManager.UpdateNoSymbols(playerBobux);

            UpdateText(TextType.PLAYER_BOBUX);
        }

        private void InitText()
        {
            Game.TextManager.AddText("TotalZombiesKilled", new Vector2(bottomHudSprite.DestRec.Left, bottomHudSprite.DestRec.Top + 5));
            Game.TextManager.AddText("DefensesOwned", new Vector2(bottomHudSprite.DestRec.Left, bottomHudSprite.DestRec.Top + 25));
            Game.TextManager.AddText("DamageDealt", new Vector2(bottomHudSprite.DestRec.Left, bottomHudSprite.DestRec.Top + 45));
            Game.TextManager.AddText("PlayerBobux", bobuxSprite.DestRec.Center.ToVector2() + new Point(30, 0).ToVector2());
            Game.TextManager.AddText("TowerHealth", (heartSprite.DestRec.Location + new Point(65, 30)).ToVector2());
            Game.TextManager.AddText("WaveNumber", new Vector2(725, 25));
            Game.TextManager.AddText("ZombiesLeftInWave", new Vector2(725, 50));

            UpdateText(TextType.TOTAL_ZOMBIES_KILLED);
            UpdateText(TextType.DEFENSES_OWNED);
            UpdateText(TextType.PLAYER_BOBUX);
            UpdateText(TextType.TOWER_HEALTH);
            UpdateText(TextType.WAVE_NUMBER);
            UpdateText(TextType.ZOMBIES_LEFT_IN_WAVE);
            UpdateText(TextType.DAMAGE_DEALT);
        }

        private void HandleGoButtonPress()
        {
            if (goButton.IsPressed() && defenseIconManager.SelectedDefenseIcon == null)
            {
                switch (currentLevelState)
                {
                    case WaveState.LOADED:
                    currentLevelState = WaveState.LOADED_AND_PLAYING;

                    break;

                    case WaveState.OVER:
                    LoadNextWave();
                    break;
                }
            }
        }

        private void SpawnZombies(GameTime gameTime)
        {
            if (!waveManager.IsWaveOver() && currentLevelState == WaveState.LOADED_AND_PLAYING)
            {
                if (timeUntilZombieSpawn <= 0f)
                {
                    zombies.Add(waveManager.PopNextZombie());

                    if (!waveManager.IsWaveOver())
                    {
                        timeUntilZombieSpawn = GetRandomZombieSpawnTime();
                    }
                }
                else
                {
                    timeUntilZombieSpawn -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        private void UpdateZombiesKilled(Zombie zombie)
        {
            if (Game.ProfileManager.AreProfilesEnabled)
            {
                GameSaveData.ZombiesKilledOfEachType[Zombie.GetIndex(zombie.Type)]++;
            }

            zombiesLeftInWave--;

            UpdateText(TextType.TOTAL_ZOMBIES_KILLED);
            UpdateText(TextType.ZOMBIES_LEFT_IN_WAVE);
        }

        private void UpdateEntities(GameTime gameTime)
        {
            entities.ForEach(x => x.Update(Game, gameTime));
            zombies.ForEach(x => x.Update(Game, gameTime));

            defenses.ForEach(x => x.Update(Game, gameTime));
            defenses.ForEach(x => x.UpdateProjectiles(Game, gameTime, zombies));

            for (int i = 0; i < defenses.Count; i++)
            {
                if (defenses[i] is Grenade grenade)
                {
                    if (grenade.ShouldBeRemoved)
                    {
                        defenses.RemoveAt(i);
                    }
                }
            }

            for (int i = 0; i < zombies.Count; i++)
            {
                if (zombies[i].IsDead)
                {
                    UpdateZombiesKilled(zombies[i]);
                    zombies.RemoveAt(i);
                }
                else if (zombies[i].IntersectsWithTower(GetTower()))
                {
                    zombies[i].DamageTower(GetTower());

                    zombiesLeftInWave--;
                    zombies.RemoveAt(i);

                    UpdateText(TextType.ZOMBIES_LEFT_IN_WAVE);
                    UpdateText(TextType.TOWER_HEALTH);

                    if (Tower.IsDead)
                    {
                        break;
                    }
                }
            }

            if (!Tower.IsDead)
            {
                foreach (Defense defense in defenses)
                {
                    defense.ShootAtNearestZombie(Game, gameTime, zombies);
                }
            }

            UpdateText(TextType.DAMAGE_DEALT);
        }

        private void AddSelectedDefense(Point position)
        {
            switch (defenseIconManager.SelectedDefenseIcon.Type)
            {
                case EntityType.ARROW_TURRET:
                defenses.Add(new ArrowTurret(Game.Content, position));
                break;

                case EntityType.ICE_TURRET:
                defenses.Add(new IceTurret(Game.Content, position));
                break;

                case EntityType.BOMB_TURRET:
                defenses.Add(new BombTurret(Game.Content, position));
                break;

                case EntityType.GRENADE:
                defenses.Add(new Grenade(Game.Content, position, zombies));
                break;
            }

            UpdateText(TextType.DEFENSES_OWNED);
            BuyDefense(defenseIconManager.SelectedDefenseIcon);
            defenseIconManager.ResetDefenseIcon();
        }

        private void UpdateDefenseIcons()
        {
            if (defenseIconManager.SelectedDefenseIcon != null)
            {
                defenseIconManager.UpdateCursorPosition();

                if (Input.IsMouseButtonPressed(MouseButton.LEFT) && cancelButton.IsMouseOver())
                {
                    defenseIconManager.ResetDefenseIcon();
                }
            }

            if (Input.IsMouseButtonPressed(MouseButton.LEFT))
            {
                if (defenseIconManager.SelectedDefenseIcon == null)
                {
                    defenseIconManager.DetermineIconPressed();
                }
                else
                {
                    int row = (int)Math.Round((float)Game.MousePosition.Y / Game1.TILE_SIZE);
                    int col = (int)Math.Round((float)Game.MousePosition.X / Game1.TILE_SIZE);

                    if (Game1.IsCoordValid(row, col))
                    {
                        Point snappedPosition = new Point(col * Game1.TILE_SIZE, row * Game1.TILE_SIZE);

                        if (CanBePlaced(snappedPosition))
                        {
                            AddSelectedDefense(snappedPosition);
                        }
                    }
                }
            }
        }

        private bool CanBePlaced(Point snappedPosition)
        {
            return entities.Where(e => e.GetPosition() == snappedPosition)
    .All(defenseIconManager.SelectedDefenseIcon.CanBePlacedOnEntity) &&
    !defenses.Exists(x => x.GetPosition() == snappedPosition);
        }

        private void CheckGameOver()
        {
            if (currentLevelState == WaveState.LOADED_AND_PLAYING)
            {
                if (zombiesLeftInWave == 0)
                {
                    currentLevelState = WaveState.OVER;
                    GameSaveData.WavesCompleted++;
                }

                if (IsGameOver())
                {
                    GameSaveData.IsWin = !Tower.IsDead;

                    SceneToExitTo = "GameOver";

                    if (Game.ProfileManager.AreProfilesEnabled)
                    {
                        Game.ProfileManager.SaveToProfile(GameSaveData);
                    }

                    goButton.IsActive = false;
                    cancelButton.IsActive = false;

                    defenseIconManager.ResetDefenseIcon();
                }
            }
        }

        private bool IsGameOver()
        {
            return (currentLevelState == WaveState.OVER && isLastRound) || Tower.IsDead;
        }

        public Tower GetTower()
        {
            return (Tower)entities[TowerIndex];
        }

        private void LoadNextWave()
        {
            entities.Clear();
            defenses.Clear();

            waveManager.MoveNextWave();
            entities.AddRange(waveManager.GetWaveLayout());
            TowerIndex = waveManager.GetTowerIndex();

            isLastRound = waveManager.WaveIndex == WaveManager.WAVES_COUNT - 1;

            currentLevelState = WaveState.LOADED;

            zombiesLeftInWave = waveManager.GetEnemyCount();

            if (Game.TextManager.KeyExists("ZombiesLeftInWave"))
            {
                playerBobux += STARTING_BOBUX * Utility.GetRandom(BOBUX_MULTIPLIER_MIN, BOBUX_MULTIPLIER_MAX + 1);
                defenseIconManager.UpdateNoSymbols(playerBobux);

                UpdateText(TextType.ZOMBIES_LEFT_IN_WAVE);
                UpdateText(TextType.PLAYER_BOBUX);
                UpdateText(TextType.WAVE_NUMBER);
                UpdateText(TextType.DEFENSES_OWNED);
            }

        }

        public void Reset()
        {
            entities.Clear();
            zombies.Clear();
            defenses.Clear();

            waveManager.Reset(Game);



            waveManager.MoveNextWave();
            entities.AddRange(waveManager.GetWaveLayout());

            zombiesLeftInWave = waveManager.GetEnemyCount();

            playerBobux = STARTING_BOBUX;

            timeUntilZombieSpawn = 0f;
            TowerIndex = waveManager.GetTowerIndex();

            Tower.ResetHealth();

            GameSaveData.Reset();
            isLastRound = waveManager.WaveIndex == WaveManager.WAVES_COUNT - 1;

            currentLevelState = WaveState.LOADED;

            defenseIconManager.UpdateNoSymbols(playerBobux);

            UpdateText(TextType.ZOMBIES_LEFT_IN_WAVE);
            UpdateText(TextType.PLAYER_BOBUX);
            UpdateText(TextType.TOWER_HEALTH);
            UpdateText(TextType.DEFENSES_OWNED);
            UpdateText(TextType.TOTAL_ZOMBIES_KILLED);
            UpdateText(TextType.WAVE_NUMBER);
            UpdateText(TextType.DAMAGE_DEALT);

            goButton.IsActive = true;
            cancelButton.IsActive = true;

        }

        private float GetRandomZombieSpawnTime()
        {
            return Utility.GetRandom(ZOMBIE_SPAWN_TIME_MIN, ZOMBIE_SPAWN_TIME_MAX);
        }

        private void UpdateText(TextType type)
        {
            switch (type)
            {
                case TextType.ZOMBIES_LEFT_IN_WAVE:
                Game.TextManager["ZombiesLeftInWave"].DisplayedString = $"Left: {zombiesLeftInWave}";
                break;

                case TextType.PLAYER_BOBUX:
                Game.TextManager["PlayerBobux"].DisplayedString = playerBobux.ToString();
                break;

                case TextType.WAVE_NUMBER:
                Game.TextManager["WaveNumber"].DisplayedString = $"Wave: {waveManager.WaveIndex + 1}";
                break;

                case TextType.TOWER_HEALTH:
                Game.TextManager["TowerHealth"].DisplayedString = Tower.GetHealth().ToString();
                break;

                case TextType.TOTAL_ZOMBIES_KILLED:
                Game.TextManager["TotalZombiesKilled"].DisplayedString = $"Total Zombies Killed: {GameSaveData.TotalZombiesKilled}";
                break;

                case TextType.DEFENSES_OWNED:
                Game.TextManager["DefensesOwned"].DisplayedString = $"Defenses Owned: {defenses.Count}";
                break;

                case TextType.DAMAGE_DEALT:
                Game.TextManager["DamageDealt"].DisplayedString = $"Damage Dealt: {GameSaveData.DamageDealt}";
                break;
            }
        }
    }
}