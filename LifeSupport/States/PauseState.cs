using LifeSupport.Config;
using LifeSupport.Controls;
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
    // This class contains all the content for the pause screen
    public class PauseState : State
    {
        /*Attributes*/

        // This list contains all the buttons on the pause screen
        private List<Component> components;

        private SpriteFont textFont;


        /*Constructor*/
        public PauseState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

        }

        /*Methods*/


        // Draw all content on pause screen function
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg)
        {
            // Draw the background
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.pauseScreen, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            // Draw all the buttons
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

        }


        // Load all this content before updating function
        public override void Load()
        {
            // Load assets
            Assets.Instance.LoadContent(game);
            game.IsMouseVisible = true;
            var btnTexture = Assets.Instance.btnTextureLarge;
            var btnText = Assets.Instance.btnText;
            textFont = btnText;


            // Resume the game button
            var resumeButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(750, 400),
                BtnText = "Resume Game",
            };
            resumeButton.Click += Resume_Button_Click;


            // Access options button
            var optionButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(750, 550),
                BtnText = "Change Game Settings"
            };
            optionButton.Click += Settings_Button_Click;

            
            // Return to main menu button
            var menuButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(750, 700),
                BtnText = "Restart Game",
            };
            menuButton.Click += Menu_Button_Click;


            // Quit the game button
            var quitButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(750, 850),
                BtnText = "Quit Game",
            };
            quitButton.Click += Quit_Button_Click;


            // Store all the buttons in the Component list
            components = new List<Component>()
            {
                resumeButton,
                optionButton,
                menuButton,
                quitButton,
            };



        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        // Update all the buttons function
        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
        }

        // Resume the game function. 
        private void Resume_Button_Click(object sender, EventArgs e)
        {
            game.IsMouseVisible = false;
            game.returnToGame(new GameState(game, graphDevice, content));
        }

        // Go to settings function
        private void Settings_Button_Click(object sender, EventArgs e)
        {
            OptionsState.openedFromPause = true;
            game.ChangeState(new OptionsState(game, graphDevice, content));    
        }

        // Go to main menu function (restarts game progress)
        private void Menu_Button_Click(object sender, EventArgs e)
        {
            OptionsState.openedFromPause = false;
            game.ChangeState(new MenuState(game, graphDevice, content));
        }

        // Quit the game
        private void Quit_Button_Click(object sender, EventArgs e)
        {
            game.Exit();
        }
    }
}