
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class ProfilesScene : Scene
    {
        private enum Mode
        {
            NORMAL,
            SORT,
            VIEW_STATS,
            PROFILE_LOAD_ERROR
        }

        private const int SORT_BUTTONS_COUNT_WIDE = 4;
        private const int SORT_BUTTONS_COUNT_HIGH = 2;

        private const int SORT_BUTTONS = 8;

        private const int UI_X_START = 10;
        private const int UI_X_OFFSET = 210;
        private const int UI_Y_OFFSET = 25;

        private const int UI_BUTTON_HEIGHT = 75;
        private const int UI_BUTTON_WIDTH = 150;

        private const int SORT_BUTTON_START_X = 115;
        private const int SORT_BUTTON_START_Y = 10;

        private const int SORT_BUTTON_OFFSET_X = 120;
        private const int SORT_BUTTON_OFFSET_Y = 85;

        private const int UI_SORT_BUTTON_WIDTH = 100;
        private const int UI_SORT_BUTTON_HEIGHT = 80;

        private Button sortButton;
        private Button resetButton;
        private Button viewStatsButton;
        private Button exitButton;
        private Button playButton;

        private Text selectedProfileText;

        private List<Button> sortButtons;

        private List<ProfileSortMode> sortButtonModes;

        ProfileManager profileManager;

        private Mode currentMode;

        public ProfilesScene()
            : base("Profiles")
        {

        }

        public override void Init()
        {
            sortButtons = new List<Button>(SORT_BUTTONS);
            sortButtonModes = new List<ProfileSortMode>(SORT_BUTTONS);

            List<Tuple<string, ProfileSortMode>> sortButtonInfo = new List<Tuple<string, ProfileSortMode>>
            {
                new Tuple<string, ProfileSortMode>("Games Played", ProfileSortMode.GAMES_PLAYED),
                new Tuple<string, ProfileSortMode>("Games Won", ProfileSortMode.GAMES_WON),
                new Tuple<string, ProfileSortMode>("Defenses\nPurchased", ProfileSortMode.DEFENSES_PURCHASED),
                new Tuple<string, ProfileSortMode>("Games Lost", ProfileSortMode.GAMES_LOST),
                new Tuple<string, ProfileSortMode>("Waves Completed", ProfileSortMode.WAVES_COMPLETED),
                new Tuple<string, ProfileSortMode>("Zombies Killed", ProfileSortMode.ZOMBIES_KILLED),
                new Tuple<string, ProfileSortMode>("Profile Number", ProfileSortMode.PROFILE_NUMBER),
                new Tuple<string, ProfileSortMode>("Damage Dealt", ProfileSortMode.DAMAGE_DEALT)
            };

            Tuple<string, ProfileSortMode> currentButtonData;

            for (int i = 0; i < SORT_BUTTONS_COUNT_HIGH; i++)
            {
                for (int j = 0; j < SORT_BUTTONS_COUNT_WIDE; j++)
                {
                    currentButtonData = sortButtonInfo[sortButtonInfo.Count - 1];
                    sortButtonInfo.RemoveAt(sortButtonInfo.Count - 1);

                    sortButtons.Add(
                    new Button(Game.Content,
                    new Point(SORT_BUTTON_START_X + SORT_BUTTON_OFFSET_X * j, SORT_BUTTON_START_Y + SORT_BUTTON_OFFSET_Y * i),
                    new Point(UI_SORT_BUTTON_WIDTH, UI_SORT_BUTTON_HEIGHT), Game1.SmallFont, currentButtonData.Item1));

                    sortButtonModes.Add(currentButtonData.Item2);
                }
            }

            exitButton = new Button(Game.Content, new Point(10, 50), new Point(100, 100), Game1.SmallFont, "Exit");

            playButton = new Button(Game.Content, new Point(UI_X_START, UI_Y_OFFSET),
                new Point(UI_BUTTON_WIDTH, UI_BUTTON_HEIGHT), Game1.SmallFont, "Play");

            sortButton = new Button(Game.Content, new Point(UI_X_START + UI_X_OFFSET, UI_Y_OFFSET),
                new Point(UI_BUTTON_WIDTH, UI_BUTTON_HEIGHT), Game1.SmallFont, "Sort");

            resetButton = new Button(Game.Content, new Point(UI_X_START + UI_X_OFFSET * 2, UI_Y_OFFSET),
                new Point(UI_BUTTON_WIDTH, UI_BUTTON_HEIGHT), Game1.SmallFont, "Reset");

            viewStatsButton = new Button(Game.Content, new Point(UI_X_START + UI_X_OFFSET * 3, UI_Y_OFFSET),
                new Point(UI_BUTTON_WIDTH, UI_BUTTON_HEIGHT), Game1.SmallFont, "View Stats");

            profileManager = Game.ProfileManager;

            selectedProfileText = new Text(new Vector2(300, 125), Game1.Font);
            UpdateProfileSelectionText();

            if (profileManager.AreProfilesEnabled)
            {
                currentMode = Mode.NORMAL;
            }
            else
            {
                currentMode = Mode.PROFILE_LOAD_ERROR;
            }
        }

        public override void Update(GameTime gameTime)
        {
            sortButton.Update(Game.MousePosition);
            resetButton.Update(Game.MousePosition);
            viewStatsButton.Update(Game.MousePosition);
            playButton.Update(Game.MousePosition);
            profileManager.Update(Game.MousePosition);

            switch (currentMode)
            {
                case Mode.NORMAL:
                UpdateNormalMode();
                break;

                case Mode.SORT:
                UpdateSortMode();
                break;

                case Mode.VIEW_STATS:
                UpdateViewStatsMode();
                break;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            switch (currentMode)
            {
                case Mode.PROFILE_LOAD_ERROR:
                DrawErrorMode(sb);
                break;

                case Mode.NORMAL:
                DrawNormalMode(sb);
                break;

                case Mode.SORT:
                DrawSortMode(sb);
                break;

                case Mode.VIEW_STATS:
                DrawViewStatsMode(sb);
                break;
            }
        }

        private void DrawErrorMode(SpriteBatch sb)
        {
            sb.DrawString(Game1.Font, "One or more profiles could not be read from or written to at some point in this session.\n" +
                "Fix the save file and re-open the game.",
                new Vector2(Game1.ACTUAL_SCREEN_WIDTH / 2, Game1.ACTUAL_SCREEN_HEIGHT / 2) - new Vector2(350, 0), Color.Red);
        }

        private void UpdateSortMode()
        {
            exitButton.Update(Game.MousePosition);

            foreach (Button b in sortButtons)
            {
                b.Update(Game.MousePosition);
            }

            if (Input.IsMouseButtonPressed(MouseButton.LEFT))
            {
                if (exitButton.IsMouseOver())
                {
                    currentMode = Mode.NORMAL;
                }
                else
                {
                    for (int i = 0; i < sortButtons.Count; i++)
                    {
                        if (sortButtons[i].IsMouseOver())
                        {
                            profileManager.SortProfiles(sortButtonModes[i]);
                            break;
                        }
                    }
                }
            }
        }

        private void UpdateNormalMode()
        {
            if (Input.IsMouseButtonPressed(MouseButton.LEFT))
            {
                profileManager.UpdateProfileSelection(Game.MousePosition);

                if (profileManager.IsProfileSelected)
                {
                    UpdateProfileSelectionText();
                }

                if (playButton.IsPressed())
                {
                    if (profileManager.IsProfileSelected)
                    {
                        SceneToExitTo = "Playing";
                    }
                }
                else
                {
                    if (sortButton.IsMouseOver())
                    {
                        currentMode = Mode.SORT;
                    }
                    else if (viewStatsButton.IsMouseOver())
                    {
                        currentMode = Mode.VIEW_STATS;
                    }
                    else if (resetButton.IsMouseOver())
                    {
                        profileManager.CreateSave(Game.Content);
                    }
                }
            }
        }

        private void UpdateViewStatsMode()
        {
            exitButton.Update(Game.MousePosition);

            if (Input.IsMouseButtonPressed(MouseButton.LEFT))
            {
                if (exitButton.IsMouseOver())
                {
                    currentMode = Mode.NORMAL;
                }
            }
        }

        private void UpdateProfileSelectionText()
        {
            if (profileManager.ProfileIndex != -1)
            {
                selectedProfileText.DisplayedString = $"Selected Profile: {profileManager.ProfileIndex + 1}";
            }
            else
            {
                selectedProfileText.DisplayedString = "No Profile Selected";
            }
        }

        private void DrawNormalMode(SpriteBatch sb)
        {
            sortButton.Draw(sb);
            resetButton.Draw(sb);
            viewStatsButton.Draw(sb);
            playButton.Draw(sb);
            selectedProfileText.Draw(sb);

            profileManager.Draw(sb);
        }

        private void DrawSortMode(SpriteBatch sb)
        {
            foreach (Button b in sortButtons)
            {
                b.Draw(sb);
            }

            exitButton.Draw(sb);

            sb.DrawString(Game1.Font, "Sorting Options", new Vector2(600, 50), Color.Red);
            profileManager.Draw(sb);
        }

        private void DrawViewStatsMode(SpriteBatch sb)
        {
            selectedProfileText.Draw(sb);

            if (profileManager.IsProfileSelected)
            {
                profileManager.DisplayProfileStats(sb, new Point(275, 250));
            }

            exitButton.Draw(sb);
        }
    }
}