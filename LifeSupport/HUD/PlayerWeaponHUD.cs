using LifeSupport.Config;
using LifeSupport.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.HUD {

    class PlayerWeaponHUD {


        private Vector2 position ;
        private Player player ;

        private HUDString damage ;
        private HUDString rateOfFire ;
        private HUDString shotSpeed ;
        private HUDString range ;

        public PlayerWeaponHUD(Vector2 position, Player player) {
            Vector2 scalar = new Vector2((float)Settings.Instance.Width/1920, (float)Settings.Instance.Height/1080) ;

            this.position = position ;
            this.player = player ;

            this.damage = new HUDString("Damage: " + player.Damage, Color.White, (this.position + new Vector2(50, 50))*scalar) ;
            this.rateOfFire = new HUDString("ROF: " + player.RateOfFire, Color.White, (this.position + new Vector2(50, 100))*scalar) ;
            this.shotSpeed = new HUDString("Shot Speed: " + player.ShotSpeed, Color.White, (this.position + new Vector2(50, 150))*scalar) ;
            this.range = new HUDString("Range: " + player.Range, Color.White, (this.position + new Vector2(50, 200))*scalar) ;
        }

        public void Update() {
            this.damage.Update("Damage: " + player.Damage) ;
            this.rateOfFire.Update("ROF: " + player.RateOfFire) ;
            this.shotSpeed.Update("Shot Speed: " + player.ShotSpeed) ;
            this.range.Update("Range: " + player.Range) ;
        }

        public void Draw(SpriteBatch spriteBatch) {
            this.damage.Draw(spriteBatch) ;
            this.rateOfFire.Draw(spriteBatch) ;
            this.shotSpeed.Draw(spriteBatch) ;
            this.range.Draw(spriteBatch) ;
        }

    }

}
