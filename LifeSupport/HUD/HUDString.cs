using LifeSupport.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.HUD {
    class HUDString
    {

        private Vector2 position;
        private string text;
        private Color color;
        private SpriteFont font;

        public HUDString(string text, Color color, Vector2 position)
        {
            this.text = text;
            this.color = color;
            this.position = position;
        }

        public HUDString(SpriteFont font, string text, Color color, Vector2 position)
        {
            this.font = font;
            this.text = text;
            this.color = color;
            this.position = position;
        }


        public void Update(string text)
        {
            this.text = text;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Assets.Instance.mediumText, text, position, color);
        }

        public void DrawWithSpecificFont(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, color);
        }

    }
}
