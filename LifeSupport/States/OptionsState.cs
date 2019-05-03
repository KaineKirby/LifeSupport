using LifeSupport.Config;
using LifeSupport.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.States
{
    // This class contains all the content within the options page
   public  class OptionsState : State
   {

    /*Attributes*/

    // Check to see if the game was opened from the pause menu
    // This will display a different button if it is true
    public static bool openedFromPause = false;

    private MainGame game;
    private SpriteBatch bg;

    // The list of all buttons displayed on this screen
    private List<Component> components;

    /*Constructor*/
    public OptionsState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
    {
        this.game = game;
    }


    /*Methods*/


    // This function (button) takes you to the video settings page
    private void VideoButton_Click(object sender, EventArgs e)
    {
         game.ChangeState(new VideoSettingsState(game, graphDevice, content));
    }
   
    // This funciton (button) takes you to the audio settings page
    private void AudioButton_Click(object sender, EventArgs e)
    {
            game.ChangeState(new AudioSettingsState(game, graphDevice, content));
    }

    // This function (button) takes you to the control settings page
    private void ControlsButton_Click(object sender, EventArgs e)
    {
            game.ChangeState(new ControlSettingsState(game, graphDevice, content));
    }

    // This function (button) takes you back to main menu
    private void MainMenuButton_Click(object sender, EventArgs e)
    {
         game.ChangeState(new MenuState(game, graphDevice, content));
    }

    // This function (button) takes you back to the pause menu (only visible if you opened the options screen after pausing the game)
    private void PauseButton_Click(object sender, EventArgs e)
    {
         game.ChangeState(new PauseState(game, graphDevice, content));
    }



    // The draw function that draws everyting on the options page
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg)
    {
        // Draw the background
        bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
        bg.Draw(Assets.Instance.optionsBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
        bg.End();

        // Draw each button
        spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
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

    // Update each button
    public override void Update(GameTime gameTime)
    {
        foreach (var component in components)
        {
            component.Update(gameTime);
        }
    }



   // Load all content onto the option page when it is opened
    public override void Load()
    {
        // Load all assets
        Assets.Instance.LoadContent(game);
        game.IsMouseVisible = true;
        var btnTexture = Assets.Instance.btnTextureLarge;
        var btnText = Assets.Instance.btnText;


        // Go to video settings button
        var videoButton = new Button(btnTexture, btnText)
        {
            CurrPosition = new Vector2(200, 400),
            BtnText = "Video",
        }; 
        videoButton.Click += VideoButton_Click;


        // Go to audio settings button
        var audioButton = new Button(btnTexture, btnText)
        {
            CurrPosition = new Vector2(200, 550),
            BtnText = "Audio",
        };
        audioButton.Click += AudioButton_Click;


        // Go to control settings button
        var controlsButton = new Button(btnTexture, btnText)
        {
            CurrPosition = new Vector2(200, 700),
            BtnText = "Controls",
        };
        controlsButton.Click += ControlsButton_Click;


        // Go to main menu button
        var menuButton = new Button(btnTexture, btnText)
        {
            CurrPosition = new Vector2(200, 850),
            BtnText = "Return To Main Menu",
        };
        menuButton.Click += MainMenuButton_Click;


            // Go to pause screen button
            var pauseButton = new Button(btnTexture, btnText)
            {
                CurrPosition = new Vector2(200, 850),
                BtnText = "Return To Pause Screen",
            };
            pauseButton.Click += PauseButton_Click;

            // If options was accessed from main menu
            if (openedFromPause == false)
            {
                components = new List<Component>() {
                videoButton,
                audioButton,
                controlsButton,
                menuButton,
            };
            }

            // If options was accessed from pause
            else
            {
                components = new List<Component>() {
                videoButton,
                audioButton,
                controlsButton,
                pauseButton,
            };
            }
    }
}
}