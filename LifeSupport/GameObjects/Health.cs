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

    class Health : Drop {

        //this class represents a health drop

        public Health(Vector2 position, Player player, Room room) : base(position, 30, 30, Assets.Instance.healthIcon, player, room) {}

        public override void OnPickup() {
            //give the player a health when they walk over
            player.Health += 1f ;
            Assets.Instance.healthPickup.Play((float)Settings.Instance.SfxVolume/100, 0f, 0f) ;
            base.OnPickup() ;
        }
    }
}
