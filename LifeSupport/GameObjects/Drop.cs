using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeSupport.GameObjects {
    abstract class Drop : GameObject {

        protected Player player ;
        protected Room room ;

        public Drop(Vector2 position, int width, int height, Texture2D sprite, Player player, Room room) : base(position, width, height, 0, sprite, false) {

            this.player = player ;
            this.room = room ;

        }

        //check to see whether the player is on top of the pickup
        public override void UpdatePosition(GameTime gameTime) {
            if (player.IsInside(this)) {
                OnPickup() ;
            }
        }

        //when the player is on top of the drop, we call this method
        public virtual void OnPickup() {
            room.DestroyObject(this) ;
        }

    }
}
