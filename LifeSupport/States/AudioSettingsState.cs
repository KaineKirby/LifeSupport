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
    public class AudioSettingsState : State
    {
        // Check to see if audio levels were changed
        public static bool isVolumeChanged= false;


        private List<Component> components;
        private SpriteFont textFont;
        private Vector2 SfxTextPosition;
        private Vector2 SfxValuePosition;
        private Vector2 MusicTextPosition;
        private Vector2 MusicValuePosition;

        private double[] sfxVolumeLevels = new double[11];
        private int sfxIndex;
        private string sfxText = Settings.Instance.SfxVolume.ToString();

        private double[] musicVolumeLevels = new double[11];
        private int musicIndex;
        private string musicText = Settings.Instance.MusVolume.ToString();



        public double SfxVolume { get; set; }
        public double MusVolume { get; set; }



        public AudioSettingsState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

        }





        public override void Load()
        {
            SfxVolume = Settings.Instance.SfxVolume;
            MusVolume = Settings.Instance.MusVolume;

            for(int i = 0; i < sfxVolumeLevels.Length; i++)
            {
                sfxVolumeLevels[i] = i * 10;
            }
            for (int i = 0; i < musicVolumeLevels.Length; i++)
            {
                musicVolumeLevels[i] = i * 10;
            }
            sfxIndex = (int)Settings.Instance.SfxVolume / 10;
            sfxText = (sfxIndex * 10).ToString();
            musicIndex = (int)Settings.Instance.MusVolume / 10;
            musicText = (musicIndex * 10).ToString();


            Assets.Instance.LoadContent(game);
            game.IsMouseVisible = true;


            var btnTexture = Assets.Instance.btnTextureLarge;
            var btnText = Assets.Instance.btnText;
            var leftArrowTexture = Assets.Instance.leftArrowButton;
            var rightArrowTexture = Assets.Instance.rightArrowButton;



            var sfxLeftArrowButton = new Button(leftArrowTexture)
            {
                CurrPosition = new Vector2(500, 390)
            };
            sfxLeftArrowButton.Click += SFX_LeftArrow_Click;


            var sfxRightArrowButton = new Button(rightArrowTexture)
            {
                CurrPosition = new Vector2(900, 390)
            };
            sfxRightArrowButton.Click += SFX_RightArrow_Click;


            var musicLeftArrowButton = new Button(leftArrowTexture)
            {
                CurrPosition = new Vector2(500, 590)
            };
            musicLeftArrowButton.Click += Music_LeftArrow_Click;


            var musicRightArrowButton = new Button(rightArrowTexture)
            {
                CurrPosition = new Vector2(900, 590)
            };
            musicRightArrowButton.Click += Music_RightArrow_Click;


            var settingsButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 950),
                BtnText = "Return To Settings",
            };
            settingsButton.Click += SettingsButton_Click;


            var applyVolumeChangesButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(700, 950),
                BtnText = "Save Changes",
            };
            applyVolumeChangesButton.Click += ApplyVolumeChangesButton_Click;


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





        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud)
        {
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
            bg.Draw(Assets.Instance.audioSettingsBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920, (float)Settings.Instance.Height / 1080, 1.0f));
            spriteBatch.DrawString(textFont, "SFX Volume:", SfxTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, "Music Volume:", MusicTextPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, sfxText + "%", SfxValuePosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(textFont, musicText +"%", MusicValuePosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
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





        private void ApplyVolumeChangesButton_Click(object sender, EventArgs e)
        {
            File.WriteAllText("Content/Settings/Volume_Settings.json", JsonConvert.SerializeObject(this));
            Settings.Instance.reloadAudioSettings();
            isVolumeChanged = true;
        }









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






        private void SettingsButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new OptionsState(game, graphDevice, content));
        }



    }
}
