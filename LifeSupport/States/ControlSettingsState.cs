using LifeSupport.Config;
using LifeSupport.Controls;
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
    public class ControlSettingsState : State
    {

   
        private List<Button> buttons;
        private List<Component> components;
        private List<Keys> defaultControls = new List<Keys> { Keys.W, Keys.S, Keys.A, Keys.D, Keys.I, Keys.Escape };
        private List<Keys> storeKeys = new List<Keys>();

        private SpriteFont textFont;
        private Vector2 MoveUpTextPosition;
        private Vector2 MoveDownTextPosition;
        private Vector2 MoveLeftTextPosition;
        private Vector2 MoveRightTexturePosition;
        private Vector2 OpenInventoryTextPosition;
        private Vector2 OpenPauseMenuTextPosition;

        private string oldKeyAsString = "";
        private Keys oldKeyAsKey;
        private string newKey;
        private int clickedButtonIndex;

        public Keys MoveUp{ get; set; }
        public Keys MoveDown { get; set; }
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        public Keys OpenInventory { get; set; }
        public Keys PauseGame { get; set; }


        public ControlSettingsState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

        }



        public override void Load()
        {
            MoveUp = Controller.Instance.MoveUp;
            storeKeys.Add(MoveUp);
            MoveDown = Controller.Instance.MoveDown;
            storeKeys.Add(MoveDown);
            MoveLeft = Controller.Instance.MoveLeft;
            storeKeys.Add(MoveLeft);
            MoveRight = Controller.Instance.MoveRight;
            storeKeys.Add(MoveRight);
            OpenInventory = Controller.Instance.OpenInventory;
            storeKeys.Add(OpenInventory);
            PauseGame = Controller.Instance.PauseGame;
            storeKeys.Add(PauseGame);

            Assets.Instance.LoadContent(game);
            game.IsMouseVisible = true;

            var btnTexture = Assets.Instance.btnTextureLarge;
            var btnText = Assets.Instance.btnText;

            var smallbtnTexture = Assets.Instance.btnTextureSmall;
            textFont = btnText;


            var moveUpButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(520, 310),
                BtnText = MoveUp.ToString(),
            };
            moveUpButton.Click += Move_Up_Click;


            var moveDownButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(520, 420),
                BtnText = MoveDown.ToString(),
            };
            moveDownButton.Click += Move_Down_Click;


            var moveLeftButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(520, 530),
                BtnText = MoveLeft.ToString(),
            };
            moveLeftButton.Click += Move_Left_Click;


            var moveRightButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(520, 640),
                BtnText = MoveRight.ToString(),
            };
            moveRightButton.Click += Move_Right_Click;

            var openInventoryButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(520, 750),
                BtnText = OpenInventory.ToString(),
            };
            openInventoryButton.Click += Open_Inventory_Click;

            var openPauseMenuButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(1420, 310),
                BtnText = PauseGame.ToString(),
            };
            openPauseMenuButton.Click += Pause_Menu_Click;



            var settingsButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 950),
                BtnText = "Return To Settings",
            };
            settingsButton.Click += SettingsButton_Click;


            var restoreDefaultsButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(700, 950),
                BtnText = "Restore Default Controls",
            };
            restoreDefaultsButton.Click += DefaultButton_Click;

            var applyControlChangesButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(1200, 950),
                BtnText = "Save Changes",
            };
            applyControlChangesButton.Click += ApplyVolumeChangesButton_Click;


            buttons = new List<Button>() {
                moveUpButton,
                moveDownButton,
                moveLeftButton,
                moveRightButton,
                openInventoryButton,
                openPauseMenuButton,
            };

            components = new List<Component>()
            {
                settingsButton,
                restoreDefaultsButton,
                applyControlChangesButton
            };



            MoveUpTextPosition = new Vector2(200, 340);


            MoveDownTextPosition = new Vector2(200, 450);


            MoveLeftTextPosition = new Vector2(200, 560);


            MoveRightTexturePosition = new Vector2(200, 670);


            OpenInventoryTextPosition = new Vector2(200, 780);


            OpenPauseMenuTextPosition = new Vector2(1130, 340);

    }


        public override void Update(GameTime gameTime)
        {

            foreach (var component in components)
            {
                component.Update(gameTime);
            }

            foreach (var button in buttons)
            {
                button.Update(gameTime);
            }

            KeyboardState state = Keyboard.GetState();
       
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].ThisColor == Color.Black)
                {
                    var keys = state.GetPressedKeys();
                   if (keys.Length > 0)
                    {
                        newKey = keys[0].ToString();
                        buttons[i].BtnText = newKey;
                        buttons[i].ThisColor = Color.White;
                        storeKeys[i] = keys[0];
                    }
                }
            }

            
            for (int i = 0; i < buttons.Count; i++)
            {
                if(buttons[i].BtnText == buttons[clickedButtonIndex].BtnText && i != clickedButtonIndex)      
                {
                    buttons[i].BtnText = oldKeyAsString;
                    storeKeys[i] = oldKeyAsKey;
                }
            }

        }
        

        public override void PostUpdate(GameTime gameTime)
        {
          
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud)
        {
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.controlSettingsBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();


            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920, (float)Settings.Instance.Height / 1080, 1.0f));
            spriteBatch.DrawString(textFont, "Move Up:", MoveUpTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, "Move Down:", MoveDownTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, "Move Left:", MoveLeftTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, "Move Right:", MoveRightTexturePosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, "Open Inventory:", OpenInventoryTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, "Pause Game:", OpenPauseMenuTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.End();
            
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            foreach (var button in buttons)
            {
                button.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }



        private void ApplyVolumeChangesButton_Click(object sender, EventArgs e)
        {
            MoveUp = storeKeys[0];
            MoveDown = storeKeys[1];
            MoveLeft = storeKeys[2];
            MoveRight = storeKeys[3];
            OpenInventory = storeKeys[4];
            PauseGame = storeKeys[5];

            File.WriteAllText("Content/Settings/Control_Settings.json", JsonConvert.SerializeObject(this));
            Controller.Instance.reloadControls();
        }









        private void Move_Up_Click(object sender, EventArgs e)
        {
            clickedButtonIndex = 0;
            oldKeyAsString = buttons[clickedButtonIndex].BtnText;
            oldKeyAsKey = storeKeys[clickedButtonIndex];
            buttons[clickedButtonIndex].ThisColor = Color.Black;

            for(int i = 0; i < buttons.Count; i++)
            {
                if(buttons[i].ThisColor == Color.Black && i != clickedButtonIndex)
                {
                    buttons[i].ThisColor = Color.White;
                }
            }
        }





        private void Move_Down_Click(object sender, EventArgs e)
        {
            clickedButtonIndex = 1;
            oldKeyAsString = buttons[clickedButtonIndex].BtnText;
            oldKeyAsKey = storeKeys[clickedButtonIndex];
            buttons[clickedButtonIndex].ThisColor = Color.Black;

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].ThisColor == Color.Black && i != clickedButtonIndex)
                {
                    buttons[i].ThisColor = Color.White;
                }
            }
        }





        private void Move_Left_Click(object sender, EventArgs e)
        {
            clickedButtonIndex = 2;
            oldKeyAsString = buttons[clickedButtonIndex].BtnText;
            oldKeyAsKey = storeKeys[clickedButtonIndex];
            buttons[clickedButtonIndex].ThisColor = Color.Black;

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].ThisColor == Color.Black && i != clickedButtonIndex)
                {
                    buttons[i].ThisColor = Color.White;
                }
            }
        }





        private void Move_Right_Click(object sender, EventArgs e)
        {
            clickedButtonIndex = 3;
            oldKeyAsString = buttons[clickedButtonIndex].BtnText;
            oldKeyAsKey = storeKeys[clickedButtonIndex];
            buttons[clickedButtonIndex].ThisColor = Color.Black;

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].ThisColor == Color.Black && i != clickedButtonIndex)
                {
                    buttons[i].ThisColor = Color.White;
                }
            }
        }


        private void Open_Inventory_Click(object sender, EventArgs e)
        {
            clickedButtonIndex = 4;
            oldKeyAsString = buttons[clickedButtonIndex].BtnText;
            oldKeyAsKey = storeKeys[clickedButtonIndex];
            buttons[clickedButtonIndex].ThisColor = Color.Black;

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].ThisColor == Color.Black && i != clickedButtonIndex)
                {
                    buttons[i].ThisColor = Color.White;
                }
            }
        }

        private void Pause_Menu_Click(object sender, EventArgs e)
        {
            clickedButtonIndex = 5;
            oldKeyAsString = buttons[clickedButtonIndex].BtnText;
            oldKeyAsKey = storeKeys[clickedButtonIndex];
            buttons[clickedButtonIndex].ThisColor = Color.Black;

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].ThisColor == Color.Black && i != clickedButtonIndex)
                {
                    buttons[i].ThisColor = Color.White;
                }
            }
        }

        
        private void DefaultButton_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < defaultControls.Count; i++)
            {
                buttons[i].BtnText = "";
                buttons[i].BtnText = defaultControls[i].ToString();
                storeKeys[i] = defaultControls[i];
            }
        }
        

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new OptionsState(game, graphDevice, content));
        }
    }

}
