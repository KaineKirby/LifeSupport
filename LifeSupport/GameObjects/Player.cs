using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config ;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/*
* Player Class (Singleton)
* This is the class that represents the player
*/

namespace LifeSupport.GameObjects {

   public class Player : Actor
    {
        //the controller instance since the player will be manipulated with controls
        private readonly Controller controller;

        private static readonly float startPlayerSpeed = 500f ;

        //animation stuff
        private Texture2D playerLegs ;
        private int animFrame ; //the current frame of animation
        private float timer ; //the time between animation frames
        private float time ; //the current time since last animation frame
        private int legRotation ;
        private Vector2 legOrigin ;

        //will probably be constant
        public Player() : base(new Vector2(100, 100), 32, 32, 0, Assets.Instance.player, null, startPlayerSpeed) {

            this.controller = Controller.Instance;
            this.Damage = 1.0f ;
            this.Range = 1000f ;
            this.ShotSpeed = 1000f ;
            this.GunBarrelPosition = new Point(960, 540) ;
            this.RateOfFire = .2f ;

            //animation
            this.animFrame = 0 ;
            this.timer = .05f ;
            this.time = 0;
            this.playerLegs = Assets.Instance.playerLegs ;
            this.legOrigin = new Vector2(Width/2, Height/2) ;
            this.legRotation = 0 ;

        }


        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(playerLegs, Position, new Rectangle(animFrame*Width, 0, Width, Height), Color.White, legRotation, legOrigin, 1f, SpriteEffects.None, 0);
            base.Draw(spriteBatch) ;
        }

        //use the controller class to update the positions
        public new void UpdatePosition(GameTime gameTime) {

            if (Cursor.Instance.IsLeftMouseDown() && TimeBeforeShooting == 0f)
                Shoot() ;

            //on the various vectors
            if (controller.IsMovingUp() && controller.IsMovingRight()) {
                UpdateDirection(new Vector2(1, -1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingUp() && controller.IsMovingLeft()) {
                UpdateDirection(new Vector2(-1, -1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingDown() && controller.IsMovingRight()) {
                UpdateDirection(new Vector2(1, 1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingDown() && controller.IsMovingLeft()) {
                UpdateDirection(new Vector2(-1, 1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingUp()) {
                UpdateDirection(new Vector2(0, -1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingDown()) {
                UpdateDirection(new Vector2(0, 1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingLeft()) {
                UpdateDirection(new Vector2(-1, 0)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsMovingRight()) {
                UpdateDirection(new Vector2(1, 0)) ;
                base.UpdatePosition(gameTime) ;
            }
            //if not make the player not move by giving a zero vector
            else {
                UpdateDirection(new Vector2(0, 0)) ;
                base.UpdatePosition(gameTime) ;
            }

            //for animation
            if (!MoveDirection.Equals(Vector2.Zero)) {
                this.time += (float)gameTime.ElapsedGameTime.TotalSeconds ;
                this.legRotation = (int)(Math.Atan(MoveDirection.Y/MoveDirection.X)*180/Math.PI) ;
            }
            else {
                animFrame = 8 ;
            }
                
            
            if (time >= timer) {
                time = 0 ;
                animFrame = (animFrame+1)%(playerLegs.Width/(Width)) ;
            }

        }

        //shoots a projectile in the current room
        protected override void Shoot() {
            CurrentRoom.AddObject(new Projectile(Position, Cursor.Instance.GetDirection(GunBarrelPosition), Damage, ShotSpeed, Range, true, CurrentRoom)) ;
            //call the base shoot to restrict firing
            base.Shoot() ;
        }
    }
}
