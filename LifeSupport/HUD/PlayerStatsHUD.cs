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
        private HUDString playerSpeed ;
        private HUDImage speedIcon ;
        private HUDImage moneyIcon ;
        private HUDString money ;

        private HUDImage oxygen ;
        private HUDString oxyText ;

        private HUDImage key ;

        public PlayerStatsHUD(Vector2 position, Player player) {
            Vector2 scalar = new Vector2((float)Settings.Instance.Width/1920, (float)Settings.Instance.Height/1080) ;

            this.position = position ;
            this.player = player ;

            this.health = new List<HUDImage>() ;
            for (int i = 0 ; i < 12 ; i++) {
                this.health.Add(new HUDImage(Assets.Instance.healthIcon, (this.position + new Vector2(50, 50))*scalar + new Vector2((i*32), 0))) ;
            }
            this.speedIcon = new HUDImage(Assets.Instance.speedIcon, (this.position + new Vector2(50, 100))*scalar) ;
            this.playerSpeed = new HUDString(player.MoveSpeed.ToString(), Color.White, (this.position + new Vector2(82, 100))*scalar) ;

            this.money = new HUDString(player.Money.ToString(), Color.White, (this.position + new Vector2(82, 150))*scalar) ;
            this.moneyIcon = new HUDImage(Assets.Instance.moneyIcon, (this.position + new Vector2(50, 150))*scalar) ;

            this.oxygen = new HUDImage(Assets.Instance.oxygenTank, (this.position + new Vector2(50, 0))*scalar) ;
            this.oxyText = new HUDString(((int)(player.OxygenTime)).ToString(), Color.White, (this.position + new Vector2(82, 0))*scalar) ;

            this.key = new HUDImage(Assets.Instance.keycard, (this.position + new Vector2(125, 150))*scalar) ;
        }

        public void Update() {
            this.playerSpeed.Update(player.MoveSpeed.ToString()) ;
            this.money.Update(player.Money.ToString()) ;
            this.oxyText.Update(((int)(player.OxygenTime)).ToString()) ;

        }

        public void Draw(SpriteBatch spriteBatch) {

            this.oxygen.Draw(spriteBatch) ;
            this.oxyText.Draw(spriteBatch) ;
            for (int i = 0 ; i < player.Health ; i++) {
                health[i].Draw(spriteBatch) ;
            }
            this.speedIcon.Draw(spriteBatch) ;
            this.playerSpeed.Draw(spriteBatch) ;
            this.moneyIcon.Draw(spriteBatch) ;
            this.money.Draw(spriteBatch) ;
            if (player.HasCard)
                this.key.Draw(spriteBatch) ;
        }


    }
}
