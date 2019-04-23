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
    class DeathState : State
    {
        private List<Component> components;
        private SpriteFont textFont;

        public DeathState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg)
        {
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.deathScreen, new Rectangle(0, 0, 1920, 1080), Color.White);
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

            var menuButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 400),
                BtnText = "Main Menu",
            };
            menuButton.Click += Menu_Button_Click;

            var quitButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 550),
                BtnText = "Quit Game"
            };
            quitButton.Click += Quit_Button_Click;

            components = new List<Component>()
            {
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
