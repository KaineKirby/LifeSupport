using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using MonoGame ;
using LifeSupport.Config ;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace LifeSupport.GameObject {

    abstract class GameObject {

        //position on screen
        public float XPos ;
        public float YPos ;

        //width, height, and rotation on screen
        public int Width ;
        public int Height ;
        public int Rotation ;

        //image for the game object
        private readonly Texture2D sprite ;

        public GameObject(int xPos, int yPos, int width, int height, int rotation, String spritePath) {

            //we must scale to the screen resolution that is set in settings
            this.XPos = xPos;
            this.YPos = yPos ;
            this.Width = width;
            this.Height = height ;

            this.Rotation = rotation ;

            this.sprite = MainGame.Instance.Content.Load<Texture2D>(spritePath) ;

        }

        //render the sprite with its current position (independent of update)
	    //subclasses will likely have to call base() ;
        public void Render(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, new Rectangle((int)XPos, (int)YPos, Width, Height), Color.White) ;
        }

        //update the position of the object (this may be empty if the object is static, this is okay)
        public abstract void UpdatePosition(GameTime gameTime) ;

    }
}
