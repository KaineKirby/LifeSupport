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

        public float Damage ;
        public float Range ;
        public float ShotSpeed ;

        //where the projectiles come out
        private Point GunBarrelPosition ;

        //will probably be constant
        public Player(Room startingRoom) : base(new Rectangle(100, 100, 32, 32), 0, Assets.Instance.player, startingRoom, startPlayerSpeed)
        {

            this.controller = Controller.Instance;
            this.Damage = 1.0f ;
            this.Range = 1000f ;
            this.ShotSpeed = 1000f ;
            this.GunBarrelPosition = new Point(960, 540) ;

        }

        //constructor
        public Player(Game game, Room startingRoom) : base(new Rectangle(100, 100, 32, 32), 0, Assets.Instance.player, startingRoom, 500f)
        {

            this.controller = Controller.Instance;
            this.GunBarrelPosition = new Point(960, 540) ;
        }

        //use the controller class to update the positions
        public new void UpdatePosition(GameTime gameTime) {

            if (Cursor.Instance.IsLeftMouseDown())
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

        }

        //shoots a projectile in the current room
        private void Shoot() {

            CurrentRoom.AddObject(new Projectile(new Point(Rect.X+16, Rect.Y+16), Cursor.Instance.GetDirection(GunBarrelPosition), Damage, ShotSpeed, Range, CurrentRoom)) ;

        }
    }
}
