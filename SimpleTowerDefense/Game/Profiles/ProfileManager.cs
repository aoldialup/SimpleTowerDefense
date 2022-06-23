
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Engine;

namespace SimpleTowerDefense
{
    enum ProfileSortMode
    {
        GAMES_WON,
        GAMES_PLAYED,
        ZOMBIES_KILLED,
        WL_RATIO,
        GAMES_LOST,
        PROFILE_NUMBER,
        DEFENSES_PURCHASED,
        WAVES_COMPLETED,
        DAMAGE_DEALT
    }

    internal class ProfileManager
    {
        private const int MAX_PROFILES = 3;
        private const int PROFILE_DATA_LENGTH = 9;

        private static Texture2D blankTexture;

        private List<Profile> profiles;

        public int ProfileIndex { get; private set; } = -1;

        private Rectangle profileAveragesDestRec;
        private Text profileAveragesText;

        public bool AreProfilesEnabled { get; private set; } = true;

        public bool IsProfileSelected
        {
            get
            {
                return ProfileIndex != -1;
            }
        }

        public ProfileManager(ContentManager content)
        {
            profiles = new List<Profile>(MAX_PROFILES);

            StreamReader inFile = StreamReader.Null;

            try
            {
                inFile = File.OpenText("Save.txt");
                AddProfiles(content, inFile);
            }
            catch (FileNotFoundException)
            {
                CreateSave(content);
            }
            catch (Exception)
            {
                AreProfilesEnabled = false;
            }
            finally
            {
                inFile?.Close();
            }

            blankTexture = content.Load<Texture2D>("Textures/BlankTexture");

            profileAveragesDestRec = new Rectangle(new Point(5, 100), new Point(280, 100));
            profileAveragesText = new Text(new Vector2(profileAveragesDestRec.Left, profileAveragesDestRec.Top),
                Game1.Font);
            profileAveragesText.Color = Color.Black;

        }

        private void AddProfiles(ContentManager content, StreamReader inFile)
        {
            for (int i = 0; i < MAX_PROFILES; i++)
            {
                try
                {
                    string[] profileData = inFile.ReadLine().Split(',');

                    if (profileData.Length != PROFILE_DATA_LENGTH)
                    {
                        throw new Exception();
                    }

                    int gamesPlayed = 0;
                    int gamesWon = 0;
                    int wavesCompleted = 0;
                    int damageDealt = 0;
                    int defensesPurchased = 0;
                    int[] zombiesKilledOfEachType = new int[Zombie.TOTAL_TYPES];

                    gamesPlayed = int.Parse(profileData[0]);
                    gamesWon = int.Parse(profileData[1]);
                    wavesCompleted = int.Parse(profileData[2]);

                    zombiesKilledOfEachType[Zombie.EASY_INDEX] = int.Parse(profileData[3]);
                    zombiesKilledOfEachType[Zombie.MEDIUM_INDEX] = int.Parse(profileData[4]);
                    zombiesKilledOfEachType[Zombie.HARD_INDEX] = int.Parse(profileData[5]);
                    zombiesKilledOfEachType[Zombie.BOSS_INDEX] = int.Parse(profileData[6]);

                    damageDealt = int.Parse(profileData[7]);
                    defensesPurchased = int.Parse(profileData[8]);

                    if (gamesPlayed < 0 || wavesCompleted < 0 || damageDealt < 0 || zombiesKilledOfEachType.Any(x => x < 0) ||
    gamesWon < 0 || gamesWon > gamesPlayed || defensesPurchased < 0)
                    {
                        throw new Exception();
                    }

                    profiles.Add(
    new Profile(
        content,
        new Point(Profile.X_START + Profile.X_OFFSET * i, Profile.Y_OFFSET),
        i + 1, gamesPlayed, gamesWon, wavesCompleted,
        zombiesKilledOfEachType[Zombie.EASY_INDEX],
        zombiesKilledOfEachType[Zombie.MEDIUM_INDEX],
        zombiesKilledOfEachType[Zombie.HARD_INDEX],
        zombiesKilledOfEachType[Zombie.BOSS_INDEX],
        damageDealt, defensesPurchased));

                }
                catch (Exception)
                {
                    AreProfilesEnabled = false;
                    profiles.Clear();
                    break;
                }
            }
        }

