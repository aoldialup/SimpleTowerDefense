
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class TextManager
    {
        private Dictionary<string, Text> texts;

        public TextManager()
        {
            texts = new Dictionary<string, Text>();
        }

        public void AddText(string key, Vector2 position)
        {
            texts.Add(key, new Text(position, Game1.Font));
        }

        public bool KeyExists(string name)
        {
            return texts.ContainsKey(name);
        }

        public Text this[string key]
        {
            get
            {
                return texts[key];
            }
            set
            {
                texts[key] = value;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Text t in texts.Values)
            {
                t.Draw(sb);
            }
        }
    }
}
