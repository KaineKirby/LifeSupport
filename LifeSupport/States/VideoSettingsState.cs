using LifeSupport.Config;
using LifeSupport.Controls;
using LifeSupport.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
    // This class contains all the content for the video settings screen
    public class VideoSettingsState : State
    {
        /*Attributes*/

        // Check to see if the video resolution is changed
        public static bool isVideoChanged = false;

        // Store all the buttons in this list
        private List<Component> components;


        private SpriteFont textFont;
        private Vector2 resolutionTextPosition;
        private Vector2 resolutionValuePosition;
        private Vector2 fullScreenTextPosition;
        private Vector2 fullScreenValuePosition;

        // This list contains the default (and only) screen dimensions available for LifeSupport
        private string[] screenDimensions = new string[] { "1024x576", "1152x648", "1280x720", "1366x768", "1600x900", "1920x1080" };

        // Used to access items from screenDimensions
        private int dimensionsIndex;

        // this list contains the two fullscreen options
        private string[] fullScreenOption = new string[] { "Disabled", "Enabled" };

        // Used to access items from fullScreenOption
        private int fullScreenIndex;


        /*Properties*/

        // These properties are used to read and write Video settings from/to Video_Settings.json, and their
        // values are stored within the settings singleton 
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Fullscreen { get; set; }


        /*Constructor*/
        public VideoSettingsState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

        }

        /*Methods*/

        // Load the following content first 
        public override void Load()
        {
            // Assign the video properties values currently saved in the Settings singleton
            Width = Settings.Instance.Width;
            Height = Settings.Instance.Height;
            Fullscreen = Settings.Instance.Fullscreen;

            
            if (Settings.Instance.Fullscreen == false){
                fullScreenIndex = 0;
            }
            else if (Settings.Instance.Fullscreen == true){
                fullScreenIndex = 1;
            }

            // Retrieve the correct screenDimensions index
            dimensionsIndex = returnDimensionsIndex(screenDimensions, Settings.Instance.Width, Settings.Instance.Height);

            //Load assets
            Assets.Instance.LoadContent(game);
            game.IsMouseVisible = true;
            var btnTexture = Assets.Instance.btnTextureLarge;
            var btnText = Assets.Instance.btnText;
            var leftArrowTexture = Assets.Instance.leftArrowButton;
            var rightArrowTexture = Assets.Instance.rightArrowButton;


            // Left arrow button
            // Moves down the screenDimensions array
            var resolutionLeftArrowButton = new Button(leftArrowTexture)
            {
                CurrPosition = new Vector2(500, 390)
            };
            resolutionLeftArrowButton.Click += ResolutionButton_LeftArrow_Click;

            
            // Right arrow button
            // Moves up the screenDimensions array
            var resolutionRightArrowButton = new Button(rightArrowTexture)
            {
                CurrPosition = new Vector2(900, 390)
            };
            resolutionRightArrowButton.Click += ResolutionButton_RightArrow_Click;


            // Left arrow button
            // Moves down the fullScreen array
            var fullScreenLeftArrowButton = new Button(leftArrowTexture)
            {
                CurrPosition = new Vector2(500, 590)
            };
            fullScreenLeftArrowButton.Click += FullScreenButton_LeftArrow_Click;


            // Right arrow button
            // Moves up the fullScreen array
            var fullScreenRightArrowButton = new Button(rightArrowTexture)
            {
                CurrPosition = new Vector2(900, 590)
            };
            fullScreenRightArrowButton.Click += FullScreenButton_RightArrow_Click;


            // Return to settings button
            var settingsButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 950),
                BtnText = "Return To Settings",
            };
            settingsButton.Click += SettingsButton_Click;


            // Save and apply video settings button
            var applyChangesButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(700, 950),
                BtnText = "Save Changes",
            };
            applyChangesButton.Click += ApplyChangesButton_Click;


            // Instantaite buttons
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




        // Draw all content onto the screen
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg)
        {

            // Draw the background
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.videoSettingsBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            // Draw all the text
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920, (float)Settings.Instance.Height / 1080, 1.0f));
            spriteBatch.DrawString(textFont, "Resolution:", resolutionTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, "Fullscreen:", resolutionValuePosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, screenDimensions[dimensionsIndex], fullScreenTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, fullScreenOption[fullScreenIndex], fullScreenValuePosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.End();

            // Draw all the buttons
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }





        public override void PostUpdate(GameTime gameTime)
        {

        }




        // Update all the buttons
        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
        }




        // Write all the values currently set in each property (Width, Height, Fullscreen) to the Video_Settings.json file
        // Reload the settings singleton
        private void ApplyChangesButton_Click(object sender, EventArgs e)
        {
            File.WriteAllText("Content/Settings/Video_Settings.json", JsonConvert.SerializeObject(this));
            Settings.Instance.ReloadVideoSettings();
            isVideoChanged = true;
        }




        // If the left arrow button is pressed (next to the resolution option), go down the screenDimension array
        // If the button is pressed at index 0 go to the end of the array (array.length -1)
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



        // If the right arrow button is pressed (next to the resolution option), go up the screenDimension array
        // If the button is pressed at index array.length-1, go to the beginning of the array (index 0)
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




        // Change fullscreen setting
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




        // Change fullscreen setting
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





        // Go back to options page
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new OptionsState(game, graphDevice, content));
        }




        // This function is used to return the correct video dimension index from the screenDimensions array
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