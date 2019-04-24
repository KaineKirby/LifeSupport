using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using LifeSupport.Controls;
using LifeSupport.GameObjects;
using LifeSupport.HUD;
using LifeSupport.States.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LifeSupport.States {
    class AugmentationStationState : State {

        private MainGame game ;
        private GraphicsDevice graphicsDevice ;
        private GameState gameState ;
        private Player player ;

        private AugmentationStation station ;

        private int moneyInMachine ;

        //the controls on the interface
        private List<Component> buttons ;
        private List<AugmentSlot> playerAugments ;
        private HUDString machineMoney ;
        private HUDImage moneyIcon ;
        private HUDString outputLabel ;

        private AugmentSlot generatedSlot ;

        public AugmentationStationState(MainGame game, GraphicsDevice graphicsDevice, ContentManager state_content, GameState gameState, Player player, AugmentationStation station) : base(game, graphicsDevice, state_content) {
            this.game = game ;
            this.graphicsDevice = graphicsDevice ;
            this.gameState = gameState ;
            this.player = player ;
            this.station = station ;

            buttons = new List<Component>() ;
            playerAugments = new List<AugmentSlot>() ;
            moneyInMachine = 0 ;

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg) {
            
            bg.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920)) ;
            bg.Draw(Assets.Instance.augmentMenuScreen, new Rectangle(0, 0, 1920, 1080), Color.White) ;
            bg.End() ;

            hud.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920)) ;

            foreach (Component c in buttons)
                c.Draw(gameTime, hud) ;

            foreach (AugmentSlot c in playerAugments)
                c.Draw(gameTime, hud) ;

            machineMoney.Draw(hud) ;
            moneyIcon.Draw(hud) ;
            outputLabel.Draw(hud) ;

            hud.End() ;
        }

        public override void Load() {

            game.IsMouseVisible = true ;

            int xAnchor = 200 ;
            int yAnchor = 550 ;
            
            Button resume = new Button(Assets.Instance.btnTextureLarge, Assets.Instance.btnText) {
                CurrPosition = new Vector2(1410, 75),
                BtnText = "Resume Game"
            } ;
            resume.Click += ResumeGame ;
            buttons.Add(resume) ;

            Button inc = new Button(Assets.Instance.btnTextureSmall, Assets.Instance.btnText) {
                CurrPosition = new Vector2(xAnchor+325, yAnchor),
                BtnText = "+"
            } ;
            inc.Click += IncrementMoney ;
            buttons.Add(inc) ;

            Button dec = new Button(Assets.Instance.btnTextureSmall, Assets.Instance.btnText) {
                CurrPosition = new Vector2(xAnchor, yAnchor),
                BtnText = "-"
            } ;
            dec.Click += DecrementMoney ;
            buttons.Add(dec) ;

            Button gen = new Button(Assets.Instance.btnTextureLarge, Assets.Instance.btnText) {
                CurrPosition = new Vector2(xAnchor, yAnchor+150),
                BtnText = "Create Augment"
            } ;
            gen.Click += GenerateAugment ;
            buttons.Add(gen) ;

            machineMoney = new HUDString(Assets.Instance.largeText, moneyInMachine.ToString(), Color.White, new Vector2(xAnchor+200, yAnchor+20)) ;
            moneyIcon = new HUDImage(Assets.Instance.moneyLarge, new Vector2(xAnchor+100, yAnchor)) ;

            outputLabel = new HUDString(Assets.Instance.largeText, "Output", Color.White, new Vector2(xAnchor+600, yAnchor)) ;
            AugmentTextBox generatedSlotBox = new AugmentTextBox(station.Augment, game.GraphicsDevice) ;
            generatedSlot = new AugmentSlot(Assets.Instance.btnTextureMedium, generatedSlotBox, station.Augment) {
                CurrPosition = new Vector2(xAnchor+600, yAnchor+50)
            } ;

            generatedSlot.Click += EquipAugment ;

            buttons.Add(generatedSlot) ;

            int xAnchorP = 1240 ;
            int yAnchorP = 370 ;

            //player inventory augments
            AugmentTextBox hoverBox0 = new AugmentTextBox(player.Augments[0], game.GraphicsDevice);
            AugmentSlot augmentSlot0 = new AugmentSlot(Assets.Instance.btnTextureMedium, hoverBox0, player.Augments[0])
            {
                CurrPosition = new Vector2(xAnchorP, yAnchorP)
            };
            playerAugments.Add(augmentSlot0) ;


            AugmentTextBox hoverBox1 = new AugmentTextBox(player.Augments[1], game.GraphicsDevice);
            AugmentSlot augmentSlot1 = new AugmentSlot(Assets.Instance.btnTextureMedium, hoverBox1, player.Augments[1])
            {
                CurrPosition = new Vector2(xAnchorP+300, yAnchorP)
            };
            playerAugments.Add(augmentSlot1) ;


            AugmentTextBox hoverBox2 = new AugmentTextBox(player.Augments[2], game.GraphicsDevice);
            AugmentSlot augmentSlot2 = new AugmentSlot(Assets.Instance.btnTextureMedium, hoverBox2, player.Augments[2])
            {
                CurrPosition = new Vector2(xAnchorP, yAnchorP+160)
            };
            playerAugments.Add(augmentSlot2) ;


            AugmentTextBox hoverBox3 = new AugmentTextBox(player.Augments[3], game.GraphicsDevice);
            AugmentSlot augmentSlot3 = new AugmentSlot(Assets.Instance.btnTextureMedium, hoverBox3, player.Augments[3])
            {
                CurrPosition = new Vector2(xAnchorP+300, yAnchorP+160)
            };
            playerAugments.Add(augmentSlot3) ;


            AugmentTextBox hoverBox4 = new AugmentTextBox(player.Augments[4], game.GraphicsDevice);
            AugmentSlot augmentSlot4 = new AugmentSlot(Assets.Instance.btnTextureMedium, hoverBox4, player.Augments[4])
            {
                CurrPosition = new Vector2(xAnchorP, yAnchorP+320)
            };
            playerAugments.Add(augmentSlot4) ;


            AugmentTextBox hoverBox5 = new AugmentTextBox(player.Augments[5], game.GraphicsDevice);
            AugmentSlot augmentSlot5 = new AugmentSlot(Assets.Instance.btnTextureMedium, hoverBox5, player.Augments[5])
            {
                CurrPosition = new Vector2(xAnchorP+300, yAnchorP+320)
            };
            playerAugments.Add(augmentSlot5) ;


            AugmentTextBox hoverBox6 = new AugmentTextBox(player.Augments[6], game.GraphicsDevice);
            AugmentSlot augmentSlot6 = new AugmentSlot(Assets.Instance.btnTextureMedium, hoverBox6, player.Augments[6])
            {
                CurrPosition = new Vector2(xAnchorP, yAnchorP+480)
            };
            playerAugments.Add(augmentSlot6) ;

            AugmentTextBox hoverBox7 = new AugmentTextBox(player.Augments[7], game.GraphicsDevice);
            AugmentSlot augmentSlot7 = new AugmentSlot(Assets.Instance.btnTextureMedium, hoverBox7, player.Augments[7])
            {
                CurrPosition = new Vector2(xAnchorP+300, yAnchorP+480)
            };
            playerAugments.Add(augmentSlot7) ;

        }

        public override void PostUpdate(GameTime gameTime) {

        }

        public override void Update(GameTime gameTime) {
            foreach (Component c in buttons)
                c.Update(gameTime) ;

            foreach (AugmentSlot a in playerAugments)
                a.Update(gameTime) ;

            machineMoney.Update(moneyInMachine + "/" + (player.Money-moneyInMachine)) ;
        }

        private void ResumeGame(object sender, EventArgs e) {
            game.returnToGame(gameState) ;
            game.IsMouseVisible = false ;
        }

        private void IncrementMoney(object sender, EventArgs e) {
            if (this.moneyInMachine < player.Money) {
                moneyInMachine++ ;
            }
        }

        private void DecrementMoney(object sender, EventArgs e) {
            if (moneyInMachine > 0)
                moneyInMachine-- ;
        }

        private void GenerateAugment(object sender, EventArgs e) {
            if (moneyInMachine <= 0)
                return ;
            station.Augment = AugmentationStation.GenerateAugment(moneyInMachine) ;
            generatedSlot.UpdateAugment(station.Augment) ;
            player.Money -= moneyInMachine ;
            moneyInMachine = 0 ;
        }

        private void EquipAugment(object sender, EventArgs e) {
            if (station.Augment == null)
                return ;
            player.AddAugment(station.Augment, player.SearchForNextAvailableSpot()) ;
            station.Augment = null ;
            generatedSlot.UpdateAugment(station.Augment) ;
            for (int i = 0 ; i < playerAugments.Count ; i++) {
                playerAugments[i].UpdateAugment(player.Augments[i]) ;
            }
        }


    }
}
