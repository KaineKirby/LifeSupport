using LifeSupport.Config;
using Microsoft.Xna.Framework;
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
    // This class serves as the default button that will be used in all the interactive menu screens
    public class Button : Component
    {
        #region Fields
        protected MouseState currMouse;
        protected SpriteFont font;
        protected bool hover;
        protected MouseState prevMouse;
        protected Texture2D texture;
        #endregion

        #region Properties
        public event EventHandler Click;
        public bool IsClicked { get; set; }
        public Color ThisColor { get; set; }
        public Vector2 CurrPosition { get; set; }
        public Texture2D picture { get; set; }
        public String BtnText { get; set; }

        public Rectangle Rect
        {
            get
            {
                return new Rectangle((int)CurrPosition.X, (int)CurrPosition.Y, texture.Width, texture.Height);
            }
            set {; }

        }

        #endregion

        #region Methods

        // This constructor takes in the button image and text to go inside the button
        public Button(Texture2D BtnTexture, SpriteFont BtnFont)
        {
            texture = BtnTexture;
            font = BtnFont;
            ThisColor = Color.White;
            picture = BtnTexture;
            Click += PlaySound;
        }

        // This constructor does not need text inside the button
        public Button(Texture2D BtnTexture)
        {
            texture = BtnTexture;
            ThisColor = Color.White;
            Click += PlaySound;
        }

        public Button()
        {

        }

        //method to play sound will be on all buttons
        protected void PlaySound(object sender, EventArgs e)
        {
            Assets.Instance.menuClick.Play((float)Settings.Instance.SfxVolume / 100, 0f, 0f);
        }


        // How the button will be drawn
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // All buttons are outlined in white
            var currColor = Color.White;

            // If the button is hovered over with the mouse, change the outline to gray
            if (hover)
            {
                currColor = Color.Gray;
            }
            spriteBatch.Draw(texture, Rect, null, currColor, 0, Vector2.Zero, SpriteEffects.None, 0f);

            // Draw the text inside the button if it has text
            if (!string.IsNullOrEmpty(BtnText))
            {
                var x = (Rect.X + (Rect.Width / 2)) - (font.MeasureString(BtnText).X / 2);
                var y = (Rect.Y + (Rect.Height / 2)) - (font.MeasureString(BtnText).Y / 2);
                spriteBatch.DrawString(font, BtnText, new Vector2(x, y), ThisColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
        }

        // How the button will be updated
        public override void Update(GameTime gameTime)
        {
            // Get the mouse position (used to determine if it is being hovered over)
            prevMouse = currMouse;
            currMouse = Mouse.GetState();
            float scalar = (1920 / (float)Settings.Instance.Width);

            var MouseRect = new Rectangle((int)(scalar * currMouse.X), (int)(scalar * currMouse.Y), 1, 1);

            hover = false;

            // If the button is hovered over with the mouse, then the user can click on the button to call it's special function defined in other classes
            if (MouseRect.Intersects(Rect))
            {
                hover = true;
                if (currMouse.LeftButton == ButtonState.Released && prevMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
        #endregion
    }
}


