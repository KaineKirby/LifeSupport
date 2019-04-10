using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config ;
using Microsoft.Xna.Framework;
using System.Collections;
using LifeSupport.Levels;
using RoyT.AStar;

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

   public abstract class Actor : GameObject {

        //move speed of the actor
        public float MoveSpeed ;
        //the direction the actor is moving in at a particular moment (Vector2)
        protected Vector2 MoveDirection ; //enemies may need to see the player direction and this has to change
        public Room CurrentRoom ;

        //the stats of the actor
        public float Health ;
        public float Damage ;
        public float Range ;
        public float ShotSpeed ;
        public float RateOfFire ;
        //the time since the last shot was fired and whether or not they can shoot
        protected float TimeBeforeShooting ;
        
        //where the projectiles come out
        protected Point GunBarrelPosition ;
        
        public Actor(Vector2 position, int width, int height, int rotation, Texture2D sprite,  Room room, float moveSpeed) : base(position, width, height, rotation, sprite) {
            //set the passed movespeed
            this.MoveSpeed = moveSpeed ;
            this.CurrentRoom = room ;

            this.TimeBeforeShooting = 0f ;
           
        }

        //updates the direction of the actor
        protected virtual void UpdateDirection(Vector2 vector) {
            this.MoveDirection = vector ;
            //only normalize vector if it isnt the zero vector
            if (!vector.Equals(Vector2.Zero))
                this.MoveDirection.Normalize() ;
        }


        public override void UpdatePosition(GameTime gameTime) {

            //update the time allowed before firing
            if (TimeBeforeShooting > 0f)
                TimeBeforeShooting -= (float)gameTime.ElapsedGameTime.TotalSeconds ;
            else 
                TimeBeforeShooting = 0f ;

            //move the actor
            Vector2 newPosition = this.Position + (MoveDirection * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) ;

            bool canMoveX = true ;
            bool canMoveY = true ;

            foreach (GameObject obj in CurrentRoom.Objects) {

                //see if we can update JUST the X direction
                if (!(obj is Enemy) && obj.HasCollision && obj != this && obj.IsInside(newPosition.X-(Width/2), Position.Y-(Height/2), newPosition.X+(Width/2), Position.Y+(Height/2))) {
                    canMoveX = false ;
                }
                //see if we can move JUST in the Y direction
                if (!(obj is Enemy) && obj.HasCollision && obj != this && obj.IsInside(Position.X-(Width/2), newPosition.Y-(Height/2), Position.X+(Width/2), newPosition.Y+(Height/2))) {
                    canMoveY = false ;
                }

            }

            if (canMoveX)
                this.Position.X = newPosition.X ;
            if (canMoveY)
                this.Position.Y = newPosition.Y ;

        }

        //called when the actor is hit by the passed projectile
        public virtual void Hit(Projectile proj) {
            this.Health -= proj.Damage ;
            
        }

        protected virtual void Shoot() {
            //set the time before shooting to rate of fire before counting down
            this.TimeBeforeShooting = RateOfFire ;
        }

        //get the position of this actor on the room grid
        public Position GetGridPosition() {
            return new Position((int)((this.Position.Y-CurrentRoom.StartY)/Barrier.WallThickness), (int)((this.Position.X-CurrentRoom.StartX)/Barrier.WallThickness)) ;

        }

    }
}