        public void CreateSave(ContentManager content)
        {
            StreamWriter outFile = StreamWriter.Null;

            try
            {
                outFile = File.CreateText("Save.txt");

                for (int i = 0; i < MAX_PROFILES; i++)
                {
                    Profile profile = new Profile(content, new Point(Profile.X_START + Profile.X_OFFSET * i, Profile.Y_OFFSET), i + 1);
                    outFile.WriteLine(profile.GetFileFormat());
                    profiles.Add(profile);
                }
            }
            catch (Exception)
            {
                AreProfilesEnabled = false;
                profiles.Clear();
            }
            finally
            {
                outFile?.Close();
            }
        }

        public void SortProfiles(ProfileSortMode sortMode)
        {
            switch (sortMode)
            {
                case ProfileSortMode.GAMES_PLAYED:
                SortProfiles((a, b) => a.GamesPlayed > b.GamesPlayed);
                break;

                case ProfileSortMode.GAMES_WON:
                SortProfiles((a, b) => a.GamesWon > b.GamesWon);
                break;

                case ProfileSortMode.GAMES_LOST:
                SortProfiles((a, b) => a.GamesLost > b.GamesLost);
                break;

                case ProfileSortMode.WL_RATIO:
                SortProfiles((a, b) => a.WLRatio > b.WLRatio);
                break;

                case ProfileSortMode.DAMAGE_DEALT:
                SortProfiles((a, b) => a.DamageDealt > b.DamageDealt);
                break;

                case ProfileSortMode.DEFENSES_PURCHASED:
                SortProfiles((a, b) => a.DefensesPurchased > b.DefensesPurchased);
                break;

                case ProfileSortMode.WAVES_COMPLETED:
                SortProfiles((a, b) => a.WavesCompleted > b.WavesCompleted);
                break;

                case ProfileSortMode.PROFILE_NUMBER:
                SortProfiles((a, b) => a.ProfileNumber > b.ProfileNumber);
                break;

                case ProfileSortMode.ZOMBIES_KILLED:
                SortProfiles((a, b) => a.ZombiesKilled > b.ZombiesKilled);
                break;
            }
        }

        private void SortProfiles(Func<Profile, Profile, bool> sortingFunction)
        {
            for (int i = 0; i < profiles.Count - 1; i++)
            {
                for (int j = 1; j < profiles.Count - 1; j++)
                {
                    if (sortingFunction(profiles[j - 1], profiles[j]))
                    {
                        Profile temp = profiles[j];

                        profiles[j] = profiles[j - 1];

                        profiles[j - 1] = temp;
                    }
                }
            }
        }

        public void SaveToProfile(GameSaveData saveData)
        {
            StreamWriter outFile = StreamWriter.Null;

            profiles[ProfileIndex].AddSave(saveData);

            try
            {
                outFile = File.CreateText("Save.txt");

                foreach (Profile p in profiles)
                {
                    outFile.WriteLine(p.GetFileFormat());
                }
            }
            catch (Exception)
            {
                AreProfilesEnabled = false;
                profiles.Clear();
            }
            finally
            {
                outFile?.Close();
            }
        }

        public void Update(Point mousePosition)
        {
            foreach (Profile p in profiles)
            {
                p.Update(mousePosition);
            }
        }

        public void UpdateProfileSelection(Point mousePosition)
        {
            for (int i = 0; i < profiles.Count; i++)
            {
                if (profiles[i].Contains(mousePosition))
                {
                    ProfileIndex = i;
                    break;
                }
            }
        }

        public void DisplayProfileStats(SpriteBatch sb, Point position)
        {
            profileAveragesDestRec.Location = position;
            profileAveragesText.Position = position.ToVector2();

            if (ProfileIndex != -1)
            {
                sb.Draw(blankTexture, profileAveragesDestRec, Color.White);

                profileAveragesText.DisplayedString = profiles[ProfileIndex].GetAveragesString();

                profileAveragesText.Draw(sb);

            }
            else
            {
                profileAveragesText.DisplayedString = "Stats are disabled";
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Profile p in profiles)
            {
                p.Draw(sb);
            }
        }
    }
}
