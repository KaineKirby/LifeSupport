using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config ;
using LifeSupport.GameObject ;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
* Player Class (Singleton)
* This is the class that represents the player
*/

namespace LifeSupport.GameObject {

    class Player : Actor {
        
        private static Player instance ;
        public static Player Instance {
            get {
                if (instance != null)
                    return instance ;
                else 
                    return new Player() ;
            }
            private set {
                instance = value ;
            }
        }

        //the controller instance since the player will be manipulated with controls
        private readonly Controller controller ;

        //will probably be constant
        private Player() : base(100, 100, 32, 32, 0, "img/player/player", 200f) {

            this.controller = Controller.Instance ;

        }
        
        //use the controller class to update the positions
        public override void UpdatePosition(GameTime gameTime) {

            //on the various vectors
            if (controller.IsKeyDown(controller.MoveUp) && controller.IsKeyDown(controller.MoveRight)) {
                UpdateDirection(new Vector2(1, -1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsKeyDown(controller.MoveUp) && controller.IsKeyDown(controller.MoveLeft)) {
                UpdateDirection(new Vector2(-1, -1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsKeyDown(controller.MoveDown) && controller.IsKeyDown(controller.MoveRight)) {
                UpdateDirection(new Vector2(1, 1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsKeyDown(controller.MoveDown) && controller.IsKeyDown(controller.MoveLeft)) {
                UpdateDirection(new Vector2(-1, 1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsKeyDown(controller.MoveUp)) {
                UpdateDirection(new Vector2(0, -1)) ;
                base.UpdatePosition(gameTime) ;
            }
            else if (controller.IsKeyDown(controller.MoveDown)) {
                UpdateDirection(new Vector2(0, 1)) ;
                base.UpdatePosition(gameTime) ;
            }  
            else if (controller.IsKeyDown(controller.MoveLeft)) {
                UpdateDirection(new Vector2(-1, 0)) ;
                base.UpdatePosition(gameTime) ;
            }  
            else if (controller.IsKeyDown(controller.MoveRight)) {
                UpdateDirection(new Vector2(1, 0)) ;
                base.UpdatePosition(gameTime) ;
            }


        }







    }

}

