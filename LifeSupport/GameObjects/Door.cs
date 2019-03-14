using LifeSupport.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.GameObjects {

    class Door : GameObject {

        public bool IsOpen ;

        private Texture2D openTexture ;
        private Texture2D closeTexture ;

        //when a rotation is not passed we assume 0 
        public Door(int xPos, int yPos) : base(new Rectangle(xPos, yPos, 32, 64), 0, Assets.Instance.closeDoor) {
            IsOpen = false ;

            openTexture = Assets.Instance.openDoor ;
            closeTexture = Assets.Instance.closeDoor ;
        }

        //otherwise we use the passed rotation
        public Door(int xPos, int yPos, int rotation) : base(new Rectangle(xPos, yPos, 32, 64), rotation, Assets.Instance.closeDoor) {
            IsOpen = false ;

            openTexture = Assets.Instance.openDoor ;
            closeTexture = Assets.Instance.closeDoor ;
        }

        //open the door if it is not already open
        public void OpenDoor() {
            this.sprite = openTexture ;
            IsOpen = true ;
            this.HasCollision = false ; 
        }

        public void CloseDoor() {
            this.sprite = closeTexture ;
            IsOpen = false ;
            this.HasCollision = true ;
        }

        public override void UpdatePosition(GameTime gameTime) {
            //NOTE temporarily the doors can be opened/closed with L for testing
            if (Controller.Instance.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.L)) {
                if (IsOpen)
                    CloseDoor() ;
                else
                    OpenDoor() ;
            }
        }




    }
}
