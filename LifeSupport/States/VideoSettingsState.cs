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
        // Check to see if the video resolution is changed
        public static bool isVideoChanged = false;


        private List<Component> components;
        private SpriteFont textFont;
        private Vector2 resolutionTextPosition;
        private Vector2 resolutionValuePosition;
        private Vector2 fullScreenTextPosition;
        private Vector2 fullScreenValuePosition;

        private string[] screenDimensions = new string[] { "1024x576", "1152x648", "1280x720", "1366x768", "1600x900", "1920x1080" };
        private int dimensionsIndex;
        private string[] fullScreenOption = new string[] { "Disabled", "Enabled" };
        private int fullScreenIndex;



        public int Width { get; set; }
        public int Height { get; set; }
        public bool Fullscreen { get; set; }


        public VideoSettingsState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

        }




        public override void Load()
        {
            Width = Settings.Instance.Width;
            Height = Settings.Instance.Height;
            Fullscreen = Settings.Instance.Fullscreen;

            if (Settings.Instance.Fullscreen == false){
                fullScreenIndex = 0;
            }
            else if (Settings.Instance.Fullscreen == true){
                fullScreenIndex = 1;
            }

            dimensionsIndex = returnDimensionsIndex(screenDimensions, Settings.Instance.Width, Settings.Instance.Height);


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
                BtnText = "Save Changes",
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
            resolutionTextPosition = new Vector2(300, 400);
            resolutionValuePosition = new Vector2(300, 600);
            fullScreenTextPosition = new Vector2(700, 400);
            fullScreenValuePosition = new Vector2(700, 600);
        }





        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg)
        {
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.videoSettingsBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920, (float)Settings.Instance.Height / 1080, 1.0f));
            spriteBatch.DrawString(textFont, "Resolution:", resolutionTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, "Fullscreen:", resolutionValuePosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, screenDimensions[dimensionsIndex], fullScreenTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, fullScreenOption[fullScreenIndex], fullScreenValuePosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
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

        }





        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
        }





        private void ApplyChangesButton_Click(object sender, EventArgs e)
        {
            File.WriteAllText("Content/Settings/Video_Settings.json", JsonConvert.SerializeObject(this));
            Settings.Instance.reloadVideoSettings();
            isVideoChanged = true;
        }









        private void ResolutionButton_LeftArrow_Click(object sender, EventArgs e)
        {
            dimensionsIndex -= 1;
            string dimension;
            string[] widthAndHeight;

            switch (dimensionsIndex)
            {
                case -1:
                    dimensionsIndex = 5;
                    dimension = screenDimensions[dimensionsIndex];
                    widthAndHeight = dimension.Split('x');
                    this.Width = Int32.Parse(widthAndHeight[0]);
                    this.Height = Int32.Parse(widthAndHeight[1]);
                    break;
                case 0:
                    dimension = screenDimensions[dimensionsIndex];
                    widthAndHeight = dimension.Split('x');
                    this.Width = Int32.Parse(widthAndHeight[0]);
                    this.Height = Int32.Parse(widthAndHeight[1]);
                    break;
                case 1:
                    dimension = screenDimensions[dimensionsIndex];
                    widthAndHeight = dimension.Split('x');
                    this.Width = Int32.Parse(widthAndHeight[0]);
                    this.Height = Int32.Parse(widthAndHeight[1]);
                    break;
                case 2:
                    dimension = screenDimensions[dimensionsIndex];
                    widthAndHeight = dimension.Split('x');
                    this.Width = Int32.Parse(widthAndHeight[0]);
                    this.Height = Int32.Parse(widthAndHeight[1]);
                    break;
                case 3:
                    dimension = screenDimensions[dimensionsIndex];
                    widthAndHeight = dimension.Split('x');
                    this.Width = Int32.Parse(widthAndHeight[0]);
                    this.Height = Int32.Parse(widthAndHeight[1]);
                    break;
                case 4:
                    dimension = screenDimensions[dimensionsIndex];
                    widthAndHeight = dimension.Split('x');
                    this.Width = Int32.Parse(widthAndHeight[0]);
                    this.Height = Int32.Parse(widthAndHeight[1]);
                    break;
                default:
                    break;
            }
        }





        private void ResolutionButton_RightArrow_Click(object sender, EventArgs e)
        {
            dimensionsIndex += 1;
            string dimension;
            string[] widthAndHeight;

            switch (dimensionsIndex)
            {
                case 1:
                    dimension = screenDimensions[dimensionsIndex];
                    widthAndHeight = dimension.Split('x');
                    this.Width = Int32.Parse(widthAndHeight[0]);
                    this.Height = Int32.Parse(widthAndHeight[1]);
                    break;
                case 2:
                    dimension = screenDimensions[dimensionsIndex];
                    widthAndHeight = dimension.Split('x');
                    this.Width = Int32.Parse(widthAndHeight[0]);
                    this.Height = Int32.Parse(widthAndHeight[1]);
                    break;
                case 3:
                    dimension = screenDimensions[dimensionsIndex];
                    widthAndHeight = dimension.Split('x');
                    this.Width = Int32.Parse(widthAndHeight[0]);
                    this.Height = Int32.Parse(widthAndHeight[1]);
                    break;
                case 4:
                    dimension = screenDimensions[dimensionsIndex];
                    widthAndHeight = dimension.Split('x');
                    this.Width = Int32.Parse(widthAndHeight[0]);
                    this.Height = Int32.Parse(widthAndHeight[1]);
                    break;
                case 5:
                    dimension = screenDimensions[dimensionsIndex];
                    widthAndHeight = dimension.Split('x');
                    this.Width = Int32.Parse(widthAndHeight[0]);
                    this.Height = Int32.Parse(widthAndHeight[1]);
                    break;
                case 6:
                    dimensionsIndex = 0;
                    dimension = screenDimensions[dimensionsIndex];
                    widthAndHeight = dimension.Split('x');
                    this.Width = Int32.Parse(widthAndHeight[0]);
                    this.Height = Int32.Parse(widthAndHeight[1]);
                    break;
                default:
                    break;
            }
        }





        private void FullScreenButton_LeftArrow_Click(object sender, EventArgs e)
        {
            fullScreenIndex -= 1;

            switch (fullScreenIndex)
            {
                case -1:
                    fullScreenIndex = 1;
                    this.Fullscreen = true;
                    break;
                case 0:
                    this.Fullscreen = false;
                    break;
                default:
                    break;
            }
        }





        private void FullScreenButton_RightArrow_Click(object sender, EventArgs e)
        {
            fullScreenIndex += 1;

            switch (fullScreenIndex)
            {
                case 1:
                    this.Fullscreen = true;
                    break;
                case 2:
                    fullScreenIndex = 0;
                    this.Fullscreen = false;
                    break;
                default:
                    break;
            }
        }






        private void SettingsButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new OptionsState(game, graphDevice, content));
        }





        public int returnDimensionsIndex(string[] array, int width, int height)
        {
            string dimensionW = width.ToString();
            string dimensionH = height.ToString();
            string x = "x";

            string widthX = string.Concat(dimensionW, x);
            string dimensions = string.Concat(widthX, dimensionH);

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(dimensions))
                {
                    return i;
                }
            }
            return 0;
        }


    }
}