using LifeSupport.Config;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoyT.AStar;

namespace LifeSupport.GameObjects
{
  public  class AlienDog : Enemy {

        private Player player;

        public AlienDog(Player p, Vector2 position, Room room, 
            float speed, float health, float damage) : base(p, position, 30, 30, 0, Assets.Instance.alienDog, room, speed, health, damage, 0, 0, 0) {
            this.player = p;
        }

        public override void UpdatePosition(GameTime gameTime) {

            //the OnHit requires a projectile so generate a dummy one
            if (this.IsInside(player))
                player.OnHit(new Projectile(Vector2.Zero, Vector2.Zero, Damage, 0, 0, false, CurrentRoom));

            base.UpdatePosition(gameTime);
        }

    }
}
