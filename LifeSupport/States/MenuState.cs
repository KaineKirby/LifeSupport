using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using LifeSupport.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Penumbra;
using static System.Net.Mime.MediaTypeNames;

namespace LifeSupport.States
{
    // This class contains all the content for the main menu screen
    public class MenuState : State
    {
        /*Attributes*/

        private MainGame game;
        private SpriteBatch bg;

        // List of all buttons on the main menu screen
        private List<Component> components;

        /*Constructor*/
        public MenuState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content) {
            this.game = game;
        }

        /*Methods*/

        // This function (button) quits the game
        private void ExitBtn_Click(object sender, EventArgs e) {
            game.Exit();
        }

        // This function (button) takes the player to the options page
        private void OptionsBtn_Click(object sender, EventArgs e) {
            Console.WriteLine("Settings Menu");
            game.ChangeState(new OptionsState(game, graphDevice, content));
        }


        // This function (button) starts a new game, then takes the player to the main gameplay window
        private void NewGameButton_Click(object sender, EventArgs e) {
            //Load gameplay state here
            game.ChangeState(new GameState(game, graphDevice, content));
        }


        // Drawing function
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg) {
            
            // Draw the background
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width/1920));
            bg.Draw(Assets.Instance.mainMenuBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            // Draw each of the menu buttons
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width/1920));
            foreach (var component in components) {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

        }

        public override void PostUpdate(GameTime gameTime) {
            //Taking sprites away when needed
        }

        // Update all the buttons function
        public override void Update(GameTime gameTime) {
            foreach(var component in components) {
                component.Update(gameTime);
            }
        }


        // Load function that is called before any updating occurs
        public override void Load()
        {

            // Load all assets
            Assets.Instance.LoadContent(game);
            game.IsMouseVisible = true;
            var btnTexture = Assets.Instance.btnTextureLarge;
            var btnText = Assets.Instance.btnText;


            //Setting values for each of the Menu Buttons

            // Start a new game button
            var newGameBtn = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 400),
                BtnText = "New Game",
            };
            newGameBtn.Click += NewGameButton_Click;


            // Go to options screen button
            var optionsBtn = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 550),
                BtnText = "Settings",
            };
            optionsBtn.Click += OptionsBtn_Click;

            // Exit game button
            var exitBtn = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 700),
                BtnText = "Exit",
            };
            exitBtn.Click += ExitBtn_Click;

            // Store all the buttons into the component list
            components = new List<Component>() {
                newGameBtn,
                optionsBtn,
                exitBtn,
            };

        }
    }
}
