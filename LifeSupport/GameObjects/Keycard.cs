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
            //open all the doors when we pickup the keycard
            level.ChallengeRoom.OpenAllDoors() ;
            player.HasCard = true ;
            Assets.Instance.keycardPickup.Play((float)Settings.Instance.SfxVolume/100, 0f, 0f) ;
            //call base to delete the keycard from the room
            base.OnPickup() ;
        }

    }
}
