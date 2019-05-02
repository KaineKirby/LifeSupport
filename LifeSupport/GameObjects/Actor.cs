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
using Microsoft.Xna.Framework.Audio;
using Penumbra;

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

        //the passive light source that the actor has
        private PointLight light ;
        
        public Actor(Vector2 position, PenumbraComponent penumbra, int width, int height, int rotation, Texture2D sprite,  Room room, 
            float moveSpeed, float health, float damage, float range, float shotSpeed, float rateOfFire) : base(position, penumbra, width, height, rotation, sprite) {
            //set the passed movespeed
            this.MoveSpeed = moveSpeed ;
            this.CurrentRoom = room ;
            this.Health = health ;
            this.Damage = damage ;
            this.Range = range ;
            this.ShotSpeed = shotSpeed ;
            this.RateOfFire = rateOfFire ;

            this.TimeBeforeShooting = 0f ;

            this.light = new PointLight {
                Position = this.Position,
                Intensity = 1f,
                Scale = new Vector2(100f),
                ShadowType = ShadowType.Occluded
            } ;

            penumbra.Lights.Add(light) ;
           
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
                if (!(obj is Enemy && this is Player) && obj.HasCollision && obj != this && obj.IsInside(newPosition.X-(Width/2), Position.Y-(Height/2), newPosition.X+(Width/2), Position.Y+(Height/2))) {
                    canMoveX = false ;
                }
                //see if we can move JUST in the Y direction
                if (!(obj is Enemy && this is Player) && obj.HasCollision && obj != this && obj.IsInside(Position.X-(Width/2), newPosition.Y-(Height/2), Position.X+(Width/2), newPosition.Y+(Height/2))) {
                    canMoveY = false ;
                }

            }

            if (canMoveX)
                this.Position.X = newPosition.X ;
            if (canMoveY)
                this.Position.Y = newPosition.Y ;

            //update the light's position
            this.light.Position = this.Position ;

        }

        //called when the actor is hit by the passed projectile
        public virtual void OnHit(Projectile proj) {
            Console.WriteLine(this + " hit for " + proj.Damage + " damage") ;
            this.Health -= proj.Damage ;
            //kill it if its health is below 0
            if (this.Health <= 0) {
                CurrentRoom.DestroyObject(this) ;
                CurrentRoom.DestroyObject(proj) ;
                penumbra.Lights.Remove(light) ;

            }
            
        }

        //remove the light from the actor if they are alive and the floor is complete
        public void RemoveLight() {
            penumbra.Lights.Remove(light) ;
        }

        protected virtual void Shoot(Vector2 direction, SoundEffect sound) {
            bool isPlayer = false ;
            if (this is Player) {
                isPlayer = true ;
            }
            //set the time before shooting to rate of fire before counting down
            if(TimeBeforeShooting == 0f) {
                CurrentRoom.AddObject(new Projectile(Position, direction, Damage, ShotSpeed, Range, isPlayer, CurrentRoom, penumbra)) ;
                this.TimeBeforeShooting = RateOfFire ;
                sound.Play((float)(Settings.Instance.SfxVolume/100), 0f, 0f) ;
            }
        }

        //get the position of this actor on the room grid
        public Position GetGridPosition() {
            return new Position((int)((this.Position.Y-CurrentRoom.StartY)/Barrier.WallThickness), (int)((this.Position.X-CurrentRoom.StartX)/Barrier.WallThickness)) ;

        }

    }
}