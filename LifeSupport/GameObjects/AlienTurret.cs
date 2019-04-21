﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeSupport.GameObjects
{
    class AlienTurret : Enemy
    {
        private Player player;
        public AlienTurret(Player p, Vector2 position, Room room,
            float speed, float health, float damage, float range, float shotSpeed, float rateOfFire) : base(p, position, 30, 30, 0, Assets.Instance.alienTurret, room, speed, health, damage, range, shotSpeed, rateOfFire)
        {
            this.player = p;
        }
        

        public override void UpdatePosition(GameTime gameTime)
        {
            base.UpdatePosition(gameTime);
            if (this.HasLineOfSight())
            {
                Vector2 dir = player.Position - this.Position;
                dir.Normalize();
                Shoot(dir, Assets.Instance.alienShot) ;
            }
            
            
            //the OnHit requires a projectile so generate a dummy one
            if (this.IsInside(player))
                player.OnHit(new Projectile(Vector2.Zero, Vector2.Zero, Damage, 0, 0, false, CurrentRoom));
        }
    }
}
