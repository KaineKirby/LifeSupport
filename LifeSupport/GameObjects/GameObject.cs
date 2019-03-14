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
using System.Collections;


/*
* GameObject Class (Abstract)
* 
* The GameObject class will be the superclass from which all visuals objects in the game are derived
* This can include the player, enemies, barriers, etc, anything drawn on screen (probably)
* 
* Anything that we plan to implement must use the gameobject as its parent, otherwise we can not use it nicely in the Main (or level)
* 
* For the width and height of the image representation, we assume the pixels at 1920x1080, and then scaling is taken into account for 16:9
*/

namespace LifeSupport.GameObjects {

    abstract class GameObject {

        //position on screen
      
        public Rectangle Rect;
        
        //rotation on screen 
        public int Rotation ;

        //image for the game object
        protected Texture2D sprite ;

        //whether or not the object has collision
        public bool HasCollision ;

        public float XPos {
            get; private set;
        }

        public float YPos
        {
            get; private set;
        }

        public GameObject(Rectangle rect, int rotation, Texture2D sprite) {

            //we must scale to the screen resolution that is set in settings
            this.Rect = rect;
            this.Rotation = rotation ;
            this.sprite = sprite ;
            this.HasCollision = true ;
            this.XPos = Rect.X;
            this.YPos = Rect.Y;

        }

        //render the sprite with its current position (independent of update)
	    //subclasses will likely have to call base() ;
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, Rect, new Rectangle(0,0,32,32), Color.White, Rotation, new Vector2(0,0), SpriteEffects.None, 0);
        }

        //update the position of the object (this may be empty if the object is static, this is okay)
        public abstract void UpdatePosition(GameTime gameTime) ;

        public void MoveObject(float x, float y)
        {
            this.XPos = x;
            this.YPos = y;
            this.Rect.X = (int)XPos;
            this.Rect.Y = (int)YPos;
        }
    }
}
