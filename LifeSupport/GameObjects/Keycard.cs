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

    class Keycard : Drop {

        private Level level ;

        public Keycard(Vector2 position, Player player, Room room, Level level) : base(position, 30, 30, Assets.Instance.keycard, player, room) {

            this.level = level ;

        }

        public override void OnPickup() {
            player.HasCard = true ;
            //open all the doors when we pickup the keycard
            level.ChallengeRoom.OpenAllDoors() ;
            Room left = level.GetRoomAtCoordinate(level.ChallengeRoom.coordinate + new Point(-1, 0)) ;
            Room right = level.GetRoomAtCoordinate(level.ChallengeRoom.coordinate + new Point(1, 0)) ;
            Room top = level.GetRoomAtCoordinate(level.ChallengeRoom.coordinate + new Point(0, -1)) ;
            Room bot = level.GetRoomAtCoordinate(level.ChallengeRoom.coordinate + new Point(0, 1)) ;


            if (left != null) 
                left.OpenAllDoors() ;
            if (right != null) 
                right.OpenAllDoors() ;

            if (top != null) 
                top.OpenAllDoors() ;
            if (bot != null) 
                bot.OpenAllDoors() ;

            Assets.Instance.keycardPickup.Play((float)Settings.Instance.SfxVolume/100, 0f, 0f) ;
            //call base to delete the keycard from the room
            base.OnPickup() ;
        }

    }
}
