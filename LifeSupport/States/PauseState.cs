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
    public class PauseState : State
    {

        private List<Component> components;
        private SpriteFont textFont;

        public PauseState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg)
        {
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.pauseScreen, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

        }

        public override void Load()
        {
            Assets.Instance.LoadContent(game);
            game.IsMouseVisible = true;

            var btnTexture = Assets.Instance.btnTextureLarge;
            var btnText = Assets.Instance.btnText;
            textFont = btnText;

            var resumeButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(750, 400),
                BtnText = "Resume Game",
            };
            resumeButton.Click += Resume_Button_Click;

            var optionButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(750, 550),
                BtnText = "Change Game Settings"
            };
            optionButton.Click += Settings_Button_Click;

            var menuButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(750, 700),
                BtnText = "Restart Game",
            };
            menuButton.Click += Menu_Button_Click;


            var quitButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(750, 850),
                BtnText = "Quit Game",
            };
            quitButton.Click += Quit_Button_Click;

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

        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
        }

        private void Resume_Button_Click(object sender, EventArgs e)
        {
            game.IsMouseVisible = false;
            game.returnToGame(new GameState(game, graphDevice, content));
        }

        private void Settings_Button_Click(object sender, EventArgs e)
        {
            OptionsState.openedFromPause = true;
            game.ChangeState(new OptionsState(game, graphDevice, content));    
        }

        private void Menu_Button_Click(object sender, EventArgs e)
        {
            OptionsState.openedFromPause = false;
            game.ChangeState(new MenuState(game, graphDevice, content));
        }

        private void Quit_Button_Click(object sender, EventArgs e)
        {
            game.Exit();
        }
    }
}