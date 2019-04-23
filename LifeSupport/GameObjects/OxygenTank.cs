using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeSupport.GameObjects {

    class OxygenTank : GameObject {

        private Level level ;
        private Player player ;
        private Room room ;

        private int UseRadius = 30 ;


        public OxygenTank(Vector2 position, Level level, Room room, Player player) : base(position, null, 30, 30, 0, Assets.Instance.oxygenTank) {

            this.level = level ;
            this.player = player ;
            this.room = room ;

        }

        public override void UpdatePosition(GameTime gameTime) {

            //if the player is inside the use radius and hits the use button
            if (player.IsInside(Position.X-UseRadius, Position.Y-UseRadius, Position.X+UseRadius, Position.Y+UseRadius) 
                && Controller.Instance.IsKeyDown(Controller.Instance.Use) &&
                room.IsBeaten) {
                //progress level
                level.NextLevel() ;
            }
            
        }
    }
}
