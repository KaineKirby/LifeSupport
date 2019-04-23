using LifeSupport.Augments;
using LifeSupport.Config;
using LifeSupport.Controls;
using LifeSupport.GameObjects;
using LifeSupport.HUD;
using LifeSupport.States.Controls;
using LifeSupport.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.States
{
    public class PlayerPageState : State
    {

        private HUDString damage;
        private HUDString rateOfFire;
        private HUDString shotSpeed;
        private HUDString range;
        private HUDString movementSpeed;
        private HUDString health;
        private HUDString oxygenLevel;

        private HUDString money;
        private HUDString keycard;


        private List<Component> components;
        private List<Texture2D> inventoryItemTextures = new List<Texture2D>();
        private List<Vector2> inventoryItemPositions = new List<Vector2>();
        private List<HUDImage> inventoryItems = new List<HUDImage>();
        private List<Augmentation> equippedAugments = new List<Augmentation>();

        private int scale = 1;
        private int keyCardCount = 0;


        private SpriteFont btnTextFont;
        private SpriteFont largeTextFont;
        private Player player;


        public PlayerPageState(Player player, MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            this.player = player;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg) {

            bg.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.playerPageBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();


            hud.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            damage.DrawWithSpecificFont(hud);
            rateOfFire.DrawWithSpecificFont(hud);
            shotSpeed.DrawWithSpecificFont(hud);
            range.DrawWithSpecificFont(hud);
            movementSpeed.DrawWithSpecificFont(hud);
            health.DrawWithSpecificFont(hud);
            oxygenLevel.DrawWithSpecificFont(hud);

            foreach (var item in inventoryItems) {
                item.DrawWithScale(hud, scale);
                if (item.Image == Assets.instance.moneyLarge)
                {
                    money.DrawWithSpecificFont(hud);
                }
                if (item.Image == Assets.instance.keycardLarge)
                {
                    keycard.DrawWithSpecificFont(hud);
                }
            }

            foreach (var augment in equippedAugments) {
                augment.DrawWithScale(hud, scale);
            }

            foreach (var component in components) {
                component.Draw(gameTime, hud);
            }

            hud.End();

        }

        public override void Load()
        {
            Assets.Instance.LoadContent(game);
            game.IsMouseVisible = true;

            int xPos = 110;
            int yPos = 910;

            var btnTexture = Assets.Instance.btnTextureLarge;
            var mediumBtnTexture = Assets.Instance.btnTextureMedium;
            var btnText = Assets.Instance.btnText;
            var largeText = Assets.Instance.largeText;

            btnTextFont = btnText;
            largeTextFont = largeText;


            if (player.Money > 0)
            {
                inventoryItemTextures.Add(Assets.Instance.moneyLarge);
            }

            if (player.HasCard == true)
            {
                inventoryItemTextures.Add(Assets.Instance.keycardLarge);
                keyCardCount = 1;
            }
            else if (player.HasCard == false)
            {
                keyCardCount = 0;
            }



            for (int i = 0; i < inventoryItemTextures.Count; i++)
            {
                inventoryItemPositions.Add(new Vector2(xPos, yPos));
                HUDImage newItem = new HUDImage(inventoryItemTextures[i], inventoryItemPositions[i]);
                inventoryItems.Add(newItem);
                if (inventoryItemTextures[i] == Assets.Instance.moneyLarge)
                {
                    this.money = new HUDString(largeText, "X" + player.Money, Color.White, new Vector2(inventoryItemPositions[i].X + (newItem.Image.Width * scale), inventoryItemPositions[i].Y + newItem.Image.Height/2));
                }
                if (inventoryItemTextures[i] == Assets.Instance.keycardLarge)
                {
                    this.keycard = new HUDString(largeText, "X" + keyCardCount, Color.White, new Vector2(inventoryItemPositions[i].X + (newItem.Image.Width * scale), inventoryItemPositions[i].Y + newItem.Image.Height/2));
                }
                xPos += 200;
            }



            var resumeButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(1410, 75),
                BtnText = "Resume Game",
            };
            resumeButton.Click += Resume_Button_Click;

            int yPosition = 400;
            int loopCount = 0;

            for (int i = 0; i < player.Augments.Count; i++)
            {
                if (player.Augments[i] != null)
                {
                    equippedAugments.Add(player.Augments[i]);
                    if (i % 2 == 0)
                    {
                        equippedAugments[i].position.X = 1200;
                        equippedAugments[i].position.Y = yPosition;
                    }
                    else
                    {
                        equippedAugments[i].position.X = 1500;
                        equippedAugments[i].position.Y = yPosition;
                    }
                }
                loopCount++;
                if (loopCount == 2)
                {
                    yPosition += 160;
                    loopCount = 0;
                }
            }

            Console.WriteLine(player.Augments[0].position.X);

            var hoverBox0 = new AugmentTextBox(player.Augments[0], game.GraphicsDevice);
            var augmentSlot0 = new AugmentSlot(mediumBtnTexture, hoverBox0)
            {
                CurrPosition = new Vector2(1125, 370)
            };


            var hoverBox1 = new AugmentTextBox(player.Augments[1], game.GraphicsDevice);
            var augmentSlot1 = new AugmentSlot(mediumBtnTexture, hoverBox1)
            {
                CurrPosition = new Vector2(1425, 370)
            };


            var hoverBox2 = new AugmentTextBox(player.Augments[2], game.GraphicsDevice);
            var augmentSlot2 = new AugmentSlot(mediumBtnTexture, hoverBox2)
            {
                CurrPosition = new Vector2(1125, 530)
            };


            var hoverBox3 = new AugmentTextBox(player.Augments[3], game.GraphicsDevice);
            var augmentSlot3 = new AugmentSlot(mediumBtnTexture, hoverBox3)
            {
                CurrPosition = new Vector2(1425, 530)
            };


            var hoverBox4 = new AugmentTextBox(player.Augments[4], game.GraphicsDevice);
            var augmentSlot4 = new AugmentSlot(mediumBtnTexture, hoverBox4)
            {
                CurrPosition = new Vector2(1125, 690)
            };


            var hoverBox5 = new AugmentTextBox(player.Augments[5], game.GraphicsDevice);
            var augmentSlot5 = new AugmentSlot(mediumBtnTexture, hoverBox5)
            {
                CurrPosition = new Vector2(1425, 690)
            };


            var hoverBox6 = new AugmentTextBox(player.Augments[6], game.GraphicsDevice);
            var augmentSlot6 = new AugmentSlot(mediumBtnTexture, hoverBox6)
            {
                CurrPosition = new Vector2(1125, 850)
            };


            var hoverBox7 = new AugmentTextBox(player.Augments[7], game.GraphicsDevice);
            var augmentSlot7 = new AugmentSlot(mediumBtnTexture, hoverBox7)
            {
                CurrPosition = new Vector2(1425, 850)
            };

            components = new List<Component>()
            {
                resumeButton,
                augmentSlot0,
                augmentSlot1,
                augmentSlot2,
                augmentSlot3,
                augmentSlot4,
                augmentSlot5,
                augmentSlot6,
                augmentSlot7,
            };

            this.damage = new HUDString(largeText, "Weapon Damage: " + player.Damage, Color.White, new Vector2(110, 350));
            this.rateOfFire = new HUDString(largeText, "Rate of Fire: " + player.RateOfFire, Color.White, new Vector2(110, 400));
            this.shotSpeed = new HUDString(largeText, "Bullet Speed: " + player.ShotSpeed, Color.White, new Vector2(110, 450));
            this.range = new HUDString(largeText, "Bullet Range: " + player.Range, Color.White, new Vector2(110, 500));
            this.movementSpeed = new HUDString(largeText, "Movement Speed: " + player.MoveSpeed, Color.White, new Vector2(110, 550));
            this.health = new HUDString(largeText, "Health: " + player.Health + " hearts", Color.White, new Vector2(110, 600));
            this.oxygenLevel = new HUDString(largeText, "Oxygen Time Remaining: " + Math.Round(player.OxygenTime, 0) + " seconds", Color.White, new Vector2(110, 650));

        }

        private void Resume_Button_Click(object sender, EventArgs e)
        {
            game.IsMouseVisible = false;
            game.returnToGame(new GameState(game, graphDevice, content));
        }




        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
        }
    }
}
