using LifeSupport.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.GameObjects {
    class DoorH : GameObject, Door {

        public bool IsOpen ;

        private Texture2D openTexture ;
        private Texture2D closeTexture ;

        //when a rotation is not passed we assume 0 
        public DoorH(Vector2 position) : base(position, 60, 30, 0, Assets.Instance.closeDoorH) {
            IsOpen = false ;

            this.Position = position + new Vector2(Width/2, Height/2) ;

            openTexture = Assets.Instance.openDoorH ;
            closeTexture = Assets.Instance.closeDoorH ;
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
