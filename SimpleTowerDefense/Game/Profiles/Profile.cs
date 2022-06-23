
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Engine;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace SimpleTowerDefense
{
    internal class Profile
    {
        public const int Y_OFFSET = 180;
        public const int X_OFFSET = 270;

        public const int X_START = 10;

        private static readonly Point Size = new Point(240, 290);

        private Texture2D blankTexture;
        private Rectangle destRec;
        private Point mousePosition;
        private Text displayText;

        public Point Position { get; set; }

        public int GamesPlayed { get; private set; }

        public int GamesWon { get; private set; }

        public int DamageDealt { get; private set; }

        public int DefensesPurchased { get; private set; }

        public int WavesCompleted { get; private set; }

        public int[] ZombiesKilledOfEachType { get; private set; }

        public int GamesLost
        {
            get
            {
                return GamesPlayed - GamesWon;
            }
        }

        public float WLRatio
        {
            get
            {
                if (GamesLost == 0)
                {
                    return 0;
                }

                return GamesWon / GamesLost;
            }
        }

        public int AverageWavesCompleted
        {
            get
            {
                if (GamesPlayed == 0)
                {
                    return 0;
                }


                return WavesCompleted / GamesPlayed;
            }
        }

        public int AverageDefensesPurchased
        {
            get
            {

                if (GamesPlayed == 0)
                {
                    return 0;
                }

                return DefensesPurchased / GamesPlayed;
            }
        }

        public int AverageZombiesKilled
        {
            get
            {
                if (GamesPlayed == 0)
                {
                    return 0;
                }

                return ZombiesKilled / GamesPlayed;
            }
        }

        public int AverageDamageDealt
        {
            get
            {
                if (GamesPlayed == 0)
                {
                    return 0;
                }

                return DamageDealt / GamesPlayed;
            }
        }


        public int ProfileNumber { get; set; }

        public int ZombiesKilled
        {
            get
            {
                return ZombiesKilledOfEachType.Sum();
            }
        }

        public Profile(ContentManager content, Point position, int profileNumber, int gamesPlayed = 0, int gamesWon = 0,
    int wavesCompleted = 0, int easyZombieKills = 0, int mediumZombieKills = 0, int hardZombieKills = 0, int bossZombieKills = 0,
    int damageDealt = 0, int defensesPurchased = 0)
        {
            ProfileNumber = profileNumber;
            GamesPlayed = gamesPlayed;
            GamesWon = gamesWon;
            WavesCompleted = wavesCompleted;
            DamageDealt = damageDealt;
            DefensesPurchased = defensesPurchased;
            WavesCompleted = WavesCompleted;
            ZombiesKilledOfEachType = new int[] { easyZombieKills, mediumZombieKills, hardZombieKills, bossZombieKills };

            if (blankTexture == null)
            {
                blankTexture = content.Load<Texture2D>("Textures/BlankTexture");
            }

            Position = position;

            destRec = new Rectangle(Position, Size);
            displayText = new Text(new Vector2(destRec.Left, destRec.Top), Game1.Font, string.Empty);
            displayText.Color = Color.Black;

            Refresh();
        }

        public void AddSave(GameSaveData save)
        {
            GamesPlayed++;
            GamesWon += Convert.ToInt32(save.IsWin);
            DamageDealt += save.DamageDealt;
            DefensesPurchased += save.DefensesPurchased;
            WavesCompleted += save.WavesCompleted;

            for (int i = 0; i < Zombie.TOTAL_TYPES; i++)
            {
                ZombiesKilledOfEachType[i] += save.ZombiesKilledOfEachType[i];
            }

            Refresh();
        }

        public string GetAveragesString()
        {
            return
$@"                 Current Profile Averages 
Average Waves Completed: {AverageWavesCompleted}
Average Defenses Purchased: {AverageDefensesPurchased}
Average Zombies Killed: {AverageZombiesKilled}
Average Damage Dealt: {AverageDamageDealt}
";

        }

        public string GetFileFormat()
        {
            return $"{GamesPlayed},{GamesWon},{WavesCompleted}," +
    $"{ZombiesKilledOfEachType[Zombie.EASY_INDEX]},{ZombiesKilledOfEachType[Zombie.MEDIUM_INDEX]}," +
    $"{ZombiesKilledOfEachType[Zombie.HARD_INDEX]},{ZombiesKilledOfEachType[Zombie.BOSS_INDEX]}," +
    $"{DamageDealt},{DefensesPurchased}";
        }

        public void Refresh()
        {
            displayText.DisplayedString =
$@"                 Profile {ProfileNumber}

Games Played: {GamesPlayed}
Wins: {GamesWon}
Losses: {GamesLost} 
W/L Ratio: {WLRatio}
Waves Completed: {WavesCompleted}

Total Defenses Bought: {DefensesPurchased}
Total Waves Completed: {WavesCompleted}
Total Damage Dealt: {DamageDealt}

Easy Zombies Killed: {ZombiesKilledOfEachType[Zombie.EASY_INDEX]}
Medium Zombies Killed: {ZombiesKilledOfEachType[Zombie.MEDIUM_INDEX]}
Hard Zombies Killed: {ZombiesKilledOfEachType[Zombie.HARD_INDEX]}
Boss Zombies Killed: {ZombiesKilledOfEachType[Zombie.BOSS_INDEX]}";
        }

        public void Update(Point mousePosition)
        {
            this.mousePosition = mousePosition;
        }

        public bool Contains(Point mousePosition)
        {
            return destRec.Contains(mousePosition);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(blankTexture, destRec, Color.White);
            displayText.Draw(sb);
        }
    }
}