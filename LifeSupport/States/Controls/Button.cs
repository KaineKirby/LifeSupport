﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LifeSupport.Controls
{
    public class Button : Component
    {
        #region Fields
        private MouseState currMouse;
        private SpriteFont font;
        private bool hover;
        private MouseState prevMouse;
        private Texture2D texture;
        #endregion

        #region Properties
        public event EventHandler Click;
        public bool IsClicked { get; set; }
        public Color ThisColor { get; set; }
        public Vector2 CurrPosition { get; set; }

        public Rectangle Rect {
            get {
                return new Rectangle((int)CurrPosition.X, (int)CurrPosition.Y, texture.Width, texture.Height);
            }
        }

        public string BtnText { get; set; }
        #endregion

        #region Methods
        public Button(Texture2D BtnTexture, SpriteFont BtnFont) {
            texture = BtnTexture;
            font = BtnFont;
            ThisColor = Color.White;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            var currColor = Color.White;

            if(hover) {
                currColor = Color.Gray;
            }
            spriteBatch.Draw(texture, Rect, currColor);

            if(!string.IsNullOrEmpty(BtnText)) {
                var x = (Rect.X + (Rect.Width / 2)) - (font.MeasureString(BtnText).X / 2);
                var y = (Rect.Y + (Rect.Height / 2)) - (font.MeasureString(BtnText).Y / 2);

                spriteBatch.DrawString(font, BtnText, new Vector2(x, y), ThisColor);
            }       
        }

        public override void Update(GameTime gameTime) {
            prevMouse = currMouse;
            currMouse = Mouse.GetState();

            var MouseRect = new Rectangle(currMouse.X, currMouse.Y, 1, 1);

            hover = false;

            if(MouseRect.Intersects(Rect)) {
                hover = true;
                if(currMouse.LeftButton == ButtonState.Released && prevMouse.LeftButton == ButtonState.Pressed) {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
        #endregion
    }
}
