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

    class Player : Actor
    {

        //the controller instance since the player will be manipulated with controls
        private readonly Controller controller;

        private static readonly float startPlayerSpeed = 500f ;

        //this is for animating the legs
        private Texture2D playerLegs ;
        private int animFrame ;
        private float timer ; //every timer seconds, we change frames in the animation
        private int legRotation ;
        private float time ;

        //will probably be constant
        public Player(Room startingRoom) : base(new Rectangle(100, 100, 32, 32), 0, Assets.Instance.player, startingRoom, startPlayerSpeed)
        {

            this.controller = Controller.Instance;
            this.Damage = 1.0f ;
            this.Range = 1000f ;
            this.ShotSpeed = 1000f ;
            this.GunBarrelPosition = new Point(960, 540) ;
            this.RateOfFire = .8f ;

            this.playerLegs = Assets.Instance.playerLegs ;
            this.timer = .2f ;
            this.time = 0f ;
            this.legRotation = 0 ;
            this.animFrame = 1 ;

        }

        //constructor
        public Player(Game game, Room startingRoom) : base(new Rectangle(100, 100, 32, 32), 0, Assets.Instance.player, startingRoom, 500f)
        {

            this.controller = Controller.Instance;
            this.GunBarrelPosition = new Point(960, 540) ;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(playerLegs, new Rectangle(Rect.X, Rect.Y, 32, 32), new Rectangle(Rect.Width*animFrame,0,32,32), Color.White, legRotation, new Vector2(16, 16), SpriteEffects.None, 0) ;
            Console.WriteLine(legRotation) ;
            base.Draw(spriteBatch) ;
        }

        //use the controller class to update the positions
        public new void UpdatePosition(GameTime gameTime) { 

            //increment the timer every update if the movement vector isn't 0
            if (!MoveDirection.Equals(Vector2.Zero)) {
                this.time += (float)gameTime.ElapsedGameTime.TotalSeconds ;
                if (time >= timer) {
                    time = 0 ;
                    animFrame = (animFrame+1) % (playerLegs.Width/Rect.Width) + 1 ;
                }
                this.legRotation = (int)(Math.Atan(MoveDirection.Y/MoveDirection.X)*180/Math.PI) ;
            }

            //update the player's rotationdepending on where the cursor is
            //can't figure this out right now

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

        }

        //shoots a projectile in the current room
        protected override void Shoot() {
            CurrentRoom.AddObject(new Projectile(new Point(Rect.X+8, Rect.Y+8), Cursor.Instance.GetDirection(GunBarrelPosition), Damage, ShotSpeed, Range, true, CurrentRoom)) ;
            //call the base shoot to restrict firing
            base.Shoot() ;
        }
    }
}
