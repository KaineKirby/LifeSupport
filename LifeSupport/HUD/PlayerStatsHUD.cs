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

    class PlayerStatsHUD {

        private Vector2 position ;
        private Player player ;

        private List<HUDImage> health ;
        //private HUDString health ;
        private HUDString playerSpeed ;
        private HUDImage speedIcon ;

        public PlayerStatsHUD(Vector2 position, Player player) {
            Vector2 scalar = new Vector2((float)Settings.Instance.Width/1920, (float)Settings.Instance.Height/1080) ;

            this.position = position ;
            this.player = player ;

            //this.health = new HUDString("Health: " + player.Health, Color.White, (this.position + new Vector2(50, 50))*scalar) ;
            this.health = new List<HUDImage>() ;
            for (int i = 0 ; i < 12 ; i++) {
                this.health.Add(new HUDImage(Assets.Instance.healthIcon, (this.position + new Vector2(50, 50))*scalar + new Vector2((i*32), 0))) ;
            }
            this.speedIcon = new HUDImage(Assets.Instance.speedIcon, (this.position + new Vector2(50, 100))*scalar) ;
            this.playerSpeed = new HUDString(player.MoveSpeed.ToString(), Color.White, (this.position + new Vector2(82, 100))*scalar) ;
        }

        public void Update() {
            //this.health.Update("Health: " + player.Health) ;
            this.playerSpeed.Update(player.MoveSpeed.ToString()) ;

        }

        public void Draw(SpriteBatch spriteBatch) {
            for (int i = 0 ; i < player.Health ; i++) {
                health[i].Draw(spriteBatch) ;
            }
            this.speedIcon.Draw(spriteBatch) ;
            this.playerSpeed.Draw(spriteBatch) ;
        }


    }
}
