using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;

namespace LifeSupport.GameObjects
{
    class AlienTurret : Enemy {
        private Player player;
        public AlienTurret(Player p, Vector2 position, PenumbraComponent penumbra, Room room,
            float speed, float health, float damage, float range, float shotSpeed, float rateOfFire) : base(p, position, penumbra, 30, 30, 0, Assets.Instance.alienTurret, room, speed, health, damage, range, shotSpeed, rateOfFire) {
            this.player = p;
        }

        public override void UpdatePosition(GameTime gameTime) {
            base.UpdatePosition(gameTime);
            //only shoot if we have line of sight
            if (this.HasLineOfSight()) {

                Vector2 dir = player.Position - this.Position;
                dir.Normalize();

                //rotating the player relative to the mouse
                this.Rotation = (float)(Math.Atan(dir.Y/dir.X)) ;
                if (dir.X < 0f)
                    this.Rotation -= (float)Math.PI ;

                Shoot(dir, Assets.Instance.alienShot) ;

            }

            //the OnHit requires a projectile so generate a dummy one
            if (this.IsInside(player))
                player.OnHit(new Projectile(Vector2.Zero, Vector2.Zero, Damage, 0, 0, false, CurrentRoom, penumbra));
        }
    }
}
