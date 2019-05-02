using LifeSupport.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.HUD {
    class HUDString {

        //the HUDString class represents a string to be drawn somewhere on the screen in screen corordinates

        private Vector2 position;
        private string text;
        private Color color;
        private SpriteFont font;

        public HUDString(string text, Color color, Vector2 position) {
            this.text = text;
            this.color = color;
            this.position = position;
            this.font = Assets.Instance.mediumText ;
        }

        public HUDString(SpriteFont font, string text, Color color, Vector2 position) {
            this.font = font;
            this.text = text;
            this.color = color;
            this.position = position;
        }

        //update the text in the string
        public void Update(string text) {
            this.text = text;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(font, text, position, color);
        }

    }
}
