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
    // This class (screen) is executed when the player beats the game
    class VictoryState : State
    {
        /*Attributes*/
        private List<Component> components;
        private SpriteFont textFont;

        /*Constructor*/
        public VictoryState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

        }

        /*Methods*/

        // Draw content onto the screen
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg)
        {
            // Draw the background
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.victoryScreen, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            // Draw the buttons
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

        }


        // Load all content first
        public override void Load()
        {
            // Load assets
            Assets.Instance.LoadContent(game);
            game.IsMouseVisible = true;
            var btnTexture = Assets.Instance.btnTextureLarge;
            var btnText = Assets.Instance.btnText;
            textFont = btnText;

            // Return to main menu button
            var menuButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 400),
                BtnText = "Main Menu",
            };
            menuButton.Click += Menu_Button_Click;

            // Quit the game button
            var quitButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 550),
                BtnText = "Quit Game"
            };
            quitButton.Click += Quit_Button_Click;

            // Instantaite the buttons, store them in the list
            components = new List<Component>()
            {
                menuButton,
                quitButton,
            };
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }


        // Update all the buttons on screen
        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
        }

        // If the main menu button is clicked, go to main menu (resets game)
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
