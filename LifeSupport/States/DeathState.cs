using LifeSupport.Config;
using LifeSupport.Controls;
using LifeSupport.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.States
{
    // The death screen class that is displayed when the player dies
    class DeathState : State
    {
        /*Attributes*/

        // All buttons are stored in this list
        private List<Component> components;


        private SpriteFont textFont;

        /*Constructor*/
        public DeathState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

        }

        /*Methods*/

        // Draw all content on the screen
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg)
        {
            // Draw the background
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.deathScreen, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            // Draw the buttons
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

        }

        // Everything in this function is called when the page is first loaded
        public override void Load()
        {

            //Load assets
            Assets.Instance.LoadContent(game);
            game.IsMouseVisible = true;

            var btnTexture = Assets.Instance.btnTextureLarge;
            var btnText = Assets.Instance.btnText;
            textFont = btnText;

            // Return to menu button
            var menuButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 400),
                BtnText = "Main Menu",
            };
            menuButton.Click += Menu_Button_Click;

            // Quit game button
            var quitButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 550),
                BtnText = "Quit Game"
            };
            quitButton.Click += Quit_Button_Click;


            // Intitialize list of buttons as of type components
            components = new List<Component>()
            {
                menuButton,
                quitButton,
            };
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }


        // Update each button
        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
        }

        // Return to menu function
        private void Menu_Button_Click(object sender, EventArgs e)
        {
            OptionsState.openedFromPause = false;
            game.ChangeState(new MenuState(game, graphDevice, content));
        }


        // Quit game function
        private void Quit_Button_Click(object sender, EventArgs e)
        {
            game.Exit();
        }
    }
}
