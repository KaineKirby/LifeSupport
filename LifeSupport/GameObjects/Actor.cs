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
* 
* it should be noted that ALL actors have collision detection
*/

namespace LifeSupport.GameObjects {

    abstract class Actor : GameObject {

        //move speed of the actor
        public float MoveSpeed ;
        //the direction the actor is moving in at a particular moment (Vector2)
        private Vector2 MoveDirection ; //enemies may need to see the player direction and this has to change

        public Actor(int xPos, int yPos, int width, int height, int rotation, String spritePath, Game game, float moveSpeed) : base(xPos, yPos, width, height, rotation, spritePath, game) {
            //set the passed movespeed
            this.MoveSpeed = moveSpeed ;
        }

        //updates the direction of the actor
        protected void UpdateDirection(Vector2 vector) {
            this.MoveDirection = vector ;
            this.MoveDirection.Normalize() ;
        }

        public override void UpdatePosition(GameTime gameTime, GameObject[] objects) {

            float x = this.XPos + (MoveDirection * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds).X ;
            float y = this.YPos + (MoveDirection * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds).Y ;

            bool hasCollided = false ;

            //TODO collision detection is not perfect, it works but there is a scenario i want to make work
            //when the player is going diagonal towards a collidable surface they should be able to apply the force from their diagonal vector in the perpendicular direction of the collision
            for (int i = 0 ; i < objects.Length ; i++) {
                if (x < objects[i].XPos+objects[i].Width && x+this.Width > objects[i].XPos && 
                    y < objects[i].YPos+objects[i].Height && y+this.Height > objects[i].YPos) {
                    hasCollided = true ;
                }
            }

            if (!hasCollided) {
                this.XPos = x ;
                this.YPos = y ;
            }

        }

    }
}
