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
    // This class contains all the content for the player gear screen
    public class PlayerPageState : State
    {
        /* Attributes*/

        // This boolean checks to see if the player wants to delete an augment
        public static bool removeAugmentActive = false;

        // All player stats (drawn onto screen as strings)
        private HUDString damage;
        private HUDString rateOfFire;
        private HUDString shotSpeed;
        private HUDString range;
        private HUDString movementSpeed;
        private HUDString health;
        private HUDString oxygenLevel;

        // Player stats are updated through these strings
        private string damageString;
        private string rateOfFireString;
        private string shotSpeedString;
        private string rangeString;
        private string movementSpeedString;
        private string healthString;
        private string oxygenLevelString;

        // These strings show how much money the player has, and whether they have a keycard
        private HUDString money;
        private HUDString keycard;

        // This list will contain the delete augment button and resume game button
        private List<Component> components;

        // This list will contain all the augment slots (the rectangular button backgrounds behind the augments,
        // the augment textures themselves, and the augment text boxes that pop up when the slot is hovered over)
        private List<AugmentSlot> augmentSlots;

        // This list will store the money and keycard pictures (located in the inventory section)
        private List<Texture2D> inventoryItemTextures = new List<Texture2D>();

        // This list will store the positions of each inventory item (money and keycard)
        private List<Vector2> inventoryItemPositions = new List<Vector2>();

        // This list stores the textures of the money and keycard 
        private List<HUDImage> inventoryItems = new List<HUDImage>();

        // Used to scale the inventory item textures on screen
        private int scale = 1;

        // Checks to see if the player has a keycard
        private int keyCardCount = 0;


        private SpriteFont btnTextFont;
        private SpriteFont largeTextFont;

        // Player is declared in this class to access their stats, augments, money, and keycard values
        private Player player;

        /*Constructor*/
        public PlayerPageState(Player player, MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            this.player = player;
        }


        /*Methods*/

        // Draw everything on screen
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg) {

            // Draw the background
            bg.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.playerPageBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            // Draw all the character statistics 
            hud.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            damage.Draw(hud);
            rateOfFire.Draw(hud);
            shotSpeed.Draw(hud);
            range.Draw(hud);
            movementSpeed.Draw(hud);
            health.Draw(hud);
            oxygenLevel.Draw(hud);

            // Draw all of the items in the inventory section
            foreach (var item in inventoryItems) {
                item.DrawWithScale(hud, scale);
                if (item.Image == Assets.Instance.moneyLarge)
                {
                    money.Draw(hud);
                }
                if (item.Image == Assets.Instance.keycardLarge)
                {
                    keycard.Draw(hud);
                }
            }


            // Draw each augmentation and augment slot within the augmentation section
            foreach (var augmentSlot in augmentSlots)
            {
                augmentSlot.Draw(gameTime, hud);
            }

            // Draw the remaining buttons on screen
            foreach (var component in components)
            {
                component.Draw(gameTime, hud);
            }

            hud.End();

        }




        // Load function. This function is called before updatin the page
        public override void Load()
        {

            // Load game assets
            Assets.Instance.LoadContent(game);
            game.IsMouseVisible = true;

            // Used to position inventory items
            int xPos = 110;
            int yPos = 910;

            var btnTexture = Assets.Instance.btnTextureLarge;
            var mediumBtnTexture = Assets.Instance.btnTextureMedium;
            var btnText = Assets.Instance.btnText;
            var largeText = Assets.Instance.largeText;

            btnTextFont = btnText;
            largeTextFont = largeText;


            // If the player has a money or keycard, add those textures to the inventory items list (will be used to draw textures on screen)
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


            // If the player has money or a card, display the correct amount they possess
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


            // Resume game button
            var resumeButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(1410, 75),
                BtnText = "Resume Game",
            };
            resumeButton.Click += Resume_Button_Click;


            // Delete augment button
            var deleteAugmentButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(810, 75),
                BtnText = "Destroy Augment",
            };
            deleteAugmentButton.Click += Delete_Augment_Click;
            
            /*Each of the following objects declare an augmentation slot and a Augment text box.
             *The augment text box contains augment information, and it hovers over the slot (and drawn) if the slot is
             * is occupied with an augment. The slot takes in a texture, augment text box, and an augment from the player augment
             * list as parameters
             * */
             

            var hoverBox0 = new AugmentTextBox(player.Augments[0], game.GraphicsDevice);
            var augmentSlot0 = new AugmentSlot(mediumBtnTexture, hoverBox0, player.Augments[0])
            {
                CurrPosition = new Vector2(1125, 370)
            };
            augmentSlot0.Click += Delete_Augment0_Click;

            var hoverBox1 = new AugmentTextBox(player.Augments[1], game.GraphicsDevice);
            var augmentSlot1 = new AugmentSlot(mediumBtnTexture, hoverBox1, player.Augments[1])
            {
                CurrPosition = new Vector2(1425, 370)
            };
            augmentSlot1.Click += Delete_Augment1_Click;

            var hoverBox2 = new AugmentTextBox(player.Augments[2], game.GraphicsDevice);
            var augmentSlot2 = new AugmentSlot(mediumBtnTexture, hoverBox2, player.Augments[2])
            {
                CurrPosition = new Vector2(1125, 530)
            };
            augmentSlot2.Click += Delete_Augment2_Click;

            var hoverBox3 = new AugmentTextBox(player.Augments[3], game.GraphicsDevice);
            var augmentSlot3 = new AugmentSlot(mediumBtnTexture, hoverBox3, player.Augments[3])
            {
                CurrPosition = new Vector2(1425, 530)
            };
            augmentSlot3.Click += Delete_Augment3_Click;

            var hoverBox4 = new AugmentTextBox(player.Augments[4], game.GraphicsDevice);
            var augmentSlot4 = new AugmentSlot(mediumBtnTexture, hoverBox4, player.Augments[4])
            {
                CurrPosition = new Vector2(1125, 690)
            };
            augmentSlot4.Click += Delete_Augment4_Click;

            var hoverBox5 = new AugmentTextBox(player.Augments[5], game.GraphicsDevice);
            var augmentSlot5 = new AugmentSlot(mediumBtnTexture, hoverBox5, player.Augments[5])
            {
                CurrPosition = new Vector2(1425, 690)
            };
            augmentSlot5.Click += Delete_Augment5_Click;

            var hoverBox6 = new AugmentTextBox(player.Augments[6], game.GraphicsDevice);
            var augmentSlot6 = new AugmentSlot(mediumBtnTexture, hoverBox6, player.Augments[6])
            {
                CurrPosition = new Vector2(1125, 850)
            };
            augmentSlot6.Click += Delete_Augment6_Click;

            var hoverBox7 = new AugmentTextBox(player.Augments[7], game.GraphicsDevice);
            var augmentSlot7 = new AugmentSlot(mediumBtnTexture, hoverBox7, player.Augments[7])
            {
                CurrPosition = new Vector2(1425, 850)
            };
            augmentSlot7.Click += Delete_Augment7_Click;


            // Instantiate list of buttons not related to augmentations
            components = new List<Component>()
            {
                deleteAugmentButton,
                resumeButton,
            };

            // Instantiate a list of augmentation slots 
            augmentSlots = new List<AugmentSlot>()
            {
                augmentSlot0,
                augmentSlot1,
                augmentSlot2,
                augmentSlot3,
                augmentSlot4,
                augmentSlot5,
                augmentSlot6,
                augmentSlot7,
            };

            damageString = "Weapon Damage: " + player.Damage.ToString();
            rateOfFireString = "Rate of Fire: " + (1/(player.RateOfFire)).ToString() + " shot / second";
            shotSpeedString = "Bullet Speed: " + player.ShotSpeed.ToString();
            rangeString = "Bullet Range: " + player.Range.ToString();
            movementSpeedString = "Movement Speed: " + player.MoveSpeed.ToString();
            healthString = "Health: " + player.Health.ToString() + " hearts";
            oxygenLevelString = "Oxygen Time Remaining: " + Math.Round(player.OxygenTime, 0) + " seconds";

            this.damage = new HUDString(largeText, damageString, Color.White, new Vector2(110, 350));
            this.rateOfFire = new HUDString(largeText, rateOfFireString, Color.White, new Vector2(110, 400));
            this.shotSpeed = new HUDString(largeText, shotSpeedString, Color.White, new Vector2(110, 450));
            this.range = new HUDString(largeText, rangeString, Color.White, new Vector2(110, 500));
            this.movementSpeed = new HUDString(largeText, movementSpeedString, Color.White, new Vector2(110, 550));
            this.health = new HUDString(largeText, healthString, Color.White, new Vector2(110, 600));
            this.oxygenLevel = new HUDString(largeText, oxygenLevelString, Color.White, new Vector2(110, 650));

        }


        // Go back to the game function
        private void Resume_Button_Click(object sender, EventArgs e)
        {
            removeAugmentActive = false;
            game.IsMouseVisible = false;
            game.returnToGame(new GameState(game, graphDevice, content));
        }


        // If the Destroy augment button is pressed and active, then the player can delete
        // any augment by clicking on it's augmentation slot. All augment slots will turn red,
        // and a red x will be drawn if it the mouse is hovering over it.
        private void Delete_Augment_Click(object sender, EventArgs e)
        {
            if (removeAugmentActive == true)
            {
                removeAugmentActive = false;
            }
            else if (removeAugmentActive == false)
            {
                removeAugmentActive = true;
            }
        }

        // The following functions will work only if removeAugmentActive is true (all augment slots are red)
        // If an augment is clicked on while this bool is true, then it is removed from the player augmentation list,
        // and their stats are updated. 

        public void Delete_Augment0_Click(object sender, EventArgs e)
        {
            if (removeAugmentActive == true)
            {
                player.RemoveAugment(0);
                augmentSlots[0].UpdateAugment(player.Augments[0]);
            }
            else
            {
                return;
            }
        }

        public void Delete_Augment1_Click(object sender, EventArgs e)
        {
            if (removeAugmentActive == true)
            {
                player.RemoveAugment(1);
                augmentSlots[1].UpdateAugment(player.Augments[1]);
            }
            else
            {
                return;
            }
        }

        public void Delete_Augment2_Click(object sender, EventArgs e)
        {
            if (removeAugmentActive == true)
            {
                player.RemoveAugment(2);
                augmentSlots[2].UpdateAugment(player.Augments[2]);
            }
            else
            {
                return;
            }
        }

        public void Delete_Augment3_Click(object sender, EventArgs e)
        {
            if (removeAugmentActive == true)
            {
                player.RemoveAugment(3);
                augmentSlots[3].UpdateAugment(player.Augments[3]);
            }
            else
            {
                return;
            }
        }

        public void Delete_Augment4_Click(object sender, EventArgs e)
        {
            if (removeAugmentActive == true)
            {
                player.RemoveAugment(4);
                augmentSlots[4].UpdateAugment(player.Augments[4]);
            }
            else
            {
                return;
            }
        }

        public void Delete_Augment5_Click(object sender, EventArgs e)
        {
            if (removeAugmentActive == true)
            {
                player.RemoveAugment(5);
                augmentSlots[5].UpdateAugment(player.Augments[5]);
            }
            else
            {
                return;
            }
        }

        public void Delete_Augment6_Click(object sender, EventArgs e)
        {
            if (removeAugmentActive == true)
            {
                player.RemoveAugment(6);
                augmentSlots[6].UpdateAugment(player.Augments[6]);
            }
            else
            {
                return;
            }
        }

        public void Delete_Augment7_Click(object sender, EventArgs e)
        {
            if (removeAugmentActive == true)
            {
                player.RemoveAugment(7);
                augmentSlots[7].UpdateAugment(player.Augments[7]);
            }
            else
            {
                return;
            }
        }




        public override void PostUpdate(GameTime gameTime)
        {

        }

        // Update all content with the page
        public override void Update(GameTime gameTime)
        {
            // Update the resume button and destroy augment button
            foreach (var component in components)
            {
                component.Update(gameTime);
            }

            // Update all augment slots
            foreach (var augmentSlot in augmentSlots)
            {
                augmentSlot.Update(gameTime);
            }

            // Update the player statistics if an augment is removed
            damage.Update("Weapon Damage: " + player.Damage.ToString());
            rateOfFire.Update("Rate of Fire: " + (1/(player.RateOfFire)).ToString() + " shot / second");
            shotSpeed.Update("Bullet Speed: " + player.ShotSpeed.ToString());
            range.Update("Bullet Range: " + player.Range.ToString());
            movementSpeed.Update("Movement Speed: " + player.MoveSpeed.ToString());
            health.Update("Health: " + player.Health.ToString() + " hearts");
            oxygenLevel.Update("Oxygen Time Remaining: " + Math.Round(player.OxygenTime, 0) + " seconds");
        }
    }
}

