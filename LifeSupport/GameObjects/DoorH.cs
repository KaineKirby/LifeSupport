using LifeSupport.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
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
        public DoorH(Vector2 position, PenumbraComponent penumbra) : base(position, penumbra, 60, 30, 0, Assets.Instance.closeDoorH) {
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


        }



    }
}
