using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config ;
using Microsoft.Xna.Framework;

/*
* Actor Class (Abstract)
* 
* This is an extension of the game object, except it is exclusive to things that move
* (player, enemies)
* 
* moveSpeed updating is built into the class that can support screen scaling
* a distinction must be made between x and y move speed because of scaling, update method built in to help with this
*/

namespace LifeSupport.GameObject {

    abstract class Actor : GameObject {

        //move speed of the actor
        public float MoveSpeed ;
        //the direction the actor is moving in at a particular moment (Vector2)
        private Vector2 MoveDirection ; //enemies may need to see the player direction and this has to change

        public Actor(int xPos, int yPos, int width, int height, int rotation, String spritePath, float moveSpeed) : base(xPos, yPos, width, height, rotation, spritePath) {
            //set the passed movespeed
            this.MoveSpeed = moveSpeed ;
        }

        //updates the direction of the actor
        protected void UpdateDirection(Vector2 vector) {
            this.MoveDirection = vector ;
            this.MoveDirection.Normalize() ;
        }

        //updates the position based on the current vector (not necessarily called every update)
        public override void UpdatePosition(GameTime gameTime) {
            this.XPos += (MoveDirection * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds).X ;
            this.YPos += (MoveDirection * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds).Y ;

        }

    }
}
