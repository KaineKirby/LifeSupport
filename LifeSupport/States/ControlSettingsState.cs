using LifeSupport.Config;
using LifeSupport.Controls;
using LifeSupport.HUD;
using LifeSupport.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Penumbra;
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


        private Dictionary<int, Keys> currentKeys = new Dictionary<int, Keys>();
        private Dictionary<string, Keys> keysToSave = new Dictionary<string, Keys>();
        private Dictionary<string, Keys> defaultKeys;


        private SpriteFont textFont;
        private HUDString MoveUpText;
        private HUDString MoveDownText;
        private HUDString MoveLeftText;
        private HUDString MoveRightText;
        private HUDString OpenInventoryText;
        private HUDString OpenPauseMenuText;
        private HUDString InteractText;


        private string oldKeyAsString = "";
        private Keys oldKeyAsKey;
        private string newKey;
        private int clickedButtonIndex;


        public Keys MoveUp{ get; set; }
        public Keys MoveDown { get; set; }
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        public Keys OpenPlayerPage { get; set; }
        public Keys PauseGame { get; set; }
        public Keys Use { get; set; }




        public ControlSettingsState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            this.defaultKeys = new Dictionary<string, Keys>
            {
                {"MoveUp", Keys.W },
                {"MoveDown", Keys.S},
                {"MoveLeft", Keys.A},
                {"MoveRight", Keys.D },
                {"OpenPlayerPage", Keys.I },
                {"PauseGame", Keys.Escape },
                {"Use", Keys.E },
            };
        }



        public override void Load()
        {
            MoveUp = Controller.Instance.MoveUp;
            keysToSave.Add("MoveUp", MoveUp);
 
            MoveDown = Controller.Instance.MoveDown;
            keysToSave.Add("MoveDown", MoveDown);

            MoveLeft = Controller.Instance.MoveLeft;
            keysToSave.Add("MoveLeft", MoveLeft);

            MoveRight = Controller.Instance.MoveRight;
            keysToSave.Add("MoveRight", MoveRight);

            OpenPlayerPage = Controller.Instance.OpenPlayerPage;
            keysToSave.Add("OpenPlayerPage", OpenPlayerPage);

            PauseGame = Controller.Instance.PauseGame;
            keysToSave.Add("PauseGame", PauseGame);

            Use = Controller.Instance.Use;
            keysToSave.Add("Use", Use);

            int count = 0;
            foreach(KeyValuePair<string, Keys> key in keysToSave)
            {
                currentKeys.Add(count, key.Value);
                count++;
            }
            count = 0;

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
                BtnText = OpenPlayerPage.ToString(),
            };
            openInventoryButton.Click += Open_Inventory_Click;

            var openPauseMenuButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(1420, 310),
                BtnText = PauseGame.ToString(),
            };
            openPauseMenuButton.Click += Pause_Menu_Click;

            var interactButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(1420, 420),
                BtnText = Use.ToString(),
            };
            interactButton.Click += Interact_With_Object_Click;



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
            applyControlChangesButton.Click += Save_Controls;


            buttons = new List<Button>() {
                moveUpButton,
                moveDownButton,
                moveLeftButton,
                moveRightButton,
                openInventoryButton,
                openPauseMenuButton,
                interactButton,
            };

            components = new List<Component>()
            {
                settingsButton,
                restoreDefaultsButton,
                applyControlChangesButton
            };


            MoveUpText = new HUDString(textFont, "Move Up: ", Color.White, new Vector2(200, 340));

            MoveDownText = new HUDString(textFont, "Move Down: ", Color.White, new Vector2(200, 450));

            MoveLeftText = new HUDString(textFont, "Move Left: ", Color.White, new Vector2(200, 560));

            MoveRightText = new HUDString(textFont, "Move Right: ", Color.White, new Vector2(200, 670));

            OpenInventoryText = new HUDString(textFont, "Open Inventory: ", Color.White, new Vector2(200, 780));

            OpenPauseMenuText = new HUDString(textFont, "Open Pause Menu: ", Color.White, new Vector2(1130, 340));

            InteractText = new HUDString(textFont, "Interact: ", Color.White, new Vector2(1130, 450));

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
                        currentKeys[i] = keys[0];
                    }
                }
            }

            
            for (int i = 0; i < buttons.Count; i++)
            {
                if(buttons[i].BtnText == buttons[clickedButtonIndex].BtnText && i != clickedButtonIndex)      
                {
                    buttons[i].BtnText = oldKeyAsString;
                    currentKeys[i] = oldKeyAsKey;
                }
            }

        }
        

        public override void PostUpdate(GameTime gameTime)
        {
          
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg)
        {
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.controlSettingsBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();


            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920, (float)Settings.Instance.Height / 1080, 1.0f));
            MoveUpText.Draw(spriteBatch);
            MoveDownText.Draw(spriteBatch);
            MoveLeftText.Draw(spriteBatch);
            MoveRightText.Draw(spriteBatch);
            OpenInventoryText.Draw(spriteBatch);
            OpenPauseMenuText.Draw(spriteBatch);
            InteractText.Draw(spriteBatch); 
            spriteBatch.End();
            
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            foreach (var button in buttons)
            {
                button.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }



        private void Save_Controls(object sender, EventArgs e)
        {
            foreach(KeyValuePair<int, Keys> control in currentKeys)
            {
                switch(control.Key)
                {
                    case 0:
                        keysToSave["MoveUp"] = control.Value;
                        break;
                    case 1:
                        keysToSave["MoveDown"] = control.Value;
                        break;
                    case 2:
                        keysToSave["MoveLeft"] = control.Value;
                        break;
                    case 3:
                        keysToSave["MoveRight"] = control.Value;
                        break;
                    case 4:
                        keysToSave["OpenPlayerPage"] = control.Value;
                        break;
                    case 5:
                        keysToSave["PauseGame"] = control.Value;
                        break;
                    case 6:
                        keysToSave["Use"] = control.Value;
                        break;
                    default:
                        break;
                };
            }
            MoveUp = keysToSave["MoveUp"];
            MoveDown = keysToSave["MoveDown"];
            MoveLeft = keysToSave["MoveLeft"];
            MoveRight = keysToSave["MoveRight"];
            OpenPlayerPage = keysToSave["OpenPlayerPage"];
            PauseGame = keysToSave["PauseGame"];
            Use = keysToSave["Use"];

            File.WriteAllText("Content/Settings/Control_Settings.json", JsonConvert.SerializeObject(this));
            Controller.Instance.reloadControls();
        }









        private void Move_Up_Click(object sender, EventArgs e)
        {
            clickedButtonIndex = 0;
            oldKeyAsString = buttons[clickedButtonIndex].BtnText;
            oldKeyAsKey = currentKeys[clickedButtonIndex];
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
            oldKeyAsKey = currentKeys[clickedButtonIndex];
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
            oldKeyAsKey = currentKeys[clickedButtonIndex];
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
            oldKeyAsKey = currentKeys[clickedButtonIndex];
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
            oldKeyAsKey = currentKeys[clickedButtonIndex];
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
            oldKeyAsKey = currentKeys[clickedButtonIndex];
            buttons[clickedButtonIndex].ThisColor = Color.Black;

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].ThisColor == Color.Black && i != clickedButtonIndex)
                {
                    buttons[i].ThisColor = Color.White;
                }
            }
        }

        private void Interact_With_Object_Click(object sender, EventArgs e)
        {
            clickedButtonIndex = 6;

            oldKeyAsString = buttons[clickedButtonIndex].BtnText;
            oldKeyAsKey = currentKeys[clickedButtonIndex];
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
            int i = 0;
            foreach(KeyValuePair<string, Keys> control in defaultKeys)
            {
                buttons[i].BtnText = "";
                buttons[i].BtnText = control.Value.ToString();
                currentKeys[i] = control.Value;
                i++;
            }
        }
        

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new OptionsState(game, graphDevice, content));
        }




    }

}
