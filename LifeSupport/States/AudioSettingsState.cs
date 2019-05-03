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
    // This class contains all the functionality for the audio settings page
    public class AudioSettingsState : State
    {
        // Check to see if audio levels were changed
        public static bool isVolumeChanged= false;

        /*Attributes*/

        // List of buttons on the screen
        private List<Component> components;

        // Font
        private SpriteFont textFont;

        // These vectors hold the position of different text on the screen
        private Vector2 SfxTextPosition;
        private Vector2 SfxValuePosition;
        private Vector2 MusicTextPosition;
        private Vector2 MusicValuePosition;

        // SFX volume levels (0 to 100) stored as an array to draw on screen 
        private double[] sfxVolumeLevels = new double[11];
        private int sfxIndex;
        private string sfxText = Settings.Instance.SfxVolume.ToString();

        // Music volume levels (0 to 100) stored as an array to draw on screen 
        private double[] musicVolumeLevels = new double[11];
        private int musicIndex;
        private string musicText = Settings.Instance.MusVolume.ToString();


        /*Properties*/

        // These two properties will be used to write and save to Volume_Settings.json
        public double SfxVolume { get; set; }
        public double MusVolume { get; set; }


        /*Constructor*/
        public AudioSettingsState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

        }



        /*Methods*/


        // Load everything in this function before the page is drawn or updated
        public override void Load()
        {
            // Retrieve the volume levels from the settings singleton
            SfxVolume = Settings.Instance.SfxVolume;
            MusVolume = Settings.Instance.MusVolume;


            // Fill both volume arrays with values between 0 and 100 (multiples of 10)
            for(int i = 0; i < sfxVolumeLevels.Length; i++)
            {
                sfxVolumeLevels[i] = i * 10;
            }
            for (int i = 0; i < musicVolumeLevels.Length; i++)
            {
                musicVolumeLevels[i] = i * 10;
            }

            // Display the correct volume level when going back to this page
            sfxIndex = (int)Settings.Instance.SfxVolume / 10;
            sfxText = (sfxIndex * 10).ToString();
            musicIndex = (int)Settings.Instance.MusVolume / 10;
            musicText = (musicIndex * 10).ToString();


            // Load assets
            Assets.Instance.LoadContent(game);
            game.IsMouseVisible = true;
            var btnTexture = Assets.Instance.btnTextureLarge;
            var btnText = Assets.Instance.btnText;
            var leftArrowTexture = Assets.Instance.leftArrowButton;
            var rightArrowTexture = Assets.Instance.rightArrowButton;


            // Left arrow button
            // This will decrement the sfx volume when pressed
            var sfxLeftArrowButton = new Button(leftArrowTexture)
            {
                CurrPosition = new Vector2(500, 390)
            };
            sfxLeftArrowButton.Click += SFX_LeftArrow_Click;


            // Right arrow button
            // This will increment the sfx volume when pressed
            var sfxRightArrowButton = new Button(rightArrowTexture)
            {
                CurrPosition = new Vector2(900, 390)
            };
            sfxRightArrowButton.Click += SFX_RightArrow_Click;


            // Left Arrow button
            // This will decrement the music volume when pressed
            var musicLeftArrowButton = new Button(leftArrowTexture)
            {
                CurrPosition = new Vector2(500, 590)
            };
            musicLeftArrowButton.Click += Music_LeftArrow_Click;


            // Right arrow button
            // This will increment the music volume when pressed
            var musicRightArrowButton = new Button(rightArrowTexture)
            {
                CurrPosition = new Vector2(900, 590)
            };
            musicRightArrowButton.Click += Music_RightArrow_Click;


            // Return to settings button
            var settingsButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 950),
                BtnText = "Return To Settings",
            };
            settingsButton.Click += SettingsButton_Click;


            // Save and apply new volume levels
            var applyVolumeChangesButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(700, 950),
                BtnText = "Save Changes",
            };
            applyVolumeChangesButton.Click += ApplyVolumeChangesButton_Click;


            // List of all the buttons on this page
            components = new List<Component>() {
                sfxLeftArrowButton,
                sfxRightArrowButton,
                musicLeftArrowButton,
                musicRightArrowButton,
                settingsButton,
                applyVolumeChangesButton,
            };


            textFont = btnText;
            SfxTextPosition = new Vector2(300, 400);
            MusicTextPosition = new Vector2(300, 600);
            SfxValuePosition= new Vector2(740, 400);
            MusicValuePosition = new Vector2(740, 600);
        }




        // Draw everything on the page
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg)
        {
            // Draw the background
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.audioSettingsBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            // Draw the text
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920, (float)Settings.Instance.Height / 1080, 1.0f));
            spriteBatch.DrawString(textFont, "SFX Volume:", SfxTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, "Music Volume:", MusicTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, sfxText + "%", SfxValuePosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, musicText +"%", MusicValuePosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.End();

            // Draw the buttons
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




        // Update all buttons
        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
            {
                component.Update(gameTime);
            }
        }




        // When "Save Changes' is pressed, this funtion will save the new sound levels and write it to Volume_Settings.json
        // The new settings will be applied
        private void ApplyVolumeChangesButton_Click(object sender, EventArgs e)
        {
            File.WriteAllText("Content/Settings/Volume_Settings.json", JsonConvert.SerializeObject(this));
            Settings.Instance.ReloadAudioSettings();
            isVolumeChanged = true;
        }








        // Lower Sfx volume
        private void SFX_LeftArrow_Click(object sender, EventArgs e)
        {
            if(sfxIndex == 0)
            {
                return;
            }
            sfxIndex -= 1;
            this.SfxVolume = sfxVolumeLevels[sfxIndex];
            sfxText = SfxVolume.ToString();
        }




        // Raise sfx volume
        private void SFX_RightArrow_Click(object sender, EventArgs e)
        {
            if (sfxIndex == 10)
            {
                return;
            }
            sfxIndex += 1;
            this.SfxVolume = sfxVolumeLevels[sfxIndex];
            sfxText = SfxVolume.ToString();
        }




        // Lower music volume
        private void Music_LeftArrow_Click(object sender, EventArgs e)
        {
            if(musicIndex == 0)
            {
                return;
            }
            musicIndex -= 1;
            this.MusVolume = musicVolumeLevels[musicIndex];
            musicText = MusVolume.ToString();
        }




        // Raise music volume
        private void Music_RightArrow_Click(object sender, EventArgs e)
        {
            if (musicIndex == 10)
            {
                return;
            }
            musicIndex += 1;
            this.MusVolume = musicVolumeLevels[musicIndex];
            musicText = MusVolume.ToString();
        }





        // Return to the options page
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new OptionsState(game, graphDevice, content));
        }



    }
}
