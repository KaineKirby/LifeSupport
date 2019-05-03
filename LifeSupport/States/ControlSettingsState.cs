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
    // This class contains all the functionality for the control settings page
    public class ControlSettingsState : State
    {
        /*Attributes*/

        // All keybinds will be stored in the Button type list
        private List<Button> buttons;

        // Other buttons (not keybinds) will be stored in the Component type list
        private List<Component> components;

        // This associative array contains all the current keybinds displayed on screen
        private Dictionary<int, Keys> currentKeys = new Dictionary<int, Keys>();

        // This associative array contains all the final keybinds that will be saved and written to Control_Settings.json
        private Dictionary<string, Keys> keysToSave = new Dictionary<string, Keys>();

        // This associative array contains the keybinds the game starts with. The player may switch back to these at any time
        private Dictionary<string, Keys> defaultKeys;

        // All text on screen (excluding keybinds)
        private SpriteFont textFont;
        private HUDString MoveUpText;
        private HUDString MoveDownText;
        private HUDString MoveLeftText;
        private HUDString MoveRightText;
        private HUDString OpenInventoryText;
        private HUDString OpenPauseMenuText;
        private HUDString InteractText;

        // This variable is used to check for duplicates (saves the keybind as string type)
        private string oldKeyAsString = "";

        // This variable is used to check for duplicates (saves the keybind as a Key type)
        private Keys oldKeyAsKey;

        // This variable captures the key the user pressed
        private string newKey;

        // This variable keeps track of which button was pressed (keybind buttons are ordered 0-6)
        // The first keybind button is given the index of 0, the next 1, and so on...
        private int clickedButtonIndex;


        /*Properties*/

        // These keys will be used to save the new keys typed in the keybind buttons and written
        // into Control_Settings.json. These keys are declared in Controller.cs under the Config namespace.
        public Keys MoveUp{ get; set; }
        public Keys MoveDown { get; set; }
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        public Keys OpenPlayerPage { get; set; }
        public Keys PauseGame { get; set; }
        public Keys Use { get; set; }



        /*Constructor*/

        // Declare and save the default keys for each action listed
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

        /*Methods*/

        // Call this function before any other function is executed
        public override void Load()
        {
            // Read and store controller settings from the controller singleton into the correct property. 
            // Add each of the keys read from the singleton into the keysToSave list
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


            // Fill the currenKeys list with keysToSave keys
            int count = 0;
            foreach(KeyValuePair<string, Keys> key in keysToSave)
            {
                currentKeys.Add(count, key.Value);
                count++;
            }
            count = 0;


            // Load all assets
            Assets.Instance.LoadContent(game);
            game.IsMouseVisible = true;

            var btnTexture = Assets.Instance.btnTextureLarge;
            var btnText = Assets.Instance.btnText;

            var smallbtnTexture = Assets.Instance.btnTextureSmall;
            textFont = btnText;



            // Move up keybind (of type button)
            var moveUpButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(520, 310),
                BtnText = MoveUp.ToString(),
            };
            moveUpButton.Click += Move_Up_Click;


            // Move down keybind (of type button)
            var moveDownButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(520, 420),
                BtnText = MoveDown.ToString(),
            };
            moveDownButton.Click += Move_Down_Click;


            // Move left keybind (of type button)
            var moveLeftButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(520, 530),
                BtnText = MoveLeft.ToString(),
            };
            moveLeftButton.Click += Move_Left_Click;


            // Move right keybind (of type button)
            var moveRightButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(520, 640),
                BtnText = MoveRight.ToString(),
            };
            moveRightButton.Click += Move_Right_Click;


            // Open inventory keybind (of type button)
            var openInventoryButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(520, 750),
                BtnText = OpenPlayerPage.ToString(),
            };
            openInventoryButton.Click += Open_Inventory_Click;


            // Open pause menu keybind (of type button)
            var openPauseMenuButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(1420, 310),
                BtnText = PauseGame.ToString(),
            };
            openPauseMenuButton.Click += Pause_Menu_Click;


            // use keybind (of type button)
            var interactButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(1420, 420),
                BtnText = Use.ToString(),
            };
            interactButton.Click += Interact_With_Object_Click;


            // Return to settings button
            var settingsButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 950),
                BtnText = "Return To Settings",
            };
            settingsButton.Click += SettingsButton_Click;


            // Restore all default keybinds button
            var restoreDefaultsButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(700, 950),
                BtnText = "Restore Default Controls",
            };
            restoreDefaultsButton.Click += DefaultButton_Click;


            // Save the new keybinds button
            var applyControlChangesButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(1200, 950),
                BtnText = "Save Changes",
            };
            applyControlChangesButton.Click += Save_Controls;


            // Fill the Button list with all the keybind buttons
            buttons = new List<Button>() {
                moveUpButton,
                moveDownButton,
                moveLeftButton,
                moveRightButton,
                openInventoryButton,
                openPauseMenuButton,
                interactButton,
            };

            // Fill the Component list with any button that doesn't capture keybinds
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


        // Update this page frequently many times per second
        public override void Update(GameTime gameTime)
        {
            //  Update all buttons on this page

            foreach (var component in components)
            {
                component.Update(gameTime);
            }

            foreach (var button in buttons)
            {
                button.Update(gameTime);
            }

            // Used to capture the next key pressed by the player
            KeyboardState state = Keyboard.GetState();
       

            // This for loop checks through every keybind button to see if a key was pressed (text turns black).
            // If a key is black, and the user presses a key on the keyboard, the new key is captured and displayed.
            // This new key repaces the black key.
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

            // This for loop checks for duplicates. If a duplicate is found, the older duplicate is then changed
            // to the last key that was erased (erase duplicate).
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

        // This function draws all content onto the screen many times per second
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg)
        {

            // Draw the background
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.controlSettingsBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();


            // Draw all text
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920, (float)Settings.Instance.Height / 1080, 1.0f));
            MoveUpText.Draw(spriteBatch);
            MoveDownText.Draw(spriteBatch);
            MoveLeftText.Draw(spriteBatch);
            MoveRightText.Draw(spriteBatch);
            OpenInventoryText.Draw(spriteBatch);
            OpenPauseMenuText.Draw(spriteBatch);
            InteractText.Draw(spriteBatch); 
            spriteBatch.End();
            
            // Draw all buttons not associated with capturing keybinds
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            // Draw all buttons associated with capturing keybinds
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            foreach (var button in buttons)
            {
                button.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }


        // When the "Save Changes" button is pressed, all properties are set equal to the values
        // currently stored in keysToSave (which are equal to currentKeys). Then, the properties
        // are written to  Control_Settings.json, and the singleton is reloaded. 
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





        /*Keybind functions*/


       /* Each of the following functions (drawn as buttons) are assigned and index (used as an id), numbered 0-6 (one for each button)
        * When the button is pressed with the mouse, the currentKey displayed inside the button as saved as a 
        * string and as a key. Then, the text turns black. The color of the text is used in logic within the update 
        * method. A black key is ready to be changed based on player input.
        * */

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



        // When this button is clicked, all keybinds are changed back to their default values
        // (stored with the defaultKeys dictionary)
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
        

        // Return to options
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new OptionsState(game, graphDevice, content));
        }




    }

}
