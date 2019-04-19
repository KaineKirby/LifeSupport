using LifeSupport.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.HUD {

    class PlayerHUD {

        private Vector2 position ;
        private Player player ;

        private HUDElement health ;
        private HUDElement damage ;
        private HUDElement rateOfFire ;
        private HUDElement shotSpeed ;
        private HUDElement range ;
        private HUDElement playerSpeed ;

        public PlayerHUD(Vector2 position, Player player) {
            this.position = position ;
            this.player = player ;

            this.health = new HUDElement("Health: " + player.Health, Color.White, this.position + new Vector2(50, 50)) ;
            this.damage = new HUDElement("Damage: " + player.Damage, Color.White, this.position + new Vector2(50, 100)) ;
            this.rateOfFire = new HUDElement("ROF: " + player.RateOfFire, Color.White, this.position + new Vector2(50, 150)) ;
            this.shotSpeed = new HUDElement("Shot Speed: " + player.ShotSpeed, Color.White, this.position + new Vector2(50, 200)) ;
            this.range = new HUDElement("Range: " + player.Range, Color.White, this.position + new Vector2(50, 250)) ;
            this.playerSpeed = new HUDElement("Speed: " + player.MoveSpeed, Color.White, this.position + new Vector2(50, 300)) ;
        }

        public void Update() {
            this.health.Update("Health: " + player.Health) ;
            this.damage.Update("Damage: " + player.Damage) ;
            this.rateOfFire.Update("ROF: " + player.RateOfFire) ;
            this.shotSpeed.Update("Shot Speed: " + player.ShotSpeed) ;
            this.range.Update("Range: " + player.Range) ;
            this.playerSpeed.Update("Speed: " + player.MoveSpeed) ;

        }

        public void Draw(SpriteBatch spriteBatch) {
            this.health.Draw(spriteBatch) ;
            this.damage.Draw(spriteBatch) ;
            this.rateOfFire.Draw(spriteBatch) ;
            this.shotSpeed.Draw(spriteBatch) ;
            this.range.Draw(spriteBatch) ;
            this.playerSpeed.Draw(spriteBatch) ;
        }


    }
}
