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

        //the player stats hud contains all the player's stats at a glance

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

            this.position = position ;
            this.player = player ;
            
            //we require health icons, money and oxygen icons
            //then the strings for each value

            this.health = new List<HUDImage>() ;
            //we can only show up to 32 hearts at once
            for (int i = 0 ; i < 32 ; i++) {
                this.health.Add(new HUDImage(Assets.Instance.healthIcon, (this.position + new Vector2(50, 50)) + new Vector2((i*32), 0))) ;
            }
            this.speedIcon = new HUDImage(Assets.Instance.speedIcon, (this.position + new Vector2(50, 100))) ;
            this.playerSpeed = new HUDString(player.MoveSpeed.ToString(), Color.White, (this.position + new Vector2(82, 100))) ;

            this.money = new HUDString(player.Money.ToString(), Color.White, (this.position + new Vector2(82, 150))) ;
            this.moneyIcon = new HUDImage(Assets.Instance.moneyIcon, (this.position + new Vector2(50, 150))) ;

            this.oxygen = new HUDImage(Assets.Instance.oxygenIcon, (this.position + new Vector2(50, 0))) ;
            this.oxyText = new HUDString(((int)(player.OxygenTime)).ToString(), Color.White, (this.position + new Vector2(82, 0))) ;

            this.key = new HUDImage(Assets.Instance.keycard, (this.position + new Vector2(125, 150))) ;
        }

        public void Update() {
            //update the values
            this.playerSpeed.Update(player.MoveSpeed.ToString()) ;
            this.money.Update(player.Money.ToString()) ;
            this.oxyText.Update(((int)(player.OxygenTime)).ToString()) ;

        }

        public void Draw(SpriteBatch spriteBatch) {

            //draw everything
            this.oxygen.Draw(spriteBatch) ;
            this.oxyText.Draw(spriteBatch) ;
            //we can only show up to 32 hearts at once
            for (int i = 0 ; i < player.Health && i < health.Count ; i++) {
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
