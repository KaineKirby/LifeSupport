using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeSupport.Config ;
using LifeSupport.GameObjects ;
using LifeSupport.Levels;
using LifeSupport.Utilities;
using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using LifeSupport.States;
using Microsoft.Xna.Framework.Media;
using Penumbra;

namespace LifeSupport
{

    public class MainGame : Game {

        public bool changedDisplay;
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteBatch bg;
        public SpriteBatch hud;
        public SpriteBatch fg ;

        private State prevState;
        private State currState;
        private State nextState;

        public PenumbraComponent penumbra ;

        public void ChangeState(State state) {

            if (currState is GameState) {
                prevState = currState;
            }
            nextState = state ;
            nextState.Load() ;

            //music and light management
            if (nextState is GameState && currState is MenuState) {
                MediaPlayer.Stop() ;
                MediaPlayer.Play(((GameState)nextState).GetSong()) ;
                MediaPlayer.Volume = (float)Settings.Instance.MusVolume/100 ;
                //penumbra.Lights.Remove(menuLight) ;
            }
            else if (nextState is MenuState && ((currState is PauseState) || (currState is VictoryState) || (currState is DeathState))) {
                MediaPlayer.Stop() ;
                MediaPlayer.Play(Assets.Instance.menuMusic) ;
                MediaPlayer.Volume = (float)Settings.Instance.MusVolume/100 ;
            }

        }

        public void returnToGame(State state) {
            nextState = prevState;
            ((GameState)state).RecaculateScale() ;
        }

        public MainGame() {

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Settings.Instance.Height;
            graphics.PreferredBackBufferWidth = Settings.Instance.Width;
            graphics.IsFullScreen = Settings.Instance.Fullscreen;
            graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;

            this.penumbra = new PenumbraComponent(this) ;
            Components.Add(penumbra) ;

            Content.RootDirectory = "Content";
        }


        protected override void Initialize() {
            base.Initialize();
        }


        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bg = new SpriteBatch(GraphicsDevice);
            fg = new SpriteBatch(GraphicsDevice) ;
            hud = new SpriteBatch(GraphicsDevice);
            currState = new MenuState(this, graphics.GraphicsDevice, Content);
            currState.Load();

            MediaPlayer.Play(Assets.Instance.menuMusic) ;
            MediaPlayer.Volume = (float)Settings.Instance.MusVolume/100 ;
            MediaPlayer.IsRepeating = true ;

            penumbra.AmbientColor = Color.Black ;

        }


        protected override void UnloadContent() {

        }


        protected override void Update(GameTime gameTime) {
            if (nextState != null) {
                currState = nextState;
                nextState = null;
            }
            currState.Update(gameTime);
            currState.PostUpdate(gameTime);
            if (VideoSettingsState.isVideoChanged == true) {
                graphics.PreferredBackBufferHeight = Settings.Instance.Height;
                graphics.PreferredBackBufferWidth = Settings.Instance.Width;
                graphics.IsFullScreen = Settings.Instance.Fullscreen;
                graphics.ApplyChanges();
                VideoSettingsState.isVideoChanged = false;
            }
            else if(AudioSettingsState.isVolumeChanged == true) {
                AudioSettingsState.isVolumeChanged = false;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {

            GraphicsDevice.Clear(Color.Black);
            currState.Draw(gameTime, spriteBatch, bg, hud, fg);

        }
    }
}