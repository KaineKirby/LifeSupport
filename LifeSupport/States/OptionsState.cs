using LifeSupport.Config;
using LifeSupport.Controls;
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
   public  class OptionsState : State
   { 
        private MainGame game;
    private SpriteBatch bg;
    private List<Component> components;
    public OptionsState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
    {
        this.game = game;
    }


    private void VideoButton_Click(object sender, EventArgs e)
    {
         game.ChangeState(new VideoSettingsState(game, graphDevice, content));
    }

    private void AudioButton_Click(object sender, EventArgs e)
    {
            game.ChangeState(new AudioSettingsState(game, graphDevice, content));
    }

    private void ControlsButton_Click(object sender, EventArgs e)
    {
            game.ChangeState(new ControlSettingsState(game, graphDevice, content));
    }

    private void MainMenuButton_Click(object sender, EventArgs e)
    {
         game.ChangeState(new MenuState(game, graphDevice, content));
    }



    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud)
    {
        //Drawing each of the menu buttons
        bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width / 1920));
        bg.Draw(Assets.Instance.optionsBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
        bg.End();

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
        Assets.Instance.LoadContent(game);

        game.IsMouseVisible = true;


        var btnTexture = Assets.Instance.btnTextureLarge;
        var btnText = Assets.Instance.btnText;
        //Setting values for each of the Menu Buttons
        var videoButton = new Button(btnTexture, btnText)
        {
            CurrPosition = new Vector2(200, 400),
            BtnText = "Video",
        };

        videoButton.Click += VideoButton_Click;

        var audioButton = new Button(btnTexture, btnText)
        {
            CurrPosition = new Vector2(200, 550),
            BtnText = "Audio",
        };

        audioButton.Click += AudioButton_Click;

        var controlsButton = new Button(btnTexture, btnText)
        {
            CurrPosition = new Vector2(200, 700),
            BtnText = "Controls",
        };

        controlsButton.Click += ControlsButton_Click;

        var menuButton = new Button(btnTexture, btnText)
        {
            CurrPosition = new Vector2(200, 850),
            BtnText = "Return To Main Menu",
        };

        menuButton.Click += MainMenuButton_Click;

        components = new List<Component>() {
                videoButton,
                audioButton,
                controlsButton,
                menuButton,
            };
    }
}
}