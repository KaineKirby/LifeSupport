using LifeSupport.Config;
using LifeSupport.Controls;
using LifeSupport.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.States
{
    public class VideoSettingsState : State
    {
        private MainGame game;
        private SpriteBatch bg;
        private List<Component> components;

        private SpriteFont textFont;


        private string resolutionString;
        private string fullScreenString;

        private string[] screenDimensions;

        private string[] fullScreenOption = new string[]{ "Disabled", "Enabled" };
        private int fullScreenArrayIndex;
        public bool Fullscreen { get; set; }
        public int Width = 1920;
        public int Height = 1080;
        public bool ShowFps = true;
        public int Fps = 300;
        public float SfxVolume = 100;
        public float MusVolume = 100;


        public VideoSettingsState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            this.game = game;

        }


        private void ResolutionButton_LeftArrow_Click(object sender, EventArgs e)
        {

        }

        private void ResolutionButton_RightArrow_Click(object sender, EventArgs e)
        {

        }

        private void FullScreenButton_LeftArrow_Click(object sender, EventArgs e)
        {
            fullScreenArrayIndex -= 1;
            this.Fullscreen = false;
            if(fullScreenArrayIndex < 0)
            {
                fullScreenArrayIndex = 1;
                this.Fullscreen = true;
            }
        }

        private void FullScreenButton_RightArrow_Click(object sender, EventArgs e)
        {
            fullScreenArrayIndex += 1;
            this.Fullscreen = true;
            if(fullScreenArrayIndex >= fullScreenOption.Length)
            {
                fullScreenArrayIndex = 0;
                this.Fullscreen = false;
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new OptionsState(game, graphDevice, content));
        }

        private void ApplyChangesButton_Click(object sender, EventArgs e)
        {
            File.WriteAllText("Content/Settings/settings.json", JsonConvert.SerializeObject(this));
       //     game.graphics.IsFullScreen = Settings.Instance.Fullscreen;
       //     game.graphics.ApplyChanges();
            // game.Exit();
        }



        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud)
        {
            //Drawing each of the menu buttons
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.videoSettingsBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            spriteBatch.Begin();
          //  public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);
            spriteBatch.DrawString(textFont, "Resolution:", new Vector2(300, 400), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, "Fullscreen:", new Vector2(300, 600), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);

            spriteBatch.DrawString(textFont, fullScreenOption[fullScreenArrayIndex], new Vector2(700, 600), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //Taking sprites away when needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
        }

        public override void Load()
        {
            dynamic videoSettings = JSONParser.ReadJsonFile("Content/Settings/settings.json");
            if(videoSettings.Fullscreen == false)
            {
                fullScreenArrayIndex = 0;
                this.Fullscreen = false;
            }
            else if(videoSettings.Fullscreen == true)
            {
                fullScreenArrayIndex = 1;
                this.Fullscreen = true;
            }

            Assets.Instance.LoadContent(game);

            game.IsMouseVisible = true;


            var btnTexture = Assets.Instance.btnTextureLarge;
            var btnText = Assets.Instance.btnText;
            var leftArrowTexture = Assets.Instance.leftArrowButton;
            var rightArrowTexture = Assets.Instance.rightArrowButton;

            var resolutionLeftArrowButton = new Button(leftArrowTexture)
            {
                CurrPosition = new Vector2(500, 390)
            };

            resolutionLeftArrowButton.Click += ResolutionButton_LeftArrow_Click;

            var resolutionRightArrowButton = new Button(rightArrowTexture)
            {
                CurrPosition = new Vector2(900, 390)
            };

            resolutionRightArrowButton.Click += ResolutionButton_RightArrow_Click;


            var fullScreenLeftArrowButton = new Button(leftArrowTexture)
            {
                CurrPosition = new Vector2(500, 590)
            };

            fullScreenLeftArrowButton.Click += FullScreenButton_LeftArrow_Click;

            var fullScreenRightArrowButton = new Button(rightArrowTexture)
            {
                CurrPosition = new Vector2(900, 590)
            };

            fullScreenRightArrowButton.Click += FullScreenButton_RightArrow_Click;




            var settingsButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 950),
                BtnText = "Return To Settings",
            };

            settingsButton.Click += SettingsButton_Click;

            var applyChangesButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(700, 950),
                BtnText = "Apply Changes",
            };

            applyChangesButton.Click += ApplyChangesButton_Click;



            components = new List<Component>() {
                resolutionLeftArrowButton,
                resolutionRightArrowButton,
                fullScreenLeftArrowButton,
                fullScreenRightArrowButton,
                settingsButton,
                applyChangesButton,
            };

            textFont = btnText;





        }
    }
}