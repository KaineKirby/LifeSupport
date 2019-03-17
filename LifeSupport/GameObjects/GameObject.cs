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
        public Vector2 Position ;
        //x and y are defined as middle point
        public int Width ;
        public int Height ;

        //utility getters for each side
        public float Left {
            get {
                return Position.X-(Width/2) ;
            }
        }
        public float Right {
            get {
                return Position.X+(Width/2) ;
            }
        }
        public float Top {
            get {
                return Position.Y-(Height/2) ;
            }
        }
        public float Bottom {
            get {
                return Position.Y+(Height/2) ; ;
            }
        }

        //related to drawing
        private Vector2 origin ;
        private Rectangle spriteRectangle ;
        
        //rotation on screen 
        public int Rotation ;

        //image for the game object
        protected Texture2D sprite ;

        //whether or not the object has collision
        public bool HasCollision ;

        public GameObject(Vector2 position, int width, int height, int rotation, Texture2D sprite) {

            //position and rotation
            this.Position = position ;
            this.Rotation = rotation ;

            this.Width = width ;
            this.Height = height ;

            this.sprite = sprite ;
            this.origin = new Vector2(width/2, height/2) ;
            this.spriteRectangle = new Rectangle(0, 0, width, height) ;
            this.HasCollision = true ;

        }

        //render the sprite with its current position (independent of update)
	    //subclasses will likely have to call base() ;
        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, Position, spriteRectangle, Color.White, Rotation, origin, 1f, SpriteEffects.None, 0);
        }

        //update the position of the object (this may be empty if the object is static, this is okay)
        public abstract void UpdatePosition(GameTime gameTime) ;

        //checks whether the passed gameobject lies within the boundaries of this GameObject
        public bool IsInside(GameObject obj) {
            if (obj.Left < this.Right && obj.Right > this.Left && obj.Top < this.Bottom && obj.Bottom > this.Top) {
                return true ;
            }
            return false ;
        }
        //checks whether the passed floats (representing a rectangle in non integer space) lie within the boundaries of this game object
        public bool IsInside(float x1, float y1, float x2, float y2) {
            if (x1 < this.Right && x2 > this.Left && y1 < this.Bottom && y2 > this.Top) {
                return true ;
            }
            return false ;
        }

    }
}
