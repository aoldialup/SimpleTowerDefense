
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class DefenseIconManager
    {
        private List<DefenseIcon> defenseIcons;

        public DefenseIcon SelectedDefenseIcon { get; private set; }

        private Game1 game;

        public DefenseIconManager(Game1 game, int playerBobux)
        {
            this.game = game;

            defenseIcons = new List<DefenseIcon>()
            {
                new DefenseIcon(game.Content, EntityType.ARROW_TURRET, ArrowTurret.info,
                new Vector2(605f, 140f), game.Content.Load<Texture2D>("Textures/Defenses/ArcherTurret"),
                (Entity e) => e.Type == EntityType.GRASS_TILE),

                new DefenseIcon(game.Content, EntityType.ICE_TURRET, IceTurret.info,
                new Vector2(705f, 140f), game.Content.Load<Texture2D>("Textures/Defenses/IceTurret"),
                (Entity e) => e.Type == EntityType.GRASS_TILE),

                new DefenseIcon(game.Content, EntityType.BOMB_TURRET, BombTurret.info,
                new Vector2(605f, 220f), game.Content.Load<Texture2D>("Textures/Defenses/BombTurret"),
                (Entity e) => e.Type == EntityType.GRASS_TILE),

                                new DefenseIcon(game.Content, EntityType.GRENADE, Grenade.defenseInfo,
                new Vector2(705f, 220f), game.Content.Load<Texture2D>("Textures/Defenses/Grenade"),
                (Entity e) => e.Type != EntityType.TOWER)
            };

            UpdateNoSymbols(playerBobux);
        }

        public void Draw(SpriteBatch sb)
        {
            defenseIcons.ForEach(x => x.Draw(sb, false));

            if (SelectedDefenseIcon != null)
            {
                SelectedDefenseIcon.Draw(sb, true);
            }
        }

        public void UpdateNoSymbols(int playerBobux)
        {
            foreach (DefenseIcon icon in defenseIcons)
            {
                icon.UpdateNoSymbol(playerBobux);
            }
        }

        public void ResetDefenseIcon()
        {
            if (SelectedDefenseIcon != null)
            {
                SelectedDefenseIcon.ResetPosition();

                game.IsMouseVisible = true;

                SelectedDefenseIcon = null;
            }
        }

        public void UpdateCursorPosition()
        {
            if (SelectedDefenseIcon != null)
            {
                SelectedDefenseIcon.UpdateCursorPosition(game.MousePosition);
            }
        }

        public void DetermineIconPressed()
        {
            for (int i = 0; i < defenseIcons.Count; i++)
            {
                if (defenseIcons[i].Contains(game.MousePosition))
                {
                    if (defenseIcons[i].IsPurchasable)
                    {
                        game.IsMouseVisible = false;

                        SelectedDefenseIcon = defenseIcons[i];

                        SelectedDefenseIcon.UpdateCursorPosition(game.MousePosition);
                        break;
                    }
                }
            }
        }
    }
}
