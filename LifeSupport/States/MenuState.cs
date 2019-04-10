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
using static System.Net.Mime.MediaTypeNames;

namespace LifeSupport.States
{
    public class MenuState : State
    {
        private MainGame game;
        private SpriteBatch bg;
        private List<Component> components;
        public MenuState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content) {
            this.game = game;
        }

        private void ExitBtn_Click(object sender, EventArgs e) {
            game.Exit();
        }

        private void OptionsBtn_Click(object sender, EventArgs e) {
            Console.WriteLine("Options Menu");
        }

        private void ContinueGameBtn_Click(object sender, EventArgs e) {
            Console.WriteLine("Continue Game");
        }

        private void NewGameButton_Click(object sender, EventArgs e) {
            //Load gameplay state here
            game.ChangeState(new GameState(game, graphDevice, content));
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud) {
            //Drawing each of the menu buttons
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width/1920));
            bg.Draw(Assets.Instance.mainMenuBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width/1920));
            foreach (var component in components) {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime) {
            //Taking sprites away when needed
        }

        public override void Update(GameTime gameTime) {
            foreach(var component in components) {
                component.Update(gameTime);
            }
        }

        public override void Load()
        {
            Assets.Instance.LoadContent(game);

            game.IsMouseVisible = true;
            

            var btnTexture = Assets.Instance.btnTextureLarge;
            var btnText = Assets.Instance.btnText;
            //Setting values for each of the Menu Buttons
            var newGameBtn = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 400),
                BtnText = "New Game",
            };

            newGameBtn.Click += NewGameButton_Click;

            var continueGameBtn = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 550),
                BtnText = "Continue",
            };

            continueGameBtn.Click += ContinueGameBtn_Click;

            var optionsBtn = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 700),
                BtnText = "Options",
            };

            optionsBtn.Click += OptionsBtn_Click;

            var exitBtn = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 850),
                BtnText = "Exit",
            };

            exitBtn.Click += ExitBtn_Click;

            components = new List<Component>() {
                newGameBtn,
                continueGameBtn,
                optionsBtn,
                exitBtn,
            };
        }
    }
}
