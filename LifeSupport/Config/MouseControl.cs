using LifeSupport.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.Config
{
    class MouseControl
    {


        private float mouseImageCenterX;
        private float mouseImageCenterY;
        public Vector2 mousePosition;
        private Texture2D mouseImage;
        public MouseState mouseState;
        private float scale = 0.15f;

      //  static extern void ClipCursor(ref Rectangle rect);

        public MouseControl(Game game)
        {
            mouseImage = game.Content.Load<Texture2D>("crosshair");
            mouseImageCenterX = (mouseImage.Width * scale) / 2;
            mouseImageCenterY = (mouseImage.Height * scale) / 2;
            mouseState = Mouse.GetState();
            mousePosition = new Vector2(mouseState.X - mouseImageCenterX, mouseState.Y - mouseImageCenterY);
        }

        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            this.mousePosition = new Vector2(mouseState.X - mouseImageCenterX, mouseState.Y - mouseImageCenterY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mouseImage, mousePosition,null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public Vector2 getMousePosition()
        {
            return this.mousePosition;
        }




    }
}
