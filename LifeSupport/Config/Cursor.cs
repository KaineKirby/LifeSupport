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

    
    class Cursor
    {
        /*Get the center x point of the crosshair (mouse pointer) */
        public float mouseImageCenterX;

        /*Get the center y point of the crosshair (mouse pointer) */
        public float mouseImageCenterY;

        /* Store the (x,y) point of the mouse pointer */
        public Vector2 MousePosition;

        /*Load the crosshair image. This will act as the mouse pointer */
        public Texture2D mouseImage;

        /*Mouse state is a built in Monogame object that will be able to get the state of the mouse (pressed, released, not pressed) */
        public MouseState mouseState;

        /*Scale the size of the crosshair image */
        public float scale = 1.0f;

        //  static extern void ClipCursor(ref Rectangle rect);


        //singleton reference
        public static Cursor instance;
        public static Cursor Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                else
                    return new Cursor();
            }
            private set
            {
                instance = value;
            }
        }

        private Cursor()
        {
            mouseImage = Assets.Instance.cursor;
            mouseImageCenterX = (mouseImage.Width * scale) / 2;
            mouseImageCenterY = (mouseImage.Height * scale) / 2;
            mouseState = Mouse.GetState();
            this.MousePosition = new Vector2(mouseState.X - mouseImageCenterX, mouseState.Y - mouseImageCenterY);
        }

        /* Continually update the mouse. This function will updates the mouse crosshair location */
        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            MousePosition = new Vector2(mouseState.X - mouseImageCenterX, mouseState.Y - mouseImageCenterY);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mouseImage, MousePosition,new Rectangle(0,0,32,32), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public bool IsLeftMouseDown() {
            return mouseState.LeftButton == ButtonState.Pressed ;
        }

        //the source should be in screen coordinates not world
        public Vector2 GetDirection(Point source) {
            Vector2 norm = new Vector2(MousePosition.X-source.X, MousePosition.Y-source.Y) ;
            norm.Normalize() ;
            return norm ;
        }
    }
}
