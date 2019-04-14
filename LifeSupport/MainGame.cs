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

namespace LifeSupport {

    public class MainGame : Game {

        public bool changedDisplay;
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteBatch bg;
        public SpriteBatch hud;


        private State currState;
        private State nextState;

        public void ChangeState(State state) {
            nextState = state;
            nextState.Load();
        }



        public MainGame() {

            graphics = new GraphicsDeviceManager(this) ;
            graphics.PreferredBackBufferHeight = Settings.Instance.Height;
            graphics.PreferredBackBufferWidth = Settings.Instance.Width;
            graphics.IsFullScreen = Settings.Instance.Fullscreen;
            graphics.SynchronizeWithVerticalRetrace = false ;
            this.IsFixedTimeStep = false ;

            Content.RootDirectory = "Content";
        }


        protected override void Initialize() {
            base.Initialize();
        }


        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bg = new SpriteBatch(GraphicsDevice);
            hud = new SpriteBatch(GraphicsDevice);
            currState = new MenuState(this, graphics.GraphicsDevice, Content);
            currState.Load();


        }


        protected override void UnloadContent() {

        }


        protected override void Update(GameTime gameTime) {

            if(nextState != null) {
               currState = nextState;
               nextState = null;
            }
             currState.Update(gameTime);
             currState.PostUpdate(gameTime);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            currState.Draw(gameTime, spriteBatch, bg, hud);

            base.Draw(gameTime);
        }
    }
}
