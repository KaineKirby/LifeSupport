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

        public Health(Vector2 position, Player player, Room room) : base(position, 30, 30, Assets.Instance.healthIcon, player, room) {}

        public override void OnPickup() {
            player.Health += 1f ;
            base.OnPickup() ;
        }
    }
}